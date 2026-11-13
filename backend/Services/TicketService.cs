using backend.Dtos;
using backend.Models;

namespace backend.Services;

public class TicketService : ITicketService
{
    private readonly List<Ticket> _items = new();
    private int _nextId = 1;

    public List<Ticket> GetAll() => _items.ToList();

    public Ticket? GetById(int id) => _items.FirstOrDefault(t => t.Id == id);

    public Ticket Create(CreateTicketRequest request, int userId)
    {
        var ticket = new Ticket
        {
            Id = _nextId,
            Number = $"TMP-{_nextId}",  // как в методичке
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            SiteId = request.SiteId,
            Status = "New",
            CreatedByUserId = userId,
            AssigneeUserId = null
        };
        _items.Add(ticket);
        _nextId++;
        return ticket;
    }

    public Ticket? Assign(int id, int assigneeUserId)
    {
        var ticket = _items.FirstOrDefault(t => t.Id == id);
        if (ticket is null) return null;
        ticket.AssigneeUserId = assigneeUserId;
        return ticket;
    }

    public Ticket? ChangeStatus(int id, string status)
    {
        var ticket = _items.FirstOrDefault(t => t.Id == id);
        if (ticket is null) return null;
        ticket.Status = status;
        return ticket;
    }
}