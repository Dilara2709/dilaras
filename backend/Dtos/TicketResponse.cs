namespace backend.Dtos;

public class TicketResponse
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "";
    public int SiteId { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }
}