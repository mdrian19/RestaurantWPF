using System.Configuration;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Models;

namespace RestaurantApp.DataAccessLayer
{
    public class RestaurantDBContext : DbContext
    {
        public RestaurantDBContext() { }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Allergen> Allergens { get; set; } = null!;
        public DbSet<Dish> Dishes { get; set; } = null!;
        public DbSet<Menu> Menus { get; set; } = null!;
        public DbSet<UserAccount> UserAccounts { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cs = ConfigurationManager.ConnectionStrings["RestaurantDB"]?.ConnectionString;
            optionsBuilder.UseSqlServer(cs);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(c => c.CategoryID);
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Allergen>(e =>
            {
                e.ToTable("Allergen");
                e.HasKey(a => a.AllergenID);
                e.Property(a => a.Name).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<Dish>(e =>
            {
                e.ToTable("Dish");
                e.HasKey(d => d.DishID);
                e.Property(d => d.Name).HasMaxLength(200).IsRequired();
                e.Property(d => d.Price).HasColumnType("decimal(10,2)");
                e.Property(d => d.PortionQuantity).HasColumnType("decimal(10,3)");
                e.Property(d => d.PortionUnit).HasMaxLength(10).HasDefaultValue("g");
                e.Property(d => d.TotalQuantity).HasColumnType("decimal(10,3)");

                e.Ignore(d => d.IsAvailable);
                e.Ignore(d => d.DisplayQuantity);
                e.Ignore(d => d.CategoryName);

                e.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(d => d.CategoryID);
                e.HasMany(d => d.Allergens)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("DishAllergen"));
            });

            modelBuilder.Entity<Menu>(e =>
            {
                e.ToTable("Menu");
                e.HasKey(m => m.MenuID);
                e.Property(m => m.Name).HasMaxLength(200).IsRequired();
                e.Ignore(m => m.BasePrice);
                e.Ignore(m => m.IsAvailable);
                e.Ignore(m => m.DisplayDishes);
                e.Ignore(m => m.CategoryName);

                e.HasOne<Category>()
                    .WithMany()
                    .HasForeignKey(m => m.CategoryID);
                e.HasMany(m => m.Dishes)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("MenuDish"));
            });

            modelBuilder.Entity<UserAccount>(e =>
            {
                e.ToTable("UserAccount");
                e.HasKey(u => u.UserID);
                e.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                e.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                e.Property(u => u.Email).HasMaxLength(200).IsRequired();
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Role).HasMaxLength(20)
                    .HasDefaultValue("Client");
                e.Ignore(u => u.FullName);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(o => o.OrderID);
                e.Property(o => o.OrderCode).HasMaxLength(50).IsRequired();
                e.HasIndex(o => o.OrderCode).IsUnique();
                e.Property(o => o.Status).HasMaxLength(50).HasDefaultValue("Inregistrata");
                e.Property(o => o.FoodCost).HasColumnType("decimal(10,2)");
                e.Property(o => o.DeliveryCost).HasColumnType("decimal(10,2)");
                e.Property(o => o.DiscountPercent).HasColumnType("decimal(5,2)");
                e.Property(o => o.TotalCost).HasColumnType("decimal(10,2)");
 
                e.Ignore(o => o.ClientFirstName);
                e.Ignore(o => o.ClientLastName);
                e.Ignore(o => o.ClientPhone);
                e.Ignore(o => o.ClientAddress);
                e.Ignore(o => o.IsActive);
                e.Ignore(o => o.Items);
 
                e.HasOne<UserAccount>()
                    .WithMany()
                    .HasForeignKey(o => o.UserID);
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.ToTable("OrderItem");
                e.HasKey(oi => oi.OrderItemID);
                e.Property(oi => oi.UnitPrice).HasColumnType("decimal(10,2)");

                e.Ignore(oi => oi.LineTotal);
                e.Ignore(oi => oi.ProductName);

                e.HasOne<Order>()
                    .WithMany()
                    .HasForeignKey(oi => oi.OrderID);
                e.HasOne<Dish>()
                    .WithMany()
                    .HasForeignKey(oi => oi.DishID)
                    .IsRequired(false);
                e.HasOne<Menu>()
                    .WithMany()
                    .HasForeignKey(oi => oi.MenuID)
                    .IsRequired(false);
            });
        }
    }
}
