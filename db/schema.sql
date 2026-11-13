-- Удаляем старые таблицы, если они были (только для учебного пересоздания!)
DROP TABLE IF EXISTS trips CASCADE;
DROP TABLE IF EXISTS carriages CASCADE;
DROP TABLE IF EXISTS users CASCADE;

-- Справочник вагонов (аналог sites)
CREATE TABLE carriages (
  id    integer PRIMARY KEY,
  name  text NOT NULL,
  type  text NOT NULL
);

-- Пользователи (пассажиры, проводники, администраторы)
CREATE TABLE users (
  id        integer PRIMARY KEY,
  login     text NOT NULL UNIQUE,
  full_name text NOT NULL,
  role      text NOT NULL CHECK (role IN ('passenger', 'conductor', 'admin'))
);

-- Главный объект — поездки
CREATE TABLE trips (
  id                 integer PRIMARY KEY,
  number             text NOT NULL UNIQUE,
  title              text NOT NULL,
  description        text NOT NULL,
  carriage_id        integer NOT NULL REFERENCES carriages (id),
  status             text NOT NULL CHECK (status IN ('New', 'InProgress', 'Closed', 'Cancelled')),
  created_at         timestamptz NOT NULL DEFAULT now(),
  created_by_user_id integer NOT NULL REFERENCES users (id),
  assignee_user_id   integer NULL REFERENCES users (id)
);

-- Индексы:
-- зачем: фильтр списка по статусу (например, показать все "New")
CREATE INDEX trips_status_idx ON trips (status);

-- зачем: фильтр по вагону (например, "показать поездки в вагоне № 2")
CREATE INDEX trips_carriage_id_idx ON trips (carriage_id);

-- зачем: сортировка по дате создания (самые новые сверху)
CREATE INDEX trips_created_at_idx ON trips (created_at DESC);