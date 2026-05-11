import { useState } from "react";
import { Link } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useAuth } from "@/auth/AuthContext";
import { reviewsApi } from "@/api/reviews";
import { favoritesApi } from "@/api/favorites";
import { VideoCard } from "@/components/VideoCard";
import { MyReviewCard } from "@/components/MyReviewCard";
import { Loader } from "@/components/Loader";
import { Pagination } from "@/components/Pagination";

const PAGE_SIZE = 6;
type Tab = "reviews" | "favorites";

export function ProfilePage() {
  const { user } = useAuth();
  const [tab, setTab] = useState<Tab>("reviews");
  const [reviewsPage, setReviewsPage] = useState(1);
  const [favsPage, setFavsPage] = useState(1);

  const queryClient = useQueryClient();

  const reviewsQuery = useQuery({
    queryKey: ["my-reviews", reviewsPage],
    queryFn: () => reviewsApi.myReviews({ pageNumber: reviewsPage, pageSize: PAGE_SIZE }),
  });

  const favoritesQuery = useQuery({
    queryKey: ["favorites", favsPage],
    queryFn: () => favoritesApi.list({ pageNumber: favsPage, pageSize: PAGE_SIZE }),
  });

  const removeFavorite = useMutation({
    mutationFn: (videoId: string) => favoritesApi.remove(videoId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["favorites"] }),
  });

  const removeReview = useMutation({
    mutationFn: (videoId: string) => reviewsApi.remove(videoId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["my-reviews"] });
      queryClient.invalidateQueries({ queryKey: ["reviews"] });
    },
  });

  if (!user) return null;

  return (
    <div className="container main">
      <div className="card" style={{ display: "flex", gap: 20, alignItems: "center" }}>
        {user.avatarUrl ? (
          <img
            src={user.avatarUrl}
            alt=""
            style={{ width: 72, height: 72, borderRadius: "50%", objectFit: "cover" }}
          />
        ) : (
          <div
            style={{
              width: 72,
              height: 72,
              borderRadius: "50%",
              background: "linear-gradient(135deg, #4d8bff, #7e5bff)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              fontSize: 28,
              fontWeight: 700,
              color: "#fff",
            }}
          >
            {user.name?.[0] ?? "?"}
          </div>
        )}
        <div>
          <h1>
            {user.name} {user.surname}
          </h1>
          <div className="flex gap-2 mt-2">
            {user.isAdmin && <span className="tag">Администратор</span>}
          </div>
        </div>
      </div>

      <div className="mt-6" style={{ display: "flex", gap: 4, borderBottom: "1px solid var(--border)" }}>
        <TabBtn active={tab === "reviews"} onClick={() => setTab("reviews")}>
          Мои отзывы
        </TabBtn>
        <TabBtn active={tab === "favorites"} onClick={() => setTab("favorites")}>
          Избранное
        </TabBtn>
      </div>

      <div className="mt-6">
        {tab === "reviews" && (
          <>
            {reviewsQuery.isLoading && <Loader />}
            {reviewsQuery.data?.items.length === 0 && (
              <div className="empty-state">
                У вас пока нет отзывов. <Link to="/">Найдите видео</Link> и оставьте первый.
              </div>
            )}
            <div className="flex flex-col gap-3">
              {reviewsQuery.data?.items.map((review) => (
                <MyReviewCard
                  key={review.reviewId}
                  review={review}
                  onDelete={
                    review.video
                      ? () => {
                          if (confirm("Удалить отзыв?")) removeReview.mutate(review.video!.videoId);
                        }
                      : undefined
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
          </>
        )}

        {tab === "favorites" && (
          <>
            {favoritesQuery.isLoading && <Loader />}
            {favoritesQuery.data?.items.length === 0 && (
              <div className="empty-state">Список избранного пуст</div>
            )}
            {favoritesQuery.data && favoritesQuery.data.items.length > 0 && (
              <div className="grid grid-videos">
                {favoritesQuery.data.items.map((f) => (
                  <div key={f.video.videoId} style={{ position: "relative" }}>
                    <VideoCard video={f.video} />
                    <button
                      className="btn btn-sm btn-danger"
                      style={{ position: "absolute", top: 10, left: 10, zIndex: 1 }}
                      onClick={() => removeFavorite.mutate(f.video.videoId)}
                      disabled={removeFavorite.isPending}
                    >
                      Убрать
                    </button>
                  </div>
                ))}
              </div>
            )}
            {favoritesQuery.data && (
              <Pagination
                page={favoritesQuery.data.pageNumber}
                totalPages={favoritesQuery.data.totalPages}
                onChange={setFavsPage}
              />
            )}
          </>
        )}
      </div>
    </div>
  );
}

function TabBtn({
  active,
  children,
  onClick,
}: {
  active: boolean;
  children: React.ReactNode;
  onClick: () => void;
}) {
  return (
    <button
      onClick={onClick}
      style={{
        padding: "10px 16px",
        background: "transparent",
        color: active ? "var(--text)" : "var(--text-muted)",
        border: "none",
        borderBottom: `2px solid ${active ? "var(--accent)" : "transparent"}`,
        marginBottom: -1,
        cursor: "pointer",
        fontWeight: 500,
        fontSize: 14,
      }}
    >
      {children}
    </button>
  );
}
