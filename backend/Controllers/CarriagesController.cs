
using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/carriages")]
[Produces("application/json")]
public class CarriagesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CarriagesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var carriages = await _db.Carriages
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Type
            })
            .ToListAsync();

        return Ok(carriages);
    }
}
