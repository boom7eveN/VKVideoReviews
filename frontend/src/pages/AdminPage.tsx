import { useState } from "react";
import { Link } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { videosApi, type CreateVideoPayload, type UpdateVideoPayload } from "@/api/videos";
import { genresApi } from "@/api/genres";
import { videoTypesApi } from "@/api/videoTypes";
import { Loader } from "@/components/Loader";
import { VideoForm } from "@/components/VideoForm";
import type { Video } from "@/api/types";

type Tab = "videos" | "genres" | "types";

export function AdminPage() {
  const [tab, setTab] = useState<Tab>("videos");
  return (
    <div className="container main">
      <h1>Админ панель</h1>
      <p className="mt-2">Управление контентом сервиса.</p>

      <div className="mt-6" style={{ display: "flex", gap: 4, borderBottom: "1px solid var(--border)" }}>
        <TabBtn active={tab === "videos"} onClick={() => setTab("videos")}>
          Видео
        </TabBtn>
        <TabBtn active={tab === "genres"} onClick={() => setTab("genres")}>
          Жанры
        </TabBtn>
        <TabBtn active={tab === "types"} onClick={() => setTab("types")}>
          Типы видео
        </TabBtn>
      </div>

      <div className="mt-6">
        {tab === "videos" && <VideosAdmin />}
        {tab === "genres" && <GenresAdmin />}
        {tab === "types" && <VideoTypesAdmin />}
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

function VideosAdmin() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [searchInput, setSearchInput] = useState("");
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showCreate, setShowCreate] = useState(false);
  const [lastCreated, setLastCreated] = useState<Video | null>(null);

  const videosQuery = useQuery({
    queryKey: ["videos", { page, search }],
    queryFn: () => videosApi.list({ pageNumber: page, pageSize: 12, titlePart: search || undefined }),
  });
  const genresQuery = useQuery({ queryKey: ["genres"], queryFn: genresApi.list });
  const typesQuery = useQuery({ queryKey: ["video-types"], queryFn: videoTypesApi.list });

  const remove = useMutation({
    mutationFn: (id: string) => videosApi.remove(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["videos"] }),
  });

  const create = useMutation({
    mutationFn: (payload: CreateVideoPayload) => videosApi.create(payload),
    onSuccess: (video) => {
      setLastCreated(video);
      setShowCreate(false);
      queryClient.invalidateQueries({ queryKey: ["videos"] });
    },
  });

  const onSearch = (e: React.FormEvent) => {
    e.preventDefault();
    setSearch(searchInput.trim());
    setPage(1);
  };

  return (
    <div className="flex flex-col gap-4">
      <div className="flex gap-3" style={{ alignItems: "center" }}>
        <form onSubmit={onSearch} style={{ flex: 1, display: "flex", gap: 8 }}>
          <input
            className="input"
            placeholder="Поиск по названию..."
            value={searchInput}
            onChange={(e) => setSearchInput(e.target.value)}
          />
          <button type="submit" className="btn">
            Найти
          </button>
          {search && (
            <button
              type="button"
              className="btn btn-ghost"
              onClick={() => {
                setSearch("");
                setSearchInput("");
                setPage(1);
              }}
            >
              Сброс
            </button>
          )}
        </form>
        <button
          className="btn btn-primary"
          onClick={() => {
            setShowCreate((v) => !v);
            setLastCreated(null);
          }}
        >
          {showCreate ? "Закрыть" : "+ Добавить видео"}
        </button>
      </div>

      {lastCreated && (
        <div className="alert alert-info">
          Видео <strong>«{lastCreated.title}»</strong> создано.{" "}
          <Link to={`/videos/${lastCreated.videoId}`}>Открыть карточку →</Link>
        </div>
      )}

      {showCreate && (
        <div className="card">
          <h3 style={{ marginBottom: 12 }}>Новое видео</h3>
          <VideoForm
            genres={genresQuery.data ?? []}
            types={typesQuery.data ?? []}
            submitLabel="Создать"
            onSubmit={(payload) => create.mutateAsync(payload)}
            isSubmitting={create.isPending}
            error={create.error}
            onCancel={() => setShowCreate(false)}
            resetOnSuccess
          />
        </div>
      )}

      {videosQuery.isLoading && <Loader />}

      {videosQuery.data && videosQuery.data.items.length === 0 && (
        <div className="empty-state">Ничего не найдено</div>
      )}

      <div className="flex flex-col gap-2">
        {videosQuery.data?.items.map((v) => (
          <div key={v.videoId} className="card" style={{ padding: 12 }}>
            <div style={{ display: "flex", alignItems: "center", gap: 12 }}>
              <div
                style={{
                  width: 80,
                  height: 50,
                  background: v.imageUrl ? `url(${v.imageUrl}) center/cover` : "linear-gradient(135deg,#4d8bff,#7e5bff)",
                  borderRadius: 6,
                  flexShrink: 0,
                }}
              />
              <div style={{ flex: 1, minWidth: 0 }}>
                <div style={{ fontWeight: 600, overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
                  {v.title}
                </div>
                <div className="muted" style={{ fontSize: 12 }}>
                  {v.startYear}
                  {v.endYear ? ` – ${v.endYear}` : ""} · {v.videoType?.title ?? "—"} ·{" "}
                  ★ {v.averageRate.toFixed(1)} ({v.totalReviews})
                </div>
              </div>
              <Link to={`/videos/${v.videoId}`} className="btn btn-sm btn-ghost">
                Открыть
              </Link>
              <button
                className="btn btn-sm"
                onClick={() => setEditingId(editingId === v.videoId ? null : v.videoId)}
              >
                {editingId === v.videoId ? "Скрыть" : "Редактировать"}
              </button>
              <button
                className="btn btn-sm btn-danger"
                onClick={() => {
                  if (confirm(`Удалить "${v.title}"?`)) remove.mutate(v.videoId);
                }}
              >
                Удалить
              </button>
            </div>

            {editingId === v.videoId && (
              <div style={{ marginTop: 16, paddingTop: 16, borderTop: "1px solid var(--border)" }}>
                <EditVideoBlock
                  video={v}
                  genres={genresQuery.data ?? []}
                  types={typesQuery.data ?? []}
                  onDone={() => setEditingId(null)}
                />
              </div>
            )}
          </div>
        ))}
      </div>

      {videosQuery.data && videosQuery.data.totalPages > 1 && (
        <div className="flex gap-2 center mt-3">
          <button className="btn btn-sm" disabled={page <= 1} onClick={() => setPage((p) => p - 1)}>
            ←
          </button>
          <span className="muted">
            {page} / {videosQuery.data.totalPages}
          </span>
          <button
            className="btn btn-sm"
            disabled={page >= videosQuery.data.totalPages}
            onClick={() => setPage((p) => p + 1)}
          >
            →
          </button>
        </div>
      )}
    </div>
  );
}

interface EditBlockProps {
  video: Video;
  genres: { id: string; title: string }[];
  types: { id: string; title: string }[];
  onDone: () => void;
}

function EditVideoBlock({ video, genres, types, onDone }: EditBlockProps) {
  const queryClient = useQueryClient();
  const update = useMutation({
    mutationFn: (payload: UpdateVideoPayload) => videosApi.update(video.videoId, payload),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["videos"] });
      queryClient.invalidateQueries({ queryKey: ["video", video.videoId] });
      onDone();
    },
  });

  return (
    <VideoForm
      genres={genres}
      types={types}
      initial={{
        videoUrl: video.videoUrl,
        title: video.title,
        imageUrl: video.imageUrl,
        description: video.description,
        startYear: video.startYear,
        endYear: video.endYear ?? null,
        videoTypeId: video.videoType?.id ?? "",
        genreIds: video.genres.map((g) => g.id),
      }}
      submitLabel="Сохранить"
      onSubmit={(payload) => update.mutateAsync(payload)}
      isSubmitting={update.isPending}
      error={update.error}
      onCancel={onDone}
    />
  );
}

function GenresAdmin() {
  const queryClient = useQueryClient();
  const [title, setTitle] = useState("");
  const list = useQuery({ queryKey: ["genres"], queryFn: genresApi.list });
  const create = useMutation({
    mutationFn: (t: string) => genresApi.create(t),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["genres"] });
      setTitle("");
    },
  });
  const remove = useMutation({
    mutationFn: (id: string) => genresApi.remove(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["genres"] }),
  });
  const update = useMutation({
    mutationFn: ({ id, title }: { id: string; title: string }) => genresApi.update(id, title),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["genres"] }),
  });
  return (
    <SimpleAdminList
      items={list.data}
      isLoading={list.isLoading}
      newPlaceholder="Новый жанр..."
      newTitle={title}
      setNewTitle={setTitle}
      onCreate={() => create.mutate(title)}
      onRemove={(id) => remove.mutate(id)}
      onUpdate={(id, title) => update.mutateAsync({ id, title })}
      isCreating={create.isPending}
    />
  );
}

function VideoTypesAdmin() {
  const queryClient = useQueryClient();
  const [title, setTitle] = useState("");
  const list = useQuery({ queryKey: ["video-types"], queryFn: videoTypesApi.list });
  const create = useMutation({
    mutationFn: (t: string) => videoTypesApi.create(t),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["video-types"] });
      setTitle("");
    },
  });
  const remove = useMutation({
    mutationFn: (id: string) => videoTypesApi.remove(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["video-types"] }),
  });
  const update = useMutation({
    mutationFn: ({ id, title }: { id: string; title: string }) => videoTypesApi.update(id, title),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["video-types"] }),
  });
  return (
    <SimpleAdminList
      items={list.data}
      isLoading={list.isLoading}
      newPlaceholder="Новый тип видео..."
      newTitle={title}
      setNewTitle={setTitle}
      onCreate={() => create.mutate(title)}
      onRemove={(id) => remove.mutate(id)}
      onUpdate={(id, title) => update.mutateAsync({ id, title })}
      isCreating={create.isPending}
    />
  );
}

interface SimpleAdminListProps {
  items: { id: string; title: string }[] | undefined;
  isLoading: boolean;
  newPlaceholder: string;
  newTitle: string;
  setNewTitle: (v: string) => void;
  onCreate: () => void;
  onRemove: (id: string) => void;
  onUpdate: (id: string, title: string) => Promise<unknown>;
  isCreating: boolean;
}

function SimpleAdminList(props: SimpleAdminListProps) {
  return (
    <div className="flex flex-col gap-4">
      <form
        className="card"
        onSubmit={(e) => {
          e.preventDefault();
          if (props.newTitle.trim()) props.onCreate();
        }}
        style={{ display: "flex", gap: 12 }}
      >
        <input
          className="input"
          placeholder={props.newPlaceholder}
          value={props.newTitle}
          onChange={(e) => props.setNewTitle(e.target.value)}
        />
        <button className="btn btn-primary" disabled={props.isCreating || !props.newTitle.trim()}>
          Добавить
        </button>
      </form>

      {props.isLoading && <Loader />}
      <div className="flex flex-col gap-2">
        {props.items?.map((item) => (
          <EditableRow
            key={item.id}
            id={item.id}
            title={item.title}
            onSave={(newTitle) => props.onUpdate(item.id, newTitle)}
            onRemove={() => props.onRemove(item.id)}
          />
        ))}
      </div>
    </div>
  );
}

interface EditableRowProps {
  id: string;
  title: string;
  onSave: (newTitle: string) => Promise<unknown>;
  onRemove: () => void;
}

function EditableRow({ title, onSave, onRemove }: EditableRowProps) {
  const [isEditing, setIsEditing] = useState(false);
  const [value, setValue] = useState(title);
  const [saving, setSaving] = useState(false);

  const save = async () => {
    const trimmed = value.trim();
    if (!trimmed || trimmed === title) {
      setIsEditing(false);
      setValue(title);
      return;
    }
    setSaving(true);
    try {
      await onSave(trimmed);
      setIsEditing(false);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="card" style={{ display: "flex", alignItems: "center", gap: 8, padding: 12 }}>
      {isEditing ? (
        <>
          <input
            className="input"
            value={value}
            onChange={(e) => setValue(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter") void save();
              if (e.key === "Escape") {
                setIsEditing(false);
                setValue(title);
              }
            }}
            autoFocus
            style={{ flex: 1 }}
          />
          <button className="btn btn-sm btn-primary" onClick={save} disabled={saving}>
            {saving ? "..." : "Сохранить"}
          </button>
          <button
            className="btn btn-sm btn-ghost"
            onClick={() => {
              setIsEditing(false);
              setValue(title);
            }}
            disabled={saving}
          >
            Отмена
          </button>
        </>
      ) : (
        <>
          <span style={{ flex: 1 }}>{title}</span>
          <button className="btn btn-sm" onClick={() => setIsEditing(true)}>
            Редактировать
          </button>
          <button
            className="btn btn-sm btn-danger"
            onClick={() => {
              if (confirm(`Удалить "${title}"?`)) onRemove();
            }}
          >
            Удалить
          </button>
        </>
      )}
    </div>
  );
}
