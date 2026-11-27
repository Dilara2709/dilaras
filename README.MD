Шарифьянова Дилара Вагизовна
ТРИС-3-23
Разработка программного обеспечения для организации социального взаимодействия попутчиков в поездах дальнего следования


## Запуск backend

Необходимы .NET 8 SDK и PostgreSQL с настроенной строкой подключения `ConnectionStrings:Default`.

В терминале из корневой папки проекта:

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

## Запуск frontend

В отдельном терминале из корневой папки проекта:

```bash
cd frontend
npm install
npm run dev
```

Адрес приложения: http://localhost:5173

В файле `.env` папки `frontend` указать:

```env
VITE_API_BASE_URL=http://localhost:5000
```

Для работы приложения необходимо одновременно запустить backend и frontend.