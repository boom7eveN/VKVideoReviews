import { Link } from "react-router-dom";
import type { Review } from "@/api/types";
import { StarRating } from "./StarRating";
import styles from "./MyReviewCard.module.css";

interface Props {
  review: Review;
  onDelete?: () => void;
}

export function MyReviewCard({ review, onDelete }: Props) {
  const video = review.video;
  const date = new Date(review.createDate).toLocaleDateString("ru-RU", {
    day: "numeric",
    month: "long",
    year: "numeric",
  });

  return (
    <div className={styles.card}>
      <div className={styles.posterWrap}>
        {video?.imageUrl ? (
          <img src={video.imageUrl} alt={video.title} className={styles.poster} />
        ) : (
          <div className={styles.posterFallback}>VK</div>
        )}
      </div>

      <div className={styles.body}>
        <div className={styles.headerRow}>
          <div className={styles.titleBlock}>
            {video ? (
              <Link to={`/videos/${video.videoId}`} className={styles.videoTitle}>
                {video.title}
              </Link>
            ) : (
              <span className={styles.videoTitle}>Видео удалено</span>
            )}
            <div className={styles.meta}>
              <StarRating value={review.rate} size={14} />
              <span>{date}</span>
            </div>
          </div>
          {onDelete && (
            <button className="btn btn-sm btn-danger" onClick={onDelete}>
              Удалить
            </button>
          )}
        </div>

        <p className={styles.text}>{review.text}</p>

        {video && (
          <Link to={`/videos/${video.videoId}`} className={`btn btn-primary ${styles.cta}`}>
            Перейти к фильму
            <svg viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" strokeWidth="2" aria-hidden>
              <path d="M5 12h14M13 5l7 7-7 7" strokeLinecap="round" strokeLinejoin="round" />
            </svg>
          </Link>
        )}
      </div>
    </div>
  );
}
