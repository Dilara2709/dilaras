using System.ComponentModel.DataAnnotations;

namespace backend.Dtos;

public class CreateTicketRequest
{
    [Required(ErrorMessage = "Тема обязательна")]
    [StringLength(80, MinimumLength = 5, ErrorMessage = "Тема: от 5 до 80 символов")]
    public string Title { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "SiteId должен быть больше 0")]
    public int SiteId { get; set; }

    [Required(ErrorMessage = "Описание обязательно")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Описание: от 10 до 500 символов")]
    public string Description { get; set; } = "";
}