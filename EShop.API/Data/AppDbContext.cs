using System;
using EShop.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Unique index on Email
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Price precision
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Entity<OrderItem>()
            .Property(oi => oi.PriceAtPurchase)
            .HasPrecision(18, 2);

        // CartItem unique per Cart+Product
        builder.Entity<CartItem>()
            .HasIndex(ci => new { ci.CartId, ci.ProductId })
            .IsUnique();

        // Relations & delete behaviors
        builder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany(p => p.CartItems)
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
        

// ✅ Users
    builder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            Username = "John Doe",
            Email = "john@example.com",
            PasswordHash = "hashedpassword123",
            CreatedAt = new DateTime(2025, 01, 01)
        },
        new User
        {
            Id = 2,
            Username = "Jane Smith",
            Email = "jane@example.com",
            PasswordHash = "hashedpassword456",
            CreatedAt = new DateTime(2025, 01, 01)
        }
    );

    // ✅ Categories
    builder.Entity<Category>().HasData(
        new Category
        {
            Id = 1,
            Name = "Electronics",
            Description = "Electronic devices and gadgets"
        },
        new Category
        {
            Id = 2,
            Name = "Clothing",
            Description = "Men and women clothing"
        }
    );

    // ✅ Products
    builder.Entity<Product>().HasData(
        new Product
        {
            Id = 1,
            Name = "Smartphone",
            Description = "Latest Android smartphone",
            Price = 500,
            CategoryId = 1
        },
        new Product
        {
            Id = 2,
            Name = "Laptop",
            Description = "High-performance laptop",
            Price = 1200,
            CategoryId = 1
        },
        new Product
        {
            Id = 3,
            Name = "T-Shirt",
            Description = "Cotton T-Shirt",
            Price = 20,
            CategoryId = 2
        }
    );

    // ✅ Carts
    builder.Entity<Cart>().HasData(
        new Cart
        {
            Id = 1,
            UserId = 1,
            CreatedAt = new DateTime(2025, 01, 02)
        },
        new Cart
        {
            Id = 2,
            UserId = 2,
            CreatedAt = new DateTime(2025, 01, 02)
        }
    );

    // ✅ CartItems
    builder.Entity<CartItem>().HasData(
        new CartItem
        {
            Id = 1,
            CartId = 1,
            ProductId = 1,
            Quantity = 1
        },
        new CartItem
        {
            Id = 2,
            CartId = 2,
            ProductId = 3,
            Quantity = 2
        }
    );

    // ✅ Orders
    builder.Entity<Order>().HasData(
        new Order
        {
            Id = 1,
            UserId = 1,
            OrderDate = new DateTime(2025, 01, 05),
            TotalAmount = 500,
            Status = "Completed"
        },
        new Order
        {
            Id = 2,
            UserId = 2,
            OrderDate = new DateTime(2025, 01, 06),
            TotalAmount = 40,
            Status = "Pending"
        }
    );

    // ✅ OrderItems
    builder.Entity<OrderItem>().HasData(
        new OrderItem
        {
            Id = 1,
            OrderId = 1,
            ProductId = 1,
            Quantity = 1,
            PriceAtPurchase = 500
        },
        new OrderItem
        {
            Id = 2,
            OrderId = 2,
            ProductId = 3,
            Quantity = 2,
            PriceAtPurchase = 20
        }
    );

    // ✅ Reviews
    builder.Entity<Review>().HasData(
        new Review
        {
            Id = 1,
            UserId = 1,
            ProductId = 1,
            Rating = 5,
            Comment = "Great phone!",
            CreatedAt = new DateTime(2025, 01, 07)
        },
        new Review
        {
            Id = 2,
            UserId = 2,
            ProductId = 3,
            Rating = 4,
            Comment = "Nice quality T-shirt",
            CreatedAt = new DateTime(2025, 01, 07)
        }
    );
}

}
