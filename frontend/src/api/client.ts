import type { ApiError } from "./types";

type AccessTokenGetter = () => string | null;
type SessionRefresher = () => Promise<{ accessToken: string } | null>;
type SessionClearer = () => void;

let getAccessToken: AccessTokenGetter = () => null;
let refreshSession: SessionRefresher = async () => null;
let clearSession: SessionClearer = () => {};

export function configureApiClient(handlers: {
  getAccessToken: AccessTokenGetter;
  refreshSession: SessionRefresher;
  clearSession: SessionClearer;
}) {
  getAccessToken = handlers.getAccessToken;
  refreshSession = handlers.refreshSession;
  clearSession = handlers.clearSession;
}

export class HttpError extends Error {
  status: number;
  payload: ApiError | null;

  constructor(status: number, message: string, payload: ApiError | null) {
    super(message);
    this.status = status;
    this.payload = payload;
  }
}

interface RequestOptions {
  method?: string;
  body?: unknown;
  query?: Record<string, string | number | boolean | undefined | null>;
  skipAuthRetry?: boolean;
  skipAuth?: boolean;
}

function buildUrl(path: string, query?: RequestOptions["query"]): string {
  if (!query) return path;
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(query)) {
    if (value === undefined || value === null || value === "") continue;
    params.append(key, String(value));
  }
  const qs = params.toString();
  return qs ? `${path}?${qs}` : path;
}

async function parseResponse<T>(response: Response): Promise<T> {
  if (response.status === 204) return undefined as T;
  const contentType = response.headers.get("content-type") ?? "";
  if (contentType.includes("application/json")) {
    return (await response.json()) as T;
  }
  return (await response.text()) as unknown as T;
}

async function parseError(response: Response): Promise<ApiError | null> {
  try {
    const contentType = response.headers.get("content-type") ?? "";
    if (contentType.includes("application/json")) {
      return (await response.json()) as ApiError;
    }
  } catch {
    // ignore
  }
  return null;
}

export async function apiRequest<T>(path: string, options: RequestOptions = {}): Promise<T> {
  const { method = "GET", body, query, skipAuthRetry, skipAuth } = options;

  const headers: Record<string, string> = {};
  if (body !== undefined) {
    headers["Content-Type"] = "application/json";
  }

  if (!skipAuth) {
    const token = getAccessToken();
    if (token) headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(buildUrl(path, query), {
    method,
    headers,
    credentials: "include",
    body: body !== undefined ? JSON.stringify(body) : undefined,
  });

  if (response.status === 401 && !skipAuthRetry && !skipAuth) {
    const refreshed = await refreshSession();
    if (refreshed) {
      return apiRequest<T>(path, { ...options, skipAuthRetry: true });
    }
    clearSession();
  }

  if (!response.ok) {
    const payload = await parseError(response);
    throw new HttpError(response.status, payload?.message ?? response.statusText, payload);
  }

  return parseResponse<T>(response);
}
