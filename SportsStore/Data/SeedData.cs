using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using SportsStore.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        SportsStoreDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<SportsStoreDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Chess Set", Description = "A quality chess set", Category = "Chess", Price = 75 },
                new Product { Name = "Chess Board", Description = "A premium wooden board", Category = "Chess", Price = 45 },
                new Product { Name = "Chess Pieces", Description = "Hand carved chess pieces", Category = "Chess", Price = 32 },
                new Product { Name = "Soccer Ball", Description = "FIFA approved size and weight", Category = "Soccer", Price = 19.50M },
                new Product { Name = "Corner Flags", Description = "Give your pitch a pro look", Category = "Soccer", Price = 34.95M },
                new Product { Name = "Stadium", Description = "Flat pack 75,000 capacity", Category = "Soccer", Price = 79500 },
                new Product { Name = "Kayak", Description = "A boat for one person", Category = "Watersports", Price = 275 },
                new Product { Name = "Lifejacket", Description = "Protective and fashionable", Category = "Watersports", Price = 48.95M },
                new Product { Name = "Paddle", Description = "A paddle for one person", Category = "Watersports", Price = 19.50M }
            );
            context.SaveChanges();
        }
    }
}