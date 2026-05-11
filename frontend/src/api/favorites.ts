import { apiRequest, HttpError } from "./client";
import type { Favorite, FavoriteListResponse, Paged } from "./types";

export const favoritesApi = {
  list: (params: { pageNumber: number; pageSize: number }) =>
    apiRequest<Paged<Favorite>>("/api/users/me/favorites", { query: params }),
  isFavorite: async (videoId: string): Promise<boolean> => {
    try {
      await apiRequest<FavoriteListResponse>(`/api/users/me/favorites/${videoId}`);
      return true;
    } catch (err) {
      if (err instanceof HttpError && err.status === 404) return false;
      throw err;
    }
  },
  add: async (videoId: string): Promise<Favorite> => {
    const data = await apiRequest<FavoriteListResponse>("/api/users/me/favorites", {
      method: "POST",
      body: { videoId },
    });
    return data.favorite[0];
  },
  remove: (videoId: string) =>
    apiRequest<void>(`/api/users/me/favorites/${videoId}`, { method: "DELETE" }),
};
