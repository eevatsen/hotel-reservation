using Dapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Web.Models;

namespace Web.Controllers;

[Authorize(Roles = "Admin")] 
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AdminController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Hotels.ToListAsync());
    }

    // створити готель (GET)
    [HttpGet]
    public IActionResult CreateHotel()
    {
        return View();
    }

    // створити готель (POST)
    [HttpPost]
    public async Task<IActionResult> CreateHotel(Hotel hotel)
    {
        if (ModelState.IsValid)
        {
            _context.Add(hotel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(hotel);
    }

    //видалити готель
    [HttpPost]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        var hotel = await _context.Hotels.FindAsync(id);
        if (hotel != null)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // додати номер до готелю 
    [HttpGet]
    public IActionResult CreateRoom(int hotelId)
    {
        ViewData["HotelId"] = hotelId;
        return View();
    }

    // зберегти номер до готелю
    [HttpPost]
    public async Task<IActionResult> CreateRoom(Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }
        ViewData["HotelId"] = room.HotelId;
        return View(room);
    }

    // stats using dapper
    [HttpGet]
    public async Task<IActionResult> Statistics()
    {

        var sql = @"
            SELECT 
                h.Name AS HotelName, 
                COUNT(b.Id) AS TotalBookings, 
                COALESCE(SUM(b.TotalPrice), 0) AS TotalRevenue
            FROM Hotels h
            LEFT JOIN Rooms r ON h.Id = r.HotelId
            LEFT JOIN Bookings b ON r.Id = b.RoomId
            GROUP BY h.Id, h.Name";

        using (var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            var stats = await connection.QueryAsync<HotelStatsViewModel>(sql);
            return View(stats);
        }
    }
}