import { Link } from "react-router-dom";

export function NotFoundPage() {
  return (
    <div className="container main center" style={{ flexDirection: "column", gap: 16, minHeight: 360 }}>
      <h1 style={{ fontSize: 96, lineHeight: 1 }}>404</h1>
      <p>Страница не найдена</p>
      <Link to="/" className="btn btn-primary">
        На главную
      </Link>
    </div>
  );
}
