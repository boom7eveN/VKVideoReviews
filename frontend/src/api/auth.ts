import { apiRequest } from "./client";
import type { User } from "./types";

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresInSeconds: number;
  tokenType: string;
}

export const authApi = {
  refresh: (refreshToken: string) =>
    apiRequest<AuthTokens>("/api/auth/tokens/refresh", {
      method: "POST",
      body: { refreshToken },
      skipAuth: true,
      skipAuthRetry: true,
    }),
  me: () => apiRequest<User>("/api/users/me"),
};

export const VK_LOGIN_URL = "/api/auth/vk/login";
