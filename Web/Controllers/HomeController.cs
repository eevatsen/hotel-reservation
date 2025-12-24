using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Domain.Entities;
using Infrastructure.Persistence;
using Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; 

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? city, int? guestCount)
    {
        var hotelsQuery = _context.Hotels.Include(h => h.Rooms).AsQueryable();

        if (!string.IsNullOrEmpty(city))
        {
            hotelsQuery = hotelsQuery.Where(h =>
                h.Address.Contains(city) || h.Name.Contains(city));
        }

        if (guestCount.HasValue)
        {
            hotelsQuery = hotelsQuery.Where(h =>
                h.Rooms.Any(r => r.Capacity >= guestCount.Value));
        }

        var hotels = await hotelsQuery.ToListAsync();

        ViewData["CurrentCity"] = city;
        ViewData["CurrentGuestCount"] = guestCount;

        return View(hotels);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id == 0) return NotFound();

        var hotel = await _context.Hotels
            .Include(h => h.Rooms)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null)
        {
            return NotFound();
        }

        return View(hotel);
    }

    // сторінка бронювання
    [Authorize] 
    [HttpGet]
    public async Task<IActionResult> BookRoom(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room == null) return NotFound();

        var model = new BookRoomViewModel
        {
            RoomId = room.Id,
            RoomName = room.Name,
            PricePerNight = room.PricePerNight
        };

        return View(model);
    }

    // підтвердження бронювання
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> BookRoom(BookRoomViewModel model)
    {
        // валідація
        if (model.CheckInDate < DateTime.Today)
        {
            ModelState.AddModelError("CheckInDate", "Дата заїзду не може бути в минулому.");
        }
        if (model.CheckInDate >= model.CheckOutDate)
        {
            ModelState.AddModelError("CheckOutDate", "Дата виїзду має бути пізніше дати заїзду.");
        }

        if (!ModelState.IsValid) return View(model);

        // перевірка доступності номера
        bool isBooked = await _context.Bookings.AnyAsync(b =>
            b.RoomId == model.RoomId &&
            b.CheckInDate < model.CheckOutDate &&
            b.CheckOutDate > model.CheckInDate);

        if (isBooked)
        {
            ModelState.AddModelError(string.Empty, "На жаль, цей номер вже зайнятий на вибрані дати. Спробуйте інші дати.");
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var totalDays = (int)(model.CheckOutDate - model.CheckInDate).TotalDays;
        var totalPrice = totalDays * model.PricePerNight;

        var booking = new Booking
        {
            UserId = userId,
            RoomId = model.RoomId,
            CheckInDate = model.CheckInDate,
            CheckOutDate = model.CheckOutDate,
            TotalPrice = totalPrice,
            CreatedAt = DateTime.UtcNow
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var bookings = await _context.Bookings
            .Include(b => b.Room)
            .ThenInclude(r => r.Hotel)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CheckInDate) 
            .Select(b => new MyBookingViewModel
            {
                Id = b.Id,
                HotelName = b.Room.Hotel.Name,
                RoomName = b.Room.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalPrice = b.TotalPrice
            })
            .ToListAsync();

        return View(bookings);
    }
}