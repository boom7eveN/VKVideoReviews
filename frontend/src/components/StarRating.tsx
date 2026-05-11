import { useState } from "react";

interface ReadonlyProps {
  value: number;
  size?: number;
  max?: number;
  onChange?: never;
}

interface InteractiveProps {
  value: number;
  size?: number;
  max?: number;
  onChange: (value: number) => void;
}

type Props = ReadonlyProps | InteractiveProps;

export function StarRating({ value, size = 16, max = 5, onChange }: Props) {
  const interactive = !!onChange;
  const [hover, setHover] = useState<number | null>(null);
  const display = hover ?? value;

  return (
    <div
      style={{
        display: "inline-flex",
        gap: 2,
        cursor: interactive ? "pointer" : "default",
      }}
      onMouseLeave={() => setHover(null)}
      aria-label={`Рейтинг ${value.toFixed(1)} из ${max}`}
    >
      {Array.from({ length: max }).map((_, i) => {
        const filled = display >= i + 1;
        const half = !filled && display >= i + 0.5;
        return (
          <button
            key={i}
            type="button"
            disabled={!interactive}
            onMouseEnter={() => interactive && setHover(i + 1)}
            onClick={() => interactive && onChange?.(i + 1)}
            style={{
              padding: 0,
              border: 0,
              background: "transparent",
              cursor: interactive ? "pointer" : "default",
              lineHeight: 0,
            }}
            aria-label={`${i + 1} ${i === 0 ? "звезда" : "звёзд"}`}
          >
            <Star size={size} filled={filled} half={half} />
          </button>
        );
      })}
    </div>
  );
}

function Star({ size, filled, half }: { size: number; filled: boolean; half: boolean }) {
  const fill = filled ? "#ffb547" : half ? "url(#half)" : "rgba(255,255,255,0.18)";
  return (
    <svg viewBox="0 0 24 24" width={size} height={size}>
      <defs>
        <linearGradient id="half">
          <stop offset="50%" stopColor="#ffb547" />
          <stop offset="50%" stopColor="rgba(255,255,255,0.18)" />
        </linearGradient>
      </defs>
      <path
        d="M12 2l3.09 6.26L22 9.27l-5 4.87L18.18 22 12 18.56 5.82 22 7 14.14 2 9.27l6.91-1.01L12 2z"
        fill={fill}
      />
    </svg>
  );
}
