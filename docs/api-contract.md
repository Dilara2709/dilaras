# API-контракт

## Таблица запросов

| Метод и путь | Тело | Успех | Возможная ошибка |
|---|---|---|---|
| GET /api/operations | — | 200, массив или [] | - |
| GET /api/operations/{id} | — | 200, объект | 404 |
| POST /api/operations | title, budgetItemId, description, amount | 201, id, number, status: New | 400 |
| PATCH /api/operations/{id}/assignee | assigneeUserId | 200 | 404 |
| PATCH /api/operations/{id}/status | status | 200 | 404, 409 |

Статусы только: `New`, `InProgress`, `Closed`, `Cancelled`. Отмена - `Cancelled`, а не удаление строки.

Клиент не присылает при создании `id`, `number`, `status` и исполнителя. Их определяет сервер.

## Пример JSON — POST /api/operations

```json
{
  "title": "Закупка канцтоваров",
  "budgetItemId": 3,
  "description": "Бумага, ручки для бухгалтерии",
  "amount": 15000.00
}
```

## Пример ответа — 201

```json
{
  "id": 42,
  "number": "FIN-2026-0042",
  "status": "New"
}
```

## Пример — PATCH /api/operations/{id}/assignee

```json
{
  "assigneeUserId": 7
}
```

## Пример — PATCH /api/operations/{id}/status

```json
{
  "status": "InProgress"
}
```
