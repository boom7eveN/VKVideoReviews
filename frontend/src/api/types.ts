export interface User {
  userId: string;
  name: string;
  surname: string;
  avatarUrl: string;
  isAdmin: boolean;
}

export interface Genre {
  id: string;
  title: string;
}

export interface VideoType {
  id: string;
  title: string;
}

export interface Review {
  reviewId: string;
  rate: number;
  text: string;
  user: User;
  video?: Video;
  createDate: string;
  updateDate: string;
}

export interface Video {
  videoId: string;
  videoUrl: string;
  title: string;
  imageUrl: string;
  description: string;
  startYear: number;
  endYear?: number | null;
  averageRate: number;
  totalReviews: number;
  videoType: VideoType;
  genres: Genre[];
}

export interface Favorite {
  video: Video;
}

export interface Paged<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface VideosListResponse {
  videos: Video[];
}

export interface GenresListResponse {
  genres: Genre[];
}

export interface VideoTypesListResponse {
  videoTypes: VideoType[];
}

export interface FavoriteListResponse {
  favorite: Favorite[];
}

export interface ApiError {
  code: string;
  message: string;
  errors?: unknown;
}
