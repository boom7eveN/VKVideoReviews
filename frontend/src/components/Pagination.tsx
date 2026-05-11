interface Props {
  page: number;
  totalPages: number;
  onChange: (page: number) => void;
}

export function Pagination({ page, totalPages, onChange }: Props) {
  if (totalPages <= 1) return null;
  const canPrev = page > 1;
  const canNext = page < totalPages;
  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        gap: 12,
        marginTop: 32,
      }}
    >
      <button
        className="btn btn-sm"
        onClick={() => canPrev && onChange(page - 1)}
        disabled={!canPrev}
      >
        ← Назад
      </button>
      <span className="muted" style={{ fontSize: 14 }}>
        Страница <strong style={{ color: "var(--text)" }}>{page}</strong> из {totalPages}
      </span>
      <button
        className="btn btn-sm"
        onClick={() => canNext && onChange(page + 1)}
        disabled={!canNext}
      >
        Вперёд →
      </button>
    </div>
  );
}
