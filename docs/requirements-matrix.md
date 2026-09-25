# docs/requirements-matrix.md

# Матрица требований

| Требование ЛР1 | Поле или сущность | Запрос | Критерий |
|---|---|---|---|
| Создать операцию | FinancialOperation, title, budgetItemId, description, amount | POST /api/operations | 1 |
| Назначить исполнителя | assigneeUserId | PATCH …/assignee | 2 |
| Перевести в работу | status | PATCH …/status | 3 |
