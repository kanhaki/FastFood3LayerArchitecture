using Microsoft.EntityFrameworkCore;
using DAT.Entity; // namespace chứa các entity class

namespace DAT
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet cho từng bảng
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<FoodItemPromotion> FoodItemPromotions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostImage> BlogPostImages { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nếu muốn map đúng tên bảng trong MySQL (snake_case)
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Address>().ToTable("addresses");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<FoodItem>().ToTable("food_items");
            modelBuilder.Entity<Promotion>().ToTable("promotions");
            modelBuilder.Entity<FoodItemPromotion>().ToTable("fooditemspromotions");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderItem>().ToTable("orderitems");
            modelBuilder.Entity<BlogPost>().ToTable("blogposts");
            modelBuilder.Entity<BlogPostImage>().ToTable("blogpostimages");
            modelBuilder.Entity<Reservation>().ToTable("reservations");
            modelBuilder.Entity<Review>().ToTable("reviews");
            modelBuilder.Entity<Contact>().ToTable("contacts");

            // Cấu hình bảng trung gian N-N FoodItemPromotion
            modelBuilder.Entity<FoodItemPromotion>()
                .HasKey(fp => new { fp.FoodId, fp.PromoId });

            modelBuilder.Entity<FoodItemPromotion>()
                .HasOne(fp => fp.FoodItem)
                .WithMany(f => f.FoodItemPromotions)
                .HasForeignKey(fp => fp.FoodId);

            modelBuilder.Entity<FoodItemPromotion>()
                .HasOne(fp => fp.Promotion)
                .WithMany(p => p.FoodItemPromotions)
                .HasForeignKey(fp => fp.PromoId);
        }
    }
}
