using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;
    private readonly ILogger<TicketsController> _logger;

    public TicketsController(ITicketService service, ILogger<TicketsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET /api/tickets
    [HttpGet]
    public ActionResult<List<TicketResponse>> GetAll()
    {
        var items = _service.GetAll().Select(ToResponse).ToList();
        return Ok(items);  // 200, даже если пусто
    }

    // GET /api/tickets/{id}
    [HttpGet("{id}")]
    public ActionResult<TicketResponse> GetById(int id)
    {
        var ticket = _service.GetById(id);
        if (ticket is null) return NotFound();  // 404
        return Ok(ToResponse(ticket));
    }

    // POST /api/tickets
    [HttpPost]
    public ActionResult<TicketResponse> Create([FromBody] CreateTicketRequest request)
    {
        try
        {
            var created = _service.Create(request, userId: 1); // userId-заглушка, вход будет в ЛР11
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                ToResponse(created));  // 201
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CreateTicket failed; SiteId={SiteId}; error={Error}",
                request.SiteId, ex.GetType().Name);
            return Problem(statusCode: 500, title: "Не удалось обработать запрос");
        }
    }

    // PATCH /api/tickets/{id}/assignee
    [HttpPatch("{id}/assignee")]
    public ActionResult<TicketResponse> Assign(int id, [FromBody] UpdateAssigneeRequest request)
    {
        var ticket = _service.GetById(id);
        if (ticket is null) return NotFound();

        if (request.AssigneeUserId <= 0)
            return BadRequest(new { message = "AssigneeUserId должен быть больше 0" });

        var updated = _service.Assign(id, request.AssigneeUserId);
        return Ok(ToResponse(updated!));
    }

    // PATCH /api/tickets/{id}/status
    [HttpPatch("{id}/status")]
    public ActionResult<TicketResponse> ChangeStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        var ticket = _service.GetById(id);
        if (ticket is null) return NotFound();

        var allowed = new[] { "New", "InProgress", "Closed", "Cancelled" };
        if (!allowed.Contains(request.Status))
            return BadRequest(new { message = "Недопустимый статус" });

        // Правило процесса: нельзя в InProgress без исполнителя → 409
        if (request.Status == "InProgress" && ticket.AssigneeUserId is null)
            return Conflict(new { message = "Нельзя перевести в InProgress без исполнителя" });

        var updated = _service.ChangeStatus(id, request.Status);
        return Ok(ToResponse(updated!));
    }

    private static TicketResponse ToResponse(Ticket t) => new()
    {
        Id = t.Id,
        Number = t.Number,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status,
        SiteId = t.SiteId,
        CreatedByUserId = t.CreatedByUserId,
        AssigneeUserId = t.AssigneeUserId
    };
}