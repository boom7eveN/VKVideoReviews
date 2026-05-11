import { Link } from "react-router-dom";
import type { Video } from "@/api/types";
import { StarRating } from "./StarRating";
import styles from "./VideoCard.module.css";

interface Props {
  video: Video;
}

export function VideoCard({ video }: Props) {
  return (
    <Link to={`/videos/${video.videoId}`} className={styles.card}>
      <div className={styles.thumbWrap}>
        {video.imageUrl ? (
          <img src={video.imageUrl} alt={video.title} className={styles.thumb} />
        ) : (
          <div className={styles.thumbFallback}>
            <span>VK</span>
          </div>
        )}
        <div className={styles.thumbOverlay}>
          <span className={styles.year}>
            {video.startYear}
            {video.endYear ? ` – ${video.endYear}` : ""}
          </span>
        </div>
      </div>
      <div className={styles.body}>
        <h3 className={styles.title}>{video.title}</h3>
        <div className={styles.metaRow}>
          <StarRating value={video.averageRate} size={14} />
          <span className={styles.metaText}>
            {video.averageRate.toFixed(1)}
            <span className={styles.metaDim}> · {video.totalReviews} отз.</span>
          </span>
        </div>
        {video.videoType?.title && (
          <span className="tag tag-muted">{video.videoType.title}</span>
        )}
      </div>
    </Link>
  );
}
