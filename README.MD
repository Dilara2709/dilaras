
# РЖД Соседи

**Автор:** Шарифьянова Дилара Вагизовна

**Группа:** ТРИС-3-23

**Тема проекта:** Разработка программного обеспечения для организации социального взаимодействия попутчиков в поездах дальнего следования.

## Используемые технологии

- Backend: ASP.NET Core (.NET 8)
- Frontend: Vue 3, Vite
- База данных: PostgreSQL
- Документация API: Swagger
- ORM: Entity Framework Core

## Настройка базы данных

Необходимы .NET 8 SDK и PostgreSQL.

Создайте базу данных `course_trips` в PostgreSQL.

Пример строки подключения находится в файле
`backend/appsettings.Development.json.example`.

Для безопасного хранения строки подключения используйте User Secrets.

В терминале из корневой папки проекта выполнить:

```bash
cd backend
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=course_trips;Username=postgres;Password=YOUR_PASSWORD"
```

Вместо YOUR_PASSWORD необходимо указать свой пароль PostgreSQL.

Настоящий пароль не должен храниться в GitHub.

## Применение миграций

При необходимости установите инструмент Entity Framework Core:

```bash
dotnet tool install --global dotnet-ef --version 8.0.11
```

В папке backend выполните:

```bash
dotnet restore
dotnet ef database update
```

Миграции создают и обновляют таблицы trips, carriages и users, внешние ключи, ограничения CHECK, уникальные индексы и индекс по статусу поездки.

Для заполнения новой базы учебными данными используйте файл `db/seed.sql`.

Файл `db/schema.sql` является справочным описанием структуры базы данных. При использовании миграций запускать его не требуется.

## Запуск backend

В терминале из корневой папки проекта выполнить:

```bash
cd backend
dotnet restore
dotnet ef database update
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

## Проверка сохранения данных после перезапуска

1. Запустить backend командой `dotnet run`.
2. Открыть Swagger по адресу http://localhost:5000/swagger/index.html.
3. Выполнить POST /api/trips с заголовком X-User-Id и корректными данными поездки.
4. Убедиться, что сервер вернул 201 Created.
5. Запомнить идентификатор созданной поездки.
6. Остановить backend сочетанием клавиш Ctrl+C.
7. Повторно запустить backend командой `dotnet run`.
8. Выполнить GET /api/trips/{id}, указав сохранённый идентификатор.
9. Убедиться, что сервер возвращает 200 OK и данные ранее созданной поездки.

Ожидаемый результат: данные сохраняются после перезапуска приложения, поскольку хранятся в PostgreSQL, а не в оперативной памяти.

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
