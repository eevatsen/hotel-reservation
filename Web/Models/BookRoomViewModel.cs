using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class BookRoomViewModel
{
    public int RoomId { get; set; }
    public string? RoomName { get; set; }
    public decimal PricePerNight { get; set; }

    [Required(ErrorMessage = "Вкажіть дату заїзду")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата заїзду")]
    public DateTime CheckInDate { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "Вкажіть дату виїзду")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата виїзду")]
    public DateTime CheckOutDate { get; set; } = DateTime.Today.AddDays(2);
}