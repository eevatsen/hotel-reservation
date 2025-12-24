using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Client"));
        }

        var adminEmail = "admin@hotel.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "admin123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        if (!context.Hotels.Any())
        {
            var hotel = new Hotel
            {
                Name = "Grand Kyiv Hotel",
                Address = "Kyiv, Khreschatyk 1",
                Description = "Luxury hotel in the city center.",
                Rooms = new List<Room>
                {
                    new Room { Name = "101", PricePerNight = 1500, Capacity = 2 },
                    new Room { Name = "102", PricePerNight = 2500, Capacity = 3 },
                    new Room { Name = "Lux", PricePerNight = 5000, Capacity = 4 }
                }
            };

            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();
        }
    }
}