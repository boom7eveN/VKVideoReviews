import { apiRequest } from "./client";
import type { Favorite, FavoriteListResponse, Paged } from "./types";

export const favoritesApi = {
  list: (params: { pageNumber: number; pageSize: number }) =>
    apiRequest<Paged<Favorite>>("/api/users/me/favorites", { query: params }),
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
