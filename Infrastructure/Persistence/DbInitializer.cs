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
            var hotels = new List<Hotel>
            {
                new Hotel
                {
                    Name = "Grand Kyiv Hotel",
                    Address = "Kyiv, Khreschatyk 1",
                    Description = "Розкішний готель у самому серці столиці. Ідеальний вибір для бізнесу та відпочинку.",
                    Rooms = new List<Room>
                    {
                        new Room { Name = "101", PricePerNight = 1500, Capacity = 2 },
                        new Room { Name = "102", PricePerNight = 2500, Capacity = 3 },
                        new Room { Name = "Lux", PricePerNight = 5000, Capacity = 4 }
                    }
                },

                new Hotel
                {
                    Name = "Lviv Coffee Residence",
                    Address = "Lviv, Rynok Square 10",
                    Description = "Атмосферний готель у старовинному будинку з ароматом кави та історією.",
                    Rooms = new List<Room>
                    {
                        new Room { Name = "Classic", PricePerNight = 1200, Capacity = 2 },
                        new Room { Name = "Panorama", PricePerNight = 2000, Capacity = 2 },
                        new Room { Name = "Family", PricePerNight = 2800, Capacity = 4 }
                    }
                },

                new Hotel
                {
                    Name = "Odesa Sea Breeze",
                    Address = "Odesa, Arcadia 5",
                    Description = "Сучасний готель на березі моря з власним пляжем та басейном.",
                    Rooms = new List<Room>
                    {
                        new Room { Name = "Standard Sea View", PricePerNight = 1800, Capacity = 2 },
                        new Room { Name = "Suite", PricePerNight = 3500, Capacity = 3 },
                        new Room { Name = "Presidential", PricePerNight = 6000, Capacity = 2 }
                    }
                },

                new Hotel
                {
                    Name = "Bukovel Mountain Resort",
                    Address = "Bukovel, Vyshneva 1",
                    Description = "Найкраще місце для лижного відпочинку взимку та походів влітку.",
                    Rooms = new List<Room>
                    {
                        new Room { Name = "Cottage", PricePerNight = 4000, Capacity = 6 },
                        new Room { Name = "Double Room", PricePerNight = 2200, Capacity = 2 }
                    }
                }
            };

            context.Hotels.AddRange(hotels);
            await context.SaveChangesAsync();
        }
    }
}