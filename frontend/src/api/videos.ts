import { apiRequest } from "./client";
import type { Paged, Video, VideosListResponse } from "./types";

export interface CreateVideoPayload {
  videoUrl: string;
  title: string;
  imageUrl: string;
  description: string;
  startYear: number;
  endYear?: number | null;
  videoTypeId: string;
  genreIds: string[];
}

export type UpdateVideoPayload = Partial<CreateVideoPayload>;

export const videosApi = {
  list: (params: { pageNumber: number; pageSize: number; titlePart?: string }) =>
    apiRequest<Paged<Video>>("/api/videos", { query: params }),
  byId: async (videoId: string): Promise<Video> => {
    const data = await apiRequest<VideosListResponse>(`/api/videos/${videoId}`);
    return data.videos[0];
  },
  create: async (payload: CreateVideoPayload): Promise<Video> => {
    const data = await apiRequest<VideosListResponse>("/api/videos", {
      method: "POST",
      body: payload,
    });
    return data.videos[0];
  },
  update: async (videoId: string, payload: UpdateVideoPayload): Promise<Video> => {
    const data = await apiRequest<VideosListResponse>(`/api/videos/${videoId}`, {
      method: "PUT",
      body: payload,
    });
    return data.videos[0];
  },
  remove: (videoId: string) =>
    apiRequest<void>(`/api/videos/${videoId}`, { method: "DELETE" }),
};
