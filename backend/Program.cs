
using backend.Data;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключение PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Default")));

// Регистрация сервиса поездок
builder.Services.AddScoped<ITripService, TripService>();

// Настройка CORS для Vue
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("Frontend");

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/info", (IHostEnvironment env) => Results.Ok(new
{
    application = "РЖД Соседи",
    environment = env.EnvironmentName,
    lab = "ЛР10"
}));

// Авторизация реализуется отдельно
// app.UseAuthorization();

app.MapControllers();

app.Run();
