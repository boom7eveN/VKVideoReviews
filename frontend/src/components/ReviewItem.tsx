import type { Review } from "@/api/types";
import { StarRating } from "./StarRating";

interface Props {
  review: Review;
  actions?: React.ReactNode;
}

export function ReviewItem({ review, actions }: Props) {
  const date = new Date(review.createDate).toLocaleDateString("ru-RU", {
    day: "numeric",
    month: "long",
    year: "numeric",
  });

  return (
    <div className="card">
      <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
        {review.user?.avatarUrl ? (
          <img
            src={review.user.avatarUrl}
            alt=""
            style={{ width: 36, height: 36, borderRadius: "50%", objectFit: "cover" }}
          />
        ) : (
          <div
            style={{
              width: 36,
              height: 36,
              borderRadius: "50%",
              background: "linear-gradient(135deg, #4d8bff, #7e5bff)",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              color: "#fff",
              fontWeight: 600,
            }}
          >
            {review.user?.name?.[0] ?? "?"}
          </div>
        )}
        <div style={{ flex: 1 }}>
          <div style={{ fontWeight: 600 }}>
            {review.user?.name} {review.user?.surname}
          </div>
          <div className="muted" style={{ fontSize: 12 }}>
            {date}
          </div>
        </div>
        <StarRating value={review.rate} />
        {actions}
      </div>
      <p style={{ marginTop: 12, color: "var(--text)", whiteSpace: "pre-wrap" }}>{review.text}</p>
    </div>
  );
}
