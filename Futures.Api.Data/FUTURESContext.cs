using Futures.Api.Data.Interfaces;
using Futures.Api.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Futures.Api.Data
{
    public class FuturesContext : DbContext, IFuturesContext
    {
        private readonly ILogger<FuturesContext> _logger;
       
        public FuturesContext(DbContextOptions<FuturesContext> options, ILogger<FuturesContext> logger) 
            : base(options)
        {
            _logger = logger;
        }

        public FuturesContext() : base()
        {
                
        }



        public virtual DbSet<Dish>  Dishes { get; set; } = null!;
        public virtual DbSet<FoodCategory> FoodCategories { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDish> OrderDishes { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<Rider> Riders { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Zone> Zones { get; set; } = null!;

        public async Task SaveChangesAsync()
        {
            _logger.LogInformation("Your changes have been saved to the database");
            await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("dishes");

                entity.Property(e => e.DishId).HasColumnName("dish_id");

                entity.Property(e => e.DishDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("dish_description");

                entity.Property(e => e.DishName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("dish_name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Require18).HasColumnName("require_18");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__dishes__restaura__3A81B327");
            });

            modelBuilder.Entity<FoodCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryName)
                    .HasName("PK__food_cat__5189E25488FF6005");

                entity.ToTable("food_category");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_name");

                entity.Property(e => e.CategoryDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_description");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.CreatedAt)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("created_at");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.OrderStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("order_status");

                entity.Property(e => e.RiderId).HasColumnName("rider_id");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__orders__id_user__38996AB5");

                entity.HasOne(d => d.Rider)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RiderId)
                    .HasConstraintName("FK__orders__rider_id__398D8EEE");
            });

            modelBuilder.Entity<OrderDish>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("order_dishes");

                entity.Property(e => e.DishId).HasColumnName("dish_id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Dish)
                    .WithMany()
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK__order_dis__dish___37A5467C");

                entity.HasOne(d => d.Order)
                    .WithMany()
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__order_dis__order__36B12243");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("restaurants");

                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("category_name");

                entity.Property(e => e.RestaurantAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("restaurant_address");

                entity.Property(e => e.RestaurantName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("restaurant_name");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.HasOne(d => d.CategoryNameNavigation)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.CategoryName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__restauran__categ__34C8D9D1");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("FK__restauran__zone___35BCFE0A");
            });

            modelBuilder.Entity<Rider>(entity =>
            {
                entity.ToTable("riders");

                entity.Property(e => e.RiderId).HasColumnName("rider_id");

                entity.Property(e => e.RiderAvailability)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("rider_availability");

                entity.Property(e => e.RiderName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("rider_name");

                entity.Property(e => e.Surname)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("surname");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Riders)
                    .HasForeignKey(d => d.ZoneId)
                    .HasConstraintName("FK__riders__zone_id__33D4B598");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__users__D2D14637A156CA63");

                entity.ToTable("users");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.CreatedAt)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("created_at");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("full_name");

                entity.Property(e => e.IsOver18).HasColumnName("is_over_18");

                entity.Property(e => e.LastUpdate)
                    .HasColumnType("datetime")
                    .HasColumnName("last_update");

                entity.Property(e => e.UserAddress)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_address");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.ToTable("zones");

                entity.Property(e => e.ZoneId).HasColumnName("zone_id");

                entity.Property(e => e.ZoneDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("zone_description");
            });
        }
    }
}
