using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class TicketService : ITicketService
{
    private readonly AppDbContext _db;

    public TicketService(AppDbContext db)
    {
        _db = db;
    }

    public List<Ticket> GetAll() =>
        _db.Tickets.AsNoTracking().OrderByDescending(t => t.CreatedAt).ToList();

    public Ticket? GetById(int id) =>
        _db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == id);

    public Ticket Create(CreateTicketRequest request, int userId)
    {
        var ticket = new Ticket
        {
            Number = $"TMP-{DateTime.UtcNow.Ticks}",   // уникальный учебный номер
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            CarriageId = request.CarriageId,
            Status = "New",
            CreatedByUserId = userId,
            AssigneeUserId = null,
            CreatedAt = DateTime.UtcNow
        };
        _db.Tickets.Add(ticket);
        _db.SaveChanges();
        return ticket;
    }

    public Ticket? Assign(int id, int assigneeUserId)
    {
        var ticket = _db.Tickets.FirstOrDefault(t => t.Id == id);
        if (ticket is null) return null;
        ticket.AssigneeUserId = assigneeUserId;
        _db.SaveChanges();
        return ticket;
    }

    public Ticket? ChangeStatus(int id, string status)
    {
        var ticket = _db.Tickets.FirstOrDefault(t => t.Id == id);
        if (ticket is null) return null;
        ticket.Status = status;
        _db.SaveChanges();
        return ticket;
    }
}