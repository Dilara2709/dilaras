namespace backend.Models;

public class Trip
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "New";
    public int CarriageId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }       // nullable
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}