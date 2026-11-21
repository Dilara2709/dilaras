using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// === 1. Подключение к базе данных PostgreSQL ===
// Строка подключения берётся из user-secrets (ConnectionStrings:Default)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// === 2. Регистрация сервисов и контроллеров ===
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === 3. Настройка middleware ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Отключено, работаем по HTTP на порту 5000
// app.UseAuthorization();    // Включим в ЛР11

// === 4. Служебные маршруты ===
app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/info", (IHostEnvironment env) => Results.Ok(new
{
    application = "РЖД Соседи",
    environment = env.EnvironmentName,
    lab = "ЛР10"
}));

// === 5. Маршруты контроллеров ===
app.MapControllers();

app.Run();