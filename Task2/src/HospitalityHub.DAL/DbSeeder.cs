using HospitalityHub.Core.Entities;
using HospitalityHub.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HospitalityHub.DAL;

public class DbSeeder
{
    private readonly ILogger<DbSeeder> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public DbSeeder(ILogger<DbSeeder> logger,
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedIfNeededAsync()
    {
        _logger.LogInformation("Seeder: Checking if seed is needed");

        // Check if the database is empty
        if (!_context.Hotel.Any())
        {
            _logger.LogInformation("Seeder: Seeding initial data");

            await SeedRolesAsync();
            await SeedUserAsync();

            await SeedHotelsAsync();
            await SeedRoomsAsync();
            await SeedRoomPlaces();
        }
        else
        {
            _logger.LogInformation("Seeder: No seeding needed");
        }
    }

    private async Task SeedRolesAsync()
    {
        _logger.LogInformation("Seeder: Seeding roles");

        await _roleManager.CreateAsync(new IdentityRole<int>("Admin"));

        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeder: Seeded roles");
    }


    private async Task SeedUserAsync()
    {
        _logger.LogInformation("Seeder: Seeding users");

        await _userManager.CreateAsync(new User()
        {
            FirstName = "Arsenii",
            LastName = "Merkulov",
            Email = "arsenii.vm@gmail.com",
            UserName = "arsenii.vm@gmail.com",
            NormalizedEmail = "ARSENII.VM@GMAIL.COM",
            NormalizedUserName = "ARSENII.VM@GMAIL.COM",
        }, "Arsik123456");

        var user = await _userManager.FindByEmailAsync("arsenii.vm@gmail.com");

        await _userManager.AddToRoleAsync(user, "Admin");

        await _context.SaveChangesAsync();
    }

    private async Task SeedRoomsAsync()
    {
        _logger.LogInformation("Seeder: Seeding rooms");

        var room = new Room
        {
            Number = 101,
            Status = ERoomStatus.Available,
            BasePrice = 100,
            DiscountPercent = 0,
            Stage = 1,
            RoomType = ERoomType.Standard,
            HotelId = 1,
            BaseLockUri = "http://localhost:8180",
            ApiKey = "123456789",
        };

        _context.Room.Add(room);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeder: Seeded rooms");
    }

    private async Task SeedRoomPlaces()
    {
        _logger.LogInformation("Seeder: Seeding room places");

        var roomPlaces = new List<RoomPlace>
        {
            new RoomPlace
            {
                Type = ERoomPlaceType.Sofa,
                RoomId = 1,
            },
            new RoomPlace
            {
                Type = ERoomPlaceType.SingleBed,
                RoomId = 1,
            },
        };

        _context.RoomPlace.AddRange(roomPlaces);

        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeder: Seeded room places");
    }

    private async Task SeedHotelsAsync()
    {
        _logger.LogInformation("Seeder: Seeding Hotels");

        var rooms = await _context.Room.ToListAsync();

        var hotel = new Hotel
        {
            Name = "Hotel California",
            Address = "1234 California St",
            City = "San Francisco",
            ZipCode = "94101",
            Country = "US",
            Rooms = rooms,
            LockServiceUri = "http://localhost:8180",
        };

        _context.Hotel.Add(hotel);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeder: Seeded Hotels");
    }
}