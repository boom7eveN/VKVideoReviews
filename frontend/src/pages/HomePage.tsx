import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { videosApi } from "@/api/videos";
import { VideoCard } from "@/components/VideoCard";
import { Pagination } from "@/components/Pagination";
import { Loader } from "@/components/Loader";

const PAGE_SIZE = 12;

export function HomePage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [searchInput, setSearchInput] = useState("");

  const query = useQuery({
    queryKey: ["videos", { page, search }],
    queryFn: () =>
      videosApi.list({
        pageNumber: page,
        pageSize: PAGE_SIZE,
        titlePart: search || undefined,
      }),
  });

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSearch(searchInput.trim());
    setPage(1);
  };

  return (
    <div className="container main">
      <section style={{ marginBottom: 28 }}>
        <h1>Каталог видео</h1>
        <p className="mt-2">
          Отзывы и оценки на видео из VK. Найдите что-то интересное!
        </p>
      </section>

      <form
        onSubmit={onSubmit}
        style={{
          display: "flex",
          gap: 12,
          marginBottom: 28,
          background: "var(--bg-card)",
          border: "1px solid var(--border)",
          padding: 8,
          borderRadius: 12,
        }}
      >
        <input
          className="input"
          placeholder="Поиск по названию..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          style={{ border: "none", background: "transparent", flex: 1 }}
        />
        <button type="submit" className="btn btn-primary">
          Найти
        </button>
      </form>

      {query.isLoading && <Loader />}

      {query.isError && (
        <div className="alert alert-error">Не удалось загрузить видео. Попробуйте позже.</div>
      )}

      {query.data && query.data.items.length === 0 && (
        <div className="empty-state">Ничего не найдено</div>
      )}

      {query.data && query.data.items.length > 0 && (
        <>
          <div className="grid grid-videos">
            {query.data.items.map((video) => (
              <VideoCard key={video.videoId} video={video} />
            ))}
          </div>
          <Pagination
            page={query.data.pageNumber}
            totalPages={query.data.totalPages}
            onChange={setPage}
          />
        </>
      )}
    </div>
  );
}
