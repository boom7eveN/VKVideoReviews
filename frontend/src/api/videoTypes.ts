import { apiRequest } from "./client";
import type { VideoType, VideoTypesListResponse } from "./types";

export const videoTypesApi = {
  list: async (): Promise<VideoType[]> => {
    const data = await apiRequest<VideoTypesListResponse>("/api/video-types");
    return data.videoTypes ?? [];
  },
  create: async (title: string): Promise<VideoType> => {
    const data = await apiRequest<VideoTypesListResponse>("/api/video-types", {
      method: "POST",
      body: { title },
    });
    return data.videoTypes[0];
  },
  update: async (id: string, title: string): Promise<VideoType> => {
    const data = await apiRequest<VideoTypesListResponse>(`/api/video-types/${id}`, {
      method: "PUT",
      body: { title },
    });
    return data.videoTypes[0];
  },
  remove: (id: string) =>
    apiRequest<void>(`/api/video-types/${id}`, { method: "DELETE" }),
};
