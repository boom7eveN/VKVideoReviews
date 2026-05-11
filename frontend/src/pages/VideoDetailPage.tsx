import { useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { videosApi } from "@/api/videos";
import { reviewsApi } from "@/api/reviews";
import { favoritesApi } from "@/api/favorites";
import { useAuth } from "@/auth/AuthContext";
import { StarRating } from "@/components/StarRating";
import { ReviewItem } from "@/components/ReviewItem";
import { Loader } from "@/components/Loader";
import { Pagination } from "@/components/Pagination";
import { HttpError } from "@/api/client";

const PAGE_SIZE = 10;

export function VideoDetailPage() {
  const { id = "" } = useParams<{ id: string }>();
  const { user, isAuthenticated } = useAuth();
  const queryClient = useQueryClient();
  const [reviewsPage, setReviewsPage] = useState(1);

  const videoQuery = useQuery({
    queryKey: ["video", id],
    queryFn: () => videosApi.byId(id),
    enabled: !!id,
  });

  const reviewsQuery = useQuery({
    queryKey: ["reviews", id, reviewsPage],
    queryFn: () =>
      reviewsApi.byVideo(id, { pageNumber: reviewsPage, pageSize: PAGE_SIZE }),
    enabled: !!id,
  });

  const myReviewInList = reviewsQuery.data?.items.find((r) => r.user.userId === user?.userId);

  const addFavorite = useMutation({
    mutationFn: () => favoritesApi.add(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["favorites"] });
    },
  });

  if (videoQuery.isLoading) return <Loader />;
  if (videoQuery.isError || !videoQuery.data)
    return (
      <div className="container main">
        <div className="alert alert-error">Видео не найдено</div>
        <Link to="/" className="btn mt-4">
          ← К каталогу
        </Link>
      </div>
    );

  const video = videoQuery.data;

  return (
    <div className="container main">
      <Link to="/" className="muted" style={{ fontSize: 14 }}>
        ← Назад к каталогу
      </Link>

      <div
        style={{
          display: "grid",
          gridTemplateColumns: "minmax(0, 340px) minmax(0, 1fr)",
          gap: 28,
          marginTop: 16,
        }}
        className="video-hero"
      >
        <div
          className="card"
          style={{
            padding: 0,
            overflow: "hidden",
            aspectRatio: "2 / 3",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            background: "linear-gradient(135deg, #1a1a2a 0%, #0e0e17 100%)",
          }}
        >
          {video.imageUrl ? (
            <img
              src={video.imageUrl}
              alt={video.title}
              style={{ width: "100%", height: "100%", objectFit: "contain", display: "block" }}
            />
          ) : (
            <div
              style={{
                width: "100%",
                height: "100%",
                background: "linear-gradient(135deg, #4d8bff, #7e5bff)",
              }}
            />
          )}
        </div>

        <div className="flex flex-col gap-3">
          <h1>{video.title}</h1>
          <div className="flex gap-3" style={{ alignItems: "center" }}>
            <StarRating value={video.averageRate} size={20} />
            <span style={{ fontWeight: 600, fontSize: 18 }}>
              {video.averageRate.toFixed(1)}
            </span>
            <span className="muted" style={{ fontSize: 14 }}>
              · {video.totalReviews} отзывов
            </span>
          </div>

          <div className="flex gap-2" style={{ flexWrap: "wrap" }}>
            {video.videoType?.title && <span className="tag">{video.videoType.title}</span>}
            <span className="tag tag-muted">
              {video.startYear}
              {video.endYear ? ` – ${video.endYear}` : ""}
            </span>
            {video.genres.map((g) => (
              <span key={g.id} className="tag tag-muted">
                {g.title}
              </span>
            ))}
          </div>

          {video.description && (
            <p style={{ color: "var(--text)", marginTop: 8 }}>{video.description}</p>
          )}

          <div className="flex gap-2 mt-3" style={{ flexWrap: "wrap" }}>
            {video.videoUrl && (
              <a href={video.videoUrl} target="_blank" rel="noreferrer" className="btn btn-primary">
                Смотреть в VK Видео ↗
              </a>
            )}
            {isAuthenticated && (
              <button
                className="btn"
                onClick={() => addFavorite.mutate()}
                disabled={addFavorite.isPending}
              >
                {addFavorite.isSuccess ? "✓ В избранном" : "+ В избранное"}
              </button>
            )}
          </div>
          {addFavorite.isError && addFavorite.error instanceof HttpError && (
            <div className="muted" style={{ fontSize: 13 }}>
              {addFavorite.error.payload?.message ?? "Не удалось добавить"}
            </div>
          )}
        </div>
      </div>

      <div className="divider" />

      <section>
        <div style={{ display: "flex", alignItems: "baseline", justifyContent: "space-between" }}>
          <h2>Отзывы</h2>
          {reviewsQuery.data && (
            <span className="muted" style={{ fontSize: 14 }}>
              Всего: {reviewsQuery.data.totalCount}
            </span>
          )}
        </div>

        {isAuthenticated ? (
          <ReviewForm
            videoId={id}
            existing={myReviewInList ? { rate: myReviewInList.rate, text: myReviewInList.text } : null}
          />
        ) : (
          <div className="alert alert-info mt-4">
            <Link to="/login">Войдите</Link>, чтобы оставить отзыв.
          </div>
        )}

        <div className="flex flex-col gap-3 mt-6">
          {reviewsQuery.isLoading && <Loader />}
          {reviewsQuery.data?.items.length === 0 && (
            <div className="empty-state">Пока нет отзывов. Будьте первым!</div>
          )}
          {reviewsQuery.data?.items.map((review) => (
            <ReviewItem
              key={review.reviewId}
              review={review}
              actions={
                review.user.userId === user?.userId ? (
                  <DeleteMyReviewButton videoId={id} />
                ) : null
              }
            />
          ))}
        </div>

        {reviewsQuery.data && (
          <Pagination
            page={reviewsQuery.data.pageNumber}
            totalPages={reviewsQuery.data.totalPages}
            onChange={setReviewsPage}
          />
        )}
      </section>

      <style>{`
        @media (max-width: 860px) {
          .video-hero {
            grid-template-columns: 1fr !important;
          }
        }
      `}</style>
    </div>
  );
}

interface ReviewFormProps {
  videoId: string;
  existing: { rate: number; text: string } | null;
}

function ReviewForm({ videoId, existing }: ReviewFormProps) {
  const queryClient = useQueryClient();
  const [rate, setRate] = useState(existing?.rate ?? 0);
  const [text, setText] = useState(existing?.text ?? "");
  const [error, setError] = useState<string | null>(null);

  const mutation = useMutation({
    mutationFn: () =>
      existing
        ? reviewsApi.update(videoId, { rate, text })
        : reviewsApi.create(videoId, { rate, text }),
    onSuccess: () => {
      setError(null);
      queryClient.invalidateQueries({ queryKey: ["reviews", videoId] });
      queryClient.invalidateQueries({ queryKey: ["video", videoId] });
    },
    onError: (err) => {
      if (err instanceof HttpError) {
        setError(err.payload?.message ?? "Не удалось отправить отзыв");
      } else {
        setError("Не удалось отправить отзыв");
      }
    },
  });

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (rate < 1) {
      setError("Поставьте оценку");
      return;
    }
    if (!text.trim()) {
      setError("Напишите текст отзыва");
      return;
    }
    mutation.mutate();
  };

  return (
    <form className="card mt-4" onSubmit={onSubmit}>
      <div className="field">
        <label className="field-label">Ваша оценка</label>
        <StarRating value={rate} onChange={setRate} size={28} />
      </div>
      <div className="field">
        <label className="field-label">Текст отзыва</label>
        <textarea
          className="textarea"
          value={text}
          onChange={(e) => setText(e.target.value)}
          placeholder="Поделитесь впечатлениями..."
          rows={4}
        />
      </div>
      {error && <div className="alert alert-error mt-2">{error}</div>}
      <button className="btn btn-primary mt-3" type="submit" disabled={mutation.isPending}>
        {existing ? "Обновить отзыв" : "Опубликовать"}
      </button>
    </form>
  );
}

function DeleteMyReviewButton({ videoId }: { videoId: string }) {
  const queryClient = useQueryClient();
  const mutation = useMutation({
    mutationFn: () => reviewsApi.remove(videoId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["reviews", videoId] });
      queryClient.invalidateQueries({ queryKey: ["video", videoId] });
    },
  });
  return (
    <button
      className="btn btn-sm btn-danger"
      onClick={() => mutation.mutate()}
      disabled={mutation.isPending}
    >
      Удалить
    </button>
  );
}
