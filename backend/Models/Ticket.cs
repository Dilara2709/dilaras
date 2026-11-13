namespace backend.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Status { get; set; } = "New";
    public int SiteId { get; set; }  // вместо CarriageId
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }
}