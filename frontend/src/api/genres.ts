import { apiRequest } from "./client";
import type { Genre, GenresListResponse } from "./types";

export const genresApi = {
  list: async (): Promise<Genre[]> => {
    const data = await apiRequest<GenresListResponse>("/api/genres");
    return data.genres ?? [];
  },
  create: async (title: string): Promise<Genre> => {
    const data = await apiRequest<GenresListResponse>("/api/genres", {
      method: "POST",
      body: { title },
    });
    return data.genres[0];
  },
  update: async (id: string, title: string): Promise<Genre> => {
    const data = await apiRequest<GenresListResponse>(`/api/genres/${id}`, {
      method: "PUT",
      body: { title },
    });
    return data.genres[0];
  },
  remove: (id: string) =>
    apiRequest<void>(`/api/genres/${id}`, { method: "DELETE" }),
};
