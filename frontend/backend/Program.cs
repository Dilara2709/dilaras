var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Простая проверка «процесс жив»
app.MapGet("/health", () => Results.Ok("ok"));

// Информация о приложении
app.MapGet("/info", (IHostEnvironment env) => Results.Ok(new
{
    application = "РЖД Соседи",
    environment = env.EnvironmentName,
    lab = "ЛР7",
}));

app.UseAuthorization();

app.MapControllers();

app.Run();
