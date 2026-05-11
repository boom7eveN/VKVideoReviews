import { NavLink, Link } from "react-router-dom";
import { useAuth } from "@/auth/AuthContext";
import styles from "./Header.module.css";

export function Header() {
  const { user, isAuthenticated, isLoading, login, logout } = useAuth();

  return (
    <header className={styles.header}>
      <div className="container">
        <div className={styles.row}>
          <Link to="/" className={styles.brand}>
            <span className={styles.logo}>
              <svg viewBox="0 0 24 24" width="24" height="24" aria-hidden>
                <defs>
                  <linearGradient id="hgr" x1="0" x2="1" y1="0" y2="1">
                    <stop offset="0%" stopColor="#4d8bff" />
                    <stop offset="100%" stopColor="#7e5bff" />
                  </linearGradient>
                </defs>
                <rect width="24" height="24" rx="6" fill="url(#hgr)" />
                <path d="M8 8 L16 12 L8 16 Z" fill="#fff" />
              </svg>
            </span>
            <span>
              VK Video<span className={styles.brandAccent}>Reviews</span>
            </span>
          </Link>

          <nav className={styles.nav}>
            <NavLink
              to="/"
              className={({ isActive }) =>
                `${styles.navLink} ${isActive ? styles.navLinkActive : ""}`
              }
              end
            >
              Каталог
            </NavLink>
            {isAuthenticated && (
              <NavLink
                to="/profile"
                className={({ isActive }) =>
                  `${styles.navLink} ${isActive ? styles.navLinkActive : ""}`
                }
              >
                Профиль
              </NavLink>
            )}
            {user?.isAdmin && (
              <NavLink
                to="/admin"
                className={({ isActive }) =>
                  `${styles.navLink} ${isActive ? styles.navLinkActive : ""}`
                }
              >
                Админ панель
              </NavLink>
            )}
          </nav>

          <div className={styles.right}>
            {isLoading ? (
              <span className="loader" />
            ) : isAuthenticated && user ? (
              <div className={styles.user}>
                <Link to="/profile" className={styles.userLink}>
                  {user.avatarUrl ? (
                    <img src={user.avatarUrl} alt="" className={styles.avatar} />
                  ) : (
                    <span className={styles.avatarFallback}>
                      {user.name?.[0] ?? "?"}
                    </span>
                  )}
                  <span className={styles.userName}>{user.name}</span>
                </Link>
                <button className="btn btn-ghost btn-sm" onClick={logout}>
                  Выйти
                </button>
              </div>
            ) : (
              <button className="btn btn-primary" onClick={login}>
                <VkIcon />
                Войти через VK
              </button>
            )}
          </div>
        </div>
      </div>
    </header>
  );
}

function VkIcon() {
  return (
    <svg viewBox="0 0 24 24" width="16" height="16" fill="currentColor" aria-hidden>
      <path d="M12.785 16.241s.288-.032.435-.193c.135-.148.13-.425.13-.425s-.018-1.302.572-1.491c.583-.187 1.331 1.246 2.124 1.797.6.418 1.055.327 1.055.327l2.122-.03s1.11-.069.583-.957c-.043-.073-.306-.659-1.578-1.847-1.331-1.242-1.153-1.04.45-3.184.976-1.305 1.367-2.103 1.245-2.444-.116-.325-.83-.239-.83-.239l-2.382.015s-.177-.024-.308.055c-.128.077-.21.258-.21.258s-.378 1.012-.882 1.872c-1.066 1.814-1.491 1.91-1.665 1.795-.405-.265-.304-1.057-.304-1.62 0-1.76.265-2.494-.515-2.685-.259-.063-.45-.105-1.111-.112-.85-.009-1.569.003-1.976.204-.272.133-.481.43-.353.448.158.022.515.099.704.36.245.337.236 1.094.236 1.094s.141 2.085-.328 2.343c-.322.177-.764-.184-1.715-1.83-.487-.844-.855-1.776-.855-1.776s-.071-.176-.197-.27c-.153-.114-.367-.15-.367-.15l-2.265.015s-.34.01-.466.16c-.111.135-.009.413-.009.413s1.774 4.171 3.784 6.272c1.842 1.927 3.935 1.8 3.935 1.8z" />
    </svg>
  );
}
