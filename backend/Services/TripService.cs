
using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class TripService : ITripService
{
    private readonly AppDbContext _db;

    public TripService(AppDbContext db)
    {
        _db = db;
    }

    public List<Trip> GetAll(
    string? status = null,
    int? carriageId = null)
{
    var query = _db.Trips.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(status))
    {
        if (status != "New" &&
            status != "InProgress" &&
            status != "Closed" &&
            status != "Cancelled")
        {
            throw new ArgumentException(
                "Недопустимый статус поездки.");
        }

        query = query.Where(t => t.Status == status);
    }

    if (carriageId.HasValue)
    {
        if (carriageId.Value <= 0)
        {
            throw new ArgumentException(
                "CarriageId должен быть больше 0.");
        }

        query = query.Where(
            t => t.CarriageId == carriageId.Value);
    }

    return query
        .OrderByDescending(t => t.CreatedAt)
        .ToList();
}

    public Trip? GetById(int id) =>
        _db.Trips.AsNoTracking()
            .FirstOrDefault(t => t.Id == id);

    public Trip Create(CreateTripRequest request, int userId)
    {
        // Удаляем лишние пробелы
        var title = request.Title?.Trim() ?? "";
        var description = request.Description?.Trim() ?? "";

        // Проверяем длину названия
        if (title.Length < 5 || title.Length > 80)
        {
            throw new ArgumentException(
                "Тема должна содержать от 5 до 80 символов.");
        }

        // Проверяем длину описания
        if (description.Length < 10 || description.Length > 500)
        {
            throw new ArgumentException(
                "Описание должно содержать от 10 до 500 символов.");
        }

        // Проверяем существование типа вагона
        if (!_db.Carriages.Any(c => c.Id == request.CarriageId))
        {
            throw new ArgumentException(
                "Указанный тип вагона не существует.");
        }

        // Проверяем существование создателя поездки
        if (!_db.Users.Any(u => u.Id == userId))
        {
            throw new ArgumentException(
                "Пользователь не существует.");
        }

        var trip = new Trip
        {
            Number = $"TMP-{DateTime.UtcNow.Ticks:X16}",
            Title = title,
            Description = description,
            CarriageId = request.CarriageId,
            Status = "New",
            CreatedByUserId = userId,
            AssigneeUserId = null,
            CreatedAt = DateTime.UtcNow
        };

        _db.Trips.Add(trip);
        _db.SaveChanges();

        return trip;
    }

    public Trip? Assign(int id, int assigneeUserId)
    {
        var trip = _db.Trips.FirstOrDefault(t => t.Id == id);

        if (trip is null)
            return null;

        // Нельзя назначать проводника на завершённую
        // или отменённую поездку
        if (trip.Status == "Closed" ||
            trip.Status == "Cancelled")
        {
            throw new InvalidOperationException(
                "Нельзя назначить проводника на завершённую или отменённую поездку.");
        }

        // Проверяем существование пользователя
        var user = _db.Users.AsNoTracking()
            .FirstOrDefault(u => u.Id == assigneeUserId);

        if (user is null)
        {
            throw new ArgumentException(
                "Назначаемый пользователь не существует.");
        }

        // Ответственным может быть только проводник
        if (user.Role != "conductor")
        {
            throw new ArgumentException(
                "Ответственным может быть только проводник.");
        }

        // Создатель не может быть ответственным
        // за собственную поездку
        if (trip.CreatedByUserId == assigneeUserId)
        {
            throw new InvalidOperationException(
                "Создатель поездки не может быть назначен ответственным за неё.");
        }

        trip.AssigneeUserId = assigneeUserId;

        _db.SaveChanges();

        return trip;
    }

    public Trip? ChangeStatus(int id, string status)
    {
        var trip = _db.Trips.FirstOrDefault(t => t.Id == id);

        if (trip is null)
            return null;

        // Проверяем допустимость значения статуса
        if (status != "New" &&
            status != "InProgress" &&
            status != "Closed" &&
            status != "Cancelled")
        {
            throw new ArgumentException(
                "Недопустимый статус поездки.");
        }

        // Проверяем допустимость перехода
        bool allowed =
            (trip.Status == "New" && status == "InProgress") ||
            (trip.Status == "InProgress" && status == "Closed") ||
            (trip.Status == "New" && status == "Cancelled");

        if (!allowed)
        {
            throw new InvalidOperationException(
                $"Переход из статуса {trip.Status} в {status} запрещён.");
        }

        // Для начала поездки необходим проводник
        if (status == "InProgress" &&
            trip.AssigneeUserId is null)
        {
            throw new InvalidOperationException(
                "Перед началом поездки необходимо назначить проводника.");
        }

        trip.Status = status;

        _db.SaveChanges();

        return trip;
    }
}
