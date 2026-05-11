import { apiRequest } from "./client";
import type { Paged, Review } from "./types";

export interface ReviewPayload {
  rate: number;
  text: string;
}

export const reviewsApi = {
  byVideo: (videoId: string, params: { pageNumber: number; pageSize: number }) =>
    apiRequest<Paged<Review>>(`/api/videos/${videoId}/reviews`, { query: params }),
  myReviews: (params: { pageNumber: number; pageSize: number }) =>
    apiRequest<Paged<Review>>(`/api/users/me/reviews`, { query: params }),
  create: (videoId: string, payload: ReviewPayload) =>
    apiRequest<Review>(`/api/videos/${videoId}/reviews`, {
      method: "POST",
      body: payload,
    }),
  update: (videoId: string, payload: ReviewPayload) =>
    apiRequest<Review>(`/api/videos/${videoId}/reviews/me`, {
      method: "PUT",
      body: payload,
    }),
  remove: (videoId: string) =>
    apiRequest<void>(`/api/videos/${videoId}/reviews/me`, { method: "DELETE" }),
};
