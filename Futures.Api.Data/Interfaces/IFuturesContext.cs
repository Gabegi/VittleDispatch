using Futures.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Futures.Api.Data.Interfaces
{
    /// <summary>
    /// Interface for the DB Context. This allows us to use DI
    /// </summary>
    public interface IFuturesContext : IDisposable
    {
        DbSet<Dish> Dishes { get; set; }
        DbSet<FoodCategory> FoodCategories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDish> OrderDishes { get; set; }
        DbSet<Restaurant> Restaurants { get; set; }
        DbSet<Rider> Riders { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Zone> Zones { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task SaveChangesAsync();
        // void Remove<T>(ValueTask<T?> entity) where T : class;
    }
}
