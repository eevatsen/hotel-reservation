using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Domain.Entities;
using Infrastructure.Persistence;
using Web.Models; 

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var hotels = await _context.Hotels.Include(h => h.Rooms).ToListAsync();
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
}