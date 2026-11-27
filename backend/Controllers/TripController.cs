
using backend.Dtos;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/trips")]
[Produces("application/json")]
public class TripController : ControllerBase
{
    private readonly ITripService _service;
    private readonly ILogger<TripController> _logger;

    public TripController(
        ITripService service,
        ILogger<TripController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET /api/trips
    [HttpGet]
    [ProducesResponseType(typeof(List<TripResponse>), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public ActionResult<List<TripResponse>> GetAll()
    {
        var items = _service.GetAll()
            .Select(ToResponse)
            .ToList();

        return Ok(items);
    }

    // GET /api/trips/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public ActionResult<TripResponse> GetById(int id)
    {
        var trip = _service.GetById(id);

        if (trip is null)
            return Problem(
                statusCode: 404,
                title: "Поездка не найдена");

        return Ok(ToResponse(trip));
    }

    // POST /api/trips
    [HttpPost]
    [ProducesResponseType(typeof(TripResponse), 201)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public ActionResult<TripResponse> Create(
        [FromBody] CreateTripRequest request,
        [FromHeader(Name = "X-User-Id")] int? userId)
    {
        if (!userId.HasValue || userId.Value <= 0)
        {
            return Problem(
                statusCode: 400,
                title: "Некорректные данные",
                detail: "Укажите положительный X-User-Id");
        }

        try
        {
            var created = _service.Create(
                request, userId.Value);

            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                ToResponse(created));
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: 400,
                title: "Некорректные данные",
                detail: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "CreateTrip failed; CarriageId={CarriageId}; UserId={UserId}",
                request.CarriageId, userId.Value);

            return Problem(
                statusCode: 500,
                title: "Не удалось создать поездку");
        }
    }

    // PATCH /api/trips/{id}/assignee
    [HttpPatch("{id}/assignee")]
    [ProducesResponseType(typeof(TripResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public ActionResult<TripResponse> Assign(
        int id,
        [FromBody] UpdateAssigneeRequest request)
    {
        if (request.AssigneeUserId <= 0)
        {
            return Problem(
                statusCode: 400,
                title: "Некорректные данные",
                detail: "AssigneeUserId должен быть больше 0");
        }

        try
        {
            var updated = _service.Assign(
                id, request.AssigneeUserId);

            if (updated is null)
                return Problem(
                    statusCode: 404,
                    title: "Поездка не найдена");

            return Ok(ToResponse(updated));
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: 400,
                title: "Некорректные данные",
                detail: ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(
                statusCode: 409,
                title: "Нарушение бизнес-правил",
                detail: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Assign failed; TripId={TripId}; AssigneeUserId={AssigneeUserId}",
                id, request.AssigneeUserId);

            return Problem(
                statusCode: 500,
                title: "Не удалось назначить проводника");
        }
    }

    // PATCH /api/trips/{id}/status
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(TripResponse), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public ActionResult<TripResponse> ChangeStatus(
        int id,
        [FromBody] UpdateStatusRequest request)
    {
        try
        {
            var updated = _service.ChangeStatus(
                id, request.Status);

            if (updated is null)
                return Problem(
                    statusCode: 404,
                    title: "Поездка не найдена");

            return Ok(ToResponse(updated));
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: 400,
                title: "Некорректные данные",
                detail: ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(
                statusCode: 409,
                title: "Нарушение бизнес-правил",
                detail: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "ChangeStatus failed; TripId={TripId}; Status={Status}",
                id, request.Status);

            return Problem(
                statusCode: 500,
                title: "Не удалось изменить статус поездки");
        }
    }

    private static TripResponse ToResponse(Trip t) => new()
    {
        Id = t.Id,
        Number = t.Number,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status,
        CarriageId = t.CarriageId,
        CreatedByUserId = t.CreatedByUserId,
        AssigneeUserId = t.AssigneeUserId
    };
}
