# VKVideoReviews

Сервис для отзывов на видео из ВК с авторизацией через VK ID.

## Стек

- .NET 10
- EF Core
- PostgreSQL
- Redis
- Docker + Docker Compose
- AutoMapper
- FluentValidation
- Serilog
- Swagger
- JWT

## Быстрый старт (Docker)

### 1) Подготовка

1. Клонируйте репозиторий.

### 2) Настройка VK ID

1. Создайте приложение в VK ID.
2. В allowed redirect URI укажите:
   - `http://localhost:80/api/auth/vk/callback`
3. Сохраните выданные ключи (`ClientId`, `ProtectedKey`, `ServiceKey`) — они понадобятся на следующем шаге.

### 3) В `backend/` создайте `.env` на основе примера:
```bash
cp .env.example .env
```
Заполните значения в `.env` согласно комментариям из `.env.example`, включая ключи VK ID из шага 2.

### 4) Запуск

Из корня репозитория:

```bash
docker compose up -d --build
```

После запуска:

- API: `http://localhost:80`
- Swagger: `http://localhost:80/swagger`
- PostgreSQL: `localhost:5432`
- Redis: `localhost:6379`

Приложение автоматически применяет миграции БД при старте.

## Авторизация и роли

- Вход начинается с `GET /api/auth/vk/login` (редирект на VK).
- VK возвращает пользователя на `GET /api/auth/vk/callback`.
- Backend выдает `accessToken` и `refreshToken`.
- Обновление токенов: `POST /api/auth/tokens/refresh`.
- Роль `Admin` назначается, если VK user id есть в `AdminVkUserIds`.

## Особенности проекта

- В проекте используется `Unit of Work` паттерн:
  - `IUnitOfWork` для доменных операций (видео, жанры, отзывы, избранное).
  - `IAuthUnitOfWork` для auth (пользователь, токены, сессии).
- Основные write операции обернуты в транзакции (`BeginTransactionAsync` / `CommitAsync` / `RollbackAsync`) для консистентности данных.
- Для конкурентных изменений отзывов и пересчета рейтинга видео используется пессимистичная блокировка.
- После изменений отзывов инвалидируется кэш карточки видео в Redis, чтобы клиент всегда получал актуальный `AverageRate` и `TotalReviews`.

## Схема базы данных

- [ERD.png](backend/ERD.png)

