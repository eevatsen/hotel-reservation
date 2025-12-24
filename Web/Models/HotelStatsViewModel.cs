namespace Web.Models;

public class HotelStatsViewModel
{
    public string HotelName { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}