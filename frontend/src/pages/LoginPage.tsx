import { useAuth } from "@/auth/AuthContext";
import { Navigate } from "react-router-dom";

export function LoginPage() {
  const { login, isAuthenticated, isLoading } = useAuth();

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="container main" style={{ display: "flex", justifyContent: "center" }}>
      <div
        className="card"
        style={{
          maxWidth: 460,
          width: "100%",
          padding: 32,
          textAlign: "center",
        }}
      >
        <div
          style={{
            width: 64,
            height: 64,
            borderRadius: 16,
            margin: "0 auto 16px",
            background: "linear-gradient(135deg, #4d8bff, #7e5bff)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            boxShadow: "0 10px 30px rgba(77, 139, 255, 0.4)",
          }}
        >
          <svg viewBox="0 0 24 24" width="32" height="32" fill="#fff">
            <path d="M12.785 16.241s.288-.032.435-.193c.135-.148.13-.425.13-.425s-.018-1.302.572-1.491c.583-.187 1.331 1.246 2.124 1.797.6.418 1.055.327 1.055.327l2.122-.03s1.11-.069.583-.957c-.043-.073-.306-.659-1.578-1.847-1.331-1.242-1.153-1.04.45-3.184.976-1.305 1.367-2.103 1.245-2.444-.116-.325-.83-.239-.83-.239l-2.382.015s-.177-.024-.308.055c-.128.077-.21.258-.21.258s-.378 1.012-.882 1.872c-1.066 1.814-1.491 1.91-1.665 1.795-.405-.265-.304-1.057-.304-1.62 0-1.76.265-2.494-.515-2.685-.259-.063-.45-.105-1.111-.112-.85-.009-1.569.003-1.976.204-.272.133-.481.43-.353.448.158.022.515.099.704.36.245.337.236 1.094.236 1.094s.141 2.085-.328 2.343c-.322.177-.764-.184-1.715-1.83-.487-.844-.855-1.776-.855-1.776s-.071-.176-.197-.27c-.153-.114-.367-.15-.367-.15l-2.265.015s-.34.01-.466.16c-.111.135-.009.413-.009.413s1.774 4.171 3.784 6.272c1.842 1.927 3.935 1.8 3.935 1.8z" />
          </svg>
        </div>
        <h1 style={{ fontSize: 24 }}>Войдите в аккаунт</h1>
        <p className="mt-2">Чтобы оставлять отзывы и сохранять видео в избранное, авторизуйтесь через VK ID.</p>
        <button
          className="btn btn-primary mt-4"
          onClick={login}
          disabled={isLoading}
          style={{ width: "100%", padding: "14px 18px", fontSize: 15 }}
        >
          Войти через VK
        </button>
      </div>
    </div>
  );
}
