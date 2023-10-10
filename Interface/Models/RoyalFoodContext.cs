using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Interface.Models
{
    public partial class RoyalFoodContext : DbContext
    {
        public RoyalFoodContext()
        {
        }

        public RoyalFoodContext(DbContextOptions<RoyalFoodContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItemMeal> CartItemMeals { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerService> CustomerServices { get; set; } = null!;
        public virtual DbSet<CutomerBanned> CutomerBanneds { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<ImageItem> ImageItems { get; set; } = null!;
        public virtual DbSet<ImageMeal> ImageMeals { get; set; } = null!;
        public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
        public virtual DbSet<IngredientItem> IngredientItems { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<ItemMeal> ItemMeals { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Meal> Meals { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-98SV0L9;Database=RoyalFood;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Cart_User");
            });

            modelBuilder.Entity<CartItemMeal>(entity =>
            {
                entity.ToTable("CartItemMeal");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItemMeals)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_CartItem_Cart");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.CartItemMeals)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_CartItem_Item");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.CartItemMeals)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_CartItemMeal_Meal");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.CategoryNameAr).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DescriptionAr).HasMaxLength(50);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Category_Image");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Customer_User");
            });

            modelBuilder.Entity<CustomerService>(entity =>
            {
                entity.ToTable("CustomerService");

                entity.Property(e => e.Query).HasMaxLength(500);

                entity.Property(e => e.UserResponse).HasMaxLength(500);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.CustomerServices)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CustomerService_User");
            });

            modelBuilder.Entity<CutomerBanned>(entity =>
            {
                entity.HasKey(e => e.CustomerBannedId);

                entity.ToTable("CutomerBanned");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");
            });

            modelBuilder.Entity<ImageItem>(entity =>
            {
                entity.ToTable("ImageItem");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ImageItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ImageItem_Item");
            });

            modelBuilder.Entity<ImageMeal>(entity =>
            {
                entity.ToTable("ImageMeal");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.ImageMeals)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_ImageMeal_Meal");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");

                entity.Property(e => e.Describtion).HasMaxLength(500);

                entity.Property(e => e.DescribtionAr).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NameAr).HasMaxLength(50);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Ingredient_Image");
            });

            modelBuilder.Entity<IngredientItem>(entity =>
            {
                entity.HasKey(e => e.IngridientItemId);

                entity.ToTable("IngredientItem");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.IngredientItems)
                    .HasForeignKey(d => d.IngredientId)
                    .HasConstraintName("FK_IngredientItem_Ingredient");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.IngredientItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_IngredientItem_Item");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.ItemDescribtion).HasMaxLength(500);

                entity.Property(e => e.ItemDescriptionAr).HasMaxLength(500);

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ItemNameAr).HasMaxLength(50);

                entity.Property(e => e.LastModificationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .HasConstraintName("FK_Item_User");
            });

            modelBuilder.Entity<ItemMeal>(entity =>
            {
                entity.ToTable("ItemMeal");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ItemMeals)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ItemMeal_Item");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.ItemMeals)
                    .HasForeignKey(d => d.MealId)
                    .HasConstraintName("FK_ItemMeal_Meal");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.ToTable("Login");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Login_User");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.ToTable("Meal");

                entity.Property(e => e.LastModificationDate).HasColumnType("datetime");

                entity.Property(e => e.MealDescription).HasMaxLength(500);

                entity.Property(e => e.MealDescriptionAr).HasMaxLength(500);

                entity.Property(e => e.MealName).HasMaxLength(50);

                entity.Property(e => e.MealNameAr).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Meal_Category");

                entity.HasOne(d => d.LastModifiedUser)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.LastModifiedUserId)
                    .HasConstraintName("FK_Meal_User");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.CustomerNotes).HasMaxLength(500);

                entity.Property(e => e.DelivaryAddress).HasMaxLength(500);

                entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RatingandFeedback).HasMaxLength(500);

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("FK_Order_Cart");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("FK_Order_OrderStatus");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK_Order_Payment");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Permissions).IsUnicode(false);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasDefaultValueSql("((7))");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
