
# РЖД Соседи

**Автор:** Шарифьянова Дилара Вагизовна

**Группа:** ТРИС-3-23

**Тема проекта:** Разработка программного обеспечения для организации социального взаимодействия попутчиков в поездах дальнего следования.

## Используемые технологии

- Backend: ASP.NET Core (.NET 8)
- Frontend: Vue 3, Vite
- База данных: PostgreSQL
- Документация API: Swagger

## Запуск backend

Необходимы .NET 8 SDK и PostgreSQL с настроенной строкой подключения `ConnectionStrings:Default`.

В терминале из корневой папки проекта выполнить:

```bash
cd backend
dotnet restore
dotnet run
```

Адреса:

- Backend: http://localhost:5000
- Swagger: http://localhost:5000/swagger/index.html
- Health: http://localhost:5000/health
- Info: http://localhost:5000/info
- API поездок: http://localhost:5000/api/trips
- API вагонов: http://localhost:5000/api/carriages
- API пользователей: http://localhost:5000/api/users

## Запуск frontend

Необходимы Node.js и npm.

В файле `frontend/.env` указать:

```env
VITE_API_BASE_URL=http://localhost:5000
VITE_USER_ID=1
```

В отдельном терминале из корневой папки проекта выполнить:

```bash
cd frontend
npm install
npm run dev
```

Адрес приложения: http://localhost:5173

Для работы приложения необходимо одновременно запустить backend и frontend.

## Передача идентификатора пользователя

В учебном режиме при создании поездки через
POST /api/trips используется обязательный
HTTP-заголовок `X-User-Id`.

Пример:

```http
X-User-Id: 1
```

Идентификатор должен быть положительным целым числом и соответствовать существующему пользователю в базе данных.

Frontend получает идентификатор из переменной
`VITE_USER_ID` и передаёт его в заголовке запроса.

При отсутствии или некорректном значении заголовка, а также при отсутствии пользователя в базе данных сервер возвращает 400 Bad Request.

Передача идентификатора через заголовок является учебным механизмом и не заменяет полноценную авторизацию.

## Бизнес-правила

- При создании поездки устанавливается статус New.
- Ответственным может быть только проводник.
- Переход в статус InProgress без назначенного проводника запрещён.
- Допустимые переходы: New → InProgress → Closed, New → Cancelled.
- Проверка бизнес-правил выполняется в сервисном слое TripService.
- При нарушении правил перехода между статусами API возвращает 409 Conflict.

## Сборка проекта

Backend:

```bash
cd backend
dotnet build
```

Frontend:

```bash
cd frontend
npm run build
```
