-- 1. Фильтр + сортировка: все новые поездки, самые новые сверху
SELECT number, title, status, created_at
FROM trips
WHERE status = 'New'
ORDER BY created_at DESC;

-- 2. JOIN: показать название вагона, а не только его id
SELECT t.number, t.title, c.name AS carriage_name, t.status
FROM trips AS t
JOIN carriages AS c ON c.id = t.carriage_id
ORDER BY t.created_at DESC;

-- 3. GROUP BY: сколько поездок в каждом статусе
SELECT status, COUNT(*) AS count
FROM trips
GROUP BY status
ORDER BY status;

-- 4. JOIN + JOIN: показать, кто создал и кому назначено
SELECT
  t.number,
  t.title,
  c.name  AS carriage_name,
  u1.full_name AS created_by,
  COALESCE(u2.full_name, '—') AS assignee,
  t.status
FROM trips AS t
JOIN carriages AS c ON c.id = t.carriage_id
JOIN users AS u1 ON u1.id = t.created_by_user_id
LEFT JOIN users AS u2 ON u2.id = t.assignee_user_id
ORDER BY t.id;