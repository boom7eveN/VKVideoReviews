import { createContext, useCallback, useContext, useEffect, useMemo, useRef, useState } from "react";
import type { ReactNode } from "react";
import { authApi, VK_LOGIN_URL, type AuthTokens } from "@/api/auth";
import { configureApiClient } from "@/api/client";
import type { User } from "@/api/types";

const REFRESH_TOKEN_KEY = "vkvr.refreshToken";

interface AuthState {
  user: User | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
  signIn: (tokens: AuthTokens) => Promise<User | null>;
  refresh: () => Promise<AuthTokens | null>;
}

const AuthContext = createContext<AuthState | null>(null);

function readRefreshToken(): string | null {
  try {
    return localStorage.getItem(REFRESH_TOKEN_KEY);
  } catch {
    return null;
  }
}

function writeRefreshToken(token: string | null) {
  try {
    if (token) localStorage.setItem(REFRESH_TOKEN_KEY, token);
    else localStorage.removeItem(REFRESH_TOKEN_KEY);
  } catch {
    // ignore
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const accessTokenRef = useRef<string | null>(null);
  const refreshPromiseRef = useRef<Promise<AuthTokens | null> | null>(null);

  const applyTokens = useCallback((tokens: AuthTokens | null) => {
    if (tokens) {
      accessTokenRef.current = tokens.accessToken;
      writeRefreshToken(tokens.refreshToken);
    } else {
      accessTokenRef.current = null;
      writeRefreshToken(null);
      setUser(null);
    }
  }, []);

  const refresh = useCallback(async (): Promise<AuthTokens | null> => {
    if (refreshPromiseRef.current) return refreshPromiseRef.current;

    const promise = (async () => {
      const stored = readRefreshToken();
      if (!stored) {
        applyTokens(null);
        return null;
      }
      try {
        const tokens = await authApi.refresh(stored);
        applyTokens(tokens);
        return tokens;
      } catch {
        applyTokens(null);
        return null;
      } finally {
        refreshPromiseRef.current = null;
      }
    })();

    refreshPromiseRef.current = promise;
    return promise;
  }, [applyTokens]);

  const clearSession = useCallback(() => {
    applyTokens(null);
  }, [applyTokens]);

  useEffect(() => {
    configureApiClient({
      getAccessToken: () => accessTokenRef.current,
      refreshSession: async () => {
        const tokens = await refresh();
        return tokens ? { accessToken: tokens.accessToken } : null;
      },
      clearSession,
    });
  }, [refresh, clearSession]);

  const signIn = useCallback(
    async (tokens: AuthTokens): Promise<User | null> => {
      applyTokens(tokens);
      try {
        const me = await authApi.me();
        setUser(me);
        return me;
      } catch {
        applyTokens(null);
        return null;
      }
    },
    [applyTokens],
  );

  useEffect(() => {
    let cancelled = false;
    (async () => {
      const tokens = await refresh();
      if (cancelled) return;
      if (tokens) {
        try {
          const me = await authApi.me();
          if (!cancelled) setUser(me);
        } catch {
          if (!cancelled) applyTokens(null);
        }
      }
      if (!cancelled) setIsLoading(false);
    })();
    return () => {
      cancelled = true;
    };
  }, [refresh, applyTokens]);

  const login = useCallback(() => {
    window.location.href = VK_LOGIN_URL;
  }, []);

  const logout = useCallback(() => {
    applyTokens(null);
  }, [applyTokens]);

  const value = useMemo<AuthState>(
    () => ({
      user,
      isLoading,
      isAuthenticated: !!user,
      login,
      logout,
      signIn,
      refresh,
    }),
    [user, isLoading, login, logout, signIn, refresh],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthState {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
