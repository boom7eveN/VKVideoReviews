import { useEffect, useRef } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "@/auth/AuthContext";
import { Loader } from "@/components/Loader";

export function AuthCallbackPage() {
  const [params] = useSearchParams();
  const navigate = useNavigate();
  const { signIn } = useAuth();
  const done = useRef(false);

  useEffect(() => {
    if (done.current) return;
    done.current = true;

    const status = params.get("status");
    const accessToken = params.get("accessToken");
    const refreshToken = params.get("refreshToken");
    const expiresIn = Number(params.get("expiresIn") ?? 0);

    (async () => {
      if (status !== "ok" || !accessToken || !refreshToken) {
        navigate("/login?error=1", { replace: true });
        return;
      }

      const me = await signIn({
        accessToken,
        refreshToken,
        expiresInSeconds: expiresIn,
        tokenType: "Bearer",
      });

      if (me) {
        navigate("/", { replace: true });
      } else {
        navigate("/login?error=1", { replace: true });
      }
    })();
  }, [params, navigate, signIn]);

  return (
    <div className="container main">
      <Loader label="Завершаем вход..." />
    </div>
  );
}
