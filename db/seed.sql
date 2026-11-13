-- Вагоны (справочник)
INSERT INTO carriages (id, name, type) VALUES
  (1, 'Плацкарт', 'economy'),
  (2, 'Купе', 'comfort'),
  (3, 'СВ', 'business'),
  (4, 'Тихий вагон', 'quiet');

-- Пользователи
INSERT INTO users (id, login, full_name, role) VALUES
  (1, 'ivan_p', 'Иванов Иван', 'passenger'),
  (2, 'petrova_a', 'Петрова Анна', 'passenger'),
  (3, 'sidorov_k', 'Сидоров Константин', 'conductor'),
  (4, 'admin', 'Администратор', 'admin');

-- Поездки — все 4 статуса представлены!
INSERT INTO trips (id, number, title, description, carriage_id, status, created_by_user_id, assignee_user_id) VALUES
  (1, 'П-101', 'Москва — Казань',
   'Ночной поезд, 12 часов в пути. Ищу попутчиков для настольных игр.',
   2, 'New',        1, NULL),
  (2, 'П-102', 'Санкт-Петербург — Москва',
   'Дневной экспресс, 4 часа. Тихий вагон, просьба не беспокоить.',
   4, 'InProgress', 2, 3),
  (3, 'П-103', 'Москва — Владивосток',
   'Двухдневный маршрут, хочу найти попутчиков для шахмат.',
   1, 'Closed',     1, 3),
  (4, 'П-104', 'Сочи — Москва',
   'Отменена из-за погодных условий.',
   3, 'Cancelled',  2, NULL);