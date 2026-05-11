import { useEffect, useState } from "react";
import { HttpError } from "@/api/client";
import type { Genre, Video, VideoType } from "@/api/types";
import type { CreateVideoPayload } from "@/api/videos";

export interface VideoFormInitial {
  videoUrl: string;
  title: string;
  imageUrl: string;
  description: string;
  startYear: number;
  endYear: number | null;
  videoTypeId: string;
  genreIds: string[];
}

interface Props {
  genres: Genre[];
  types: VideoType[];
  initial?: Partial<VideoFormInitial>;
  submitLabel: string;
  onSubmit: (payload: CreateVideoPayload) => Promise<Video | unknown>;
  isSubmitting: boolean;
  error: unknown;
  onCancel?: () => void;
  resetOnSuccess?: boolean;
}

const empty: VideoFormInitial = {
  videoUrl: "",
  title: "",
  imageUrl: "",
  description: "",
  startYear: new Date().getFullYear(),
  endYear: null,
  videoTypeId: "",
  genreIds: [],
};

export function VideoForm({
  genres,
  types,
  initial,
  submitLabel,
  onSubmit,
  isSubmitting,
  error,
  onCancel,
  resetOnSuccess,
}: Props) {
  const merged = { ...empty, ...initial };

  const [videoUrl, setVideoUrl] = useState(merged.videoUrl);
  const [title, setTitle] = useState(merged.title);
  const [imageUrl, setImageUrl] = useState(merged.imageUrl);
  const [description, setDescription] = useState(merged.description);
  const [startYear, setStartYear] = useState<number>(merged.startYear);
  const [endYear, setEndYear] = useState<string>(merged.endYear?.toString() ?? "");
  const [videoTypeId, setVideoTypeId] = useState(merged.videoTypeId);
  const [genreIds, setGenreIds] = useState<string[]>(merged.genreIds);

  useEffect(() => {
    if (!initial) return;
    setVideoUrl(initial.videoUrl ?? "");
    setTitle(initial.title ?? "");
    setImageUrl(initial.imageUrl ?? "");
    setDescription(initial.description ?? "");
    setStartYear(initial.startYear ?? new Date().getFullYear());
    setEndYear(initial.endYear?.toString() ?? "");
    setVideoTypeId(initial.videoTypeId ?? "");
    setGenreIds(initial.genreIds ?? []);
  }, [initial]);

  const errMsg = error instanceof HttpError ? error.payload?.message ?? error.message : null;

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit({
      videoUrl,
      title,
      imageUrl,
      description,
      startYear,
      endYear: endYear ? Number(endYear) : null,
      videoTypeId,
      genreIds,
    });
    if (resetOnSuccess) {
      setVideoUrl("");
      setTitle("");
      setImageUrl("");
      setDescription("");
      setEndYear("");
      setGenreIds([]);
    }
  };

  return (
    <form onSubmit={submit}>
      <div className="grid" style={{ gridTemplateColumns: "1fr 1fr" }}>
        <div className="field">
          <label className="field-label">Название</label>
          <input className="input" value={title} onChange={(e) => setTitle(e.target.value)} required />
        </div>
        <div className="field">
          <label className="field-label">URL видео</label>
          <input className="input" value={videoUrl} onChange={(e) => setVideoUrl(e.target.value)} required />
        </div>
        <div className="field">
          <label className="field-label">URL обложки</label>
          <input className="input" value={imageUrl} onChange={(e) => setImageUrl(e.target.value)} required />
        </div>
        <div className="field">
          <label className="field-label">Тип</label>
          <select
            className="select"
            value={videoTypeId}
            onChange={(e) => setVideoTypeId(e.target.value)}
            required
          >
            <option value="">Выберите...</option>
            {types.map((t) => (
              <option key={t.id} value={t.id}>
                {t.title}
              </option>
            ))}
          </select>
        </div>
        <div className="field">
          <label className="field-label">Год начала</label>
          <input
            type="number"
            className="input"
            value={startYear}
            onChange={(e) => setStartYear(Number(e.target.value))}
            required
          />
        </div>
        <div className="field">
          <label className="field-label">Год окончания (опц.)</label>
          <input
            type="number"
            className="input"
            value={endYear}
            onChange={(e) => setEndYear(e.target.value)}
          />
        </div>
      </div>
      <div className="field">
        <label className="field-label">Описание</label>
        <textarea
          className="textarea"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />
      </div>
      <div className="field">
        <label className="field-label">Жанры</label>
        <div className="flex gap-2" style={{ flexWrap: "wrap" }}>
          {genres.map((g) => {
            const active = genreIds.includes(g.id);
            return (
              <button
                type="button"
                key={g.id}
                onClick={() =>
                  setGenreIds((prev) =>
                    active ? prev.filter((id) => id !== g.id) : [...prev, g.id],
                  )
                }
                className={active ? "tag" : "tag tag-muted"}
                style={{
                  cursor: "pointer",
                  border: "1px solid",
                  background: active ? "var(--accent-soft)" : "transparent",
                }}
              >
                {g.title}
              </button>
            );
          })}
        </div>
      </div>
      {errMsg && <div className="alert alert-error mt-2">{errMsg}</div>}
      <div className="flex gap-2 mt-3">
        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
          {isSubmitting ? "Сохранение..." : submitLabel}
        </button>
        {onCancel && (
          <button type="button" className="btn btn-ghost" onClick={onCancel}>
            Отмена
          </button>
        )}
      </div>
    </form>
  );
}
