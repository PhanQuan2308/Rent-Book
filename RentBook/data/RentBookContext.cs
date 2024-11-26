using Microsoft.EntityFrameworkCore;

namespace RentBook.Model
{
    public class RentBookContext : DbContext
    {
        public RentBookContext(DbContextOptions<RentBookContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình quan hệ giữa Rental và Customer (1-N)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)  // Rental có 1 Customer
                .WithMany(c => c.Rentals) // Customer có nhiều Rentals
                .HasForeignKey(r => r.CustomerID); // Khóa ngoại trong bảng Rental

            // Cấu hình bảng RentalDetail
            modelBuilder.Entity<RentalDetail>(entity =>
            {
                // Đặt khóa chính cho RentalDetail
                entity.HasKey(rd => rd.RentalDetailID);

                // Cấu hình các thuộc tính
                entity.Property(rd => rd.Quantity).IsRequired(); // Quantity là bắt buộc
                entity.Property(rd => rd.PricePerDay).HasColumnType("decimal(10, 2)"); // Cấu hình kiểu dữ liệu của PricePerDay

                // Cấu hình quan hệ với Rental
                entity.HasOne(rd => rd.Rental) // RentalDetail có 1 Rental
                    .WithMany(r => r.RentalDetails) // Rental có nhiều RentalDetails
                    .HasForeignKey(rd => rd.RentalID) // Khóa ngoại trong bảng RentalDetails
                    .OnDelete(DeleteBehavior.Restrict); // Không xóa khi Rental bị xóa (để bảo vệ dữ liệu)

                // Cấu hình quan hệ với ComicBook
                entity.HasOne(rd => rd.ComicBook) // RentalDetail có 1 ComicBook
                    .WithMany(c => c.RentalDetails) // ComicBook có nhiều RentalDetails
                    .HasForeignKey(rd => rd.ComicBookID) // Khóa ngoại trong bảng RentalDetails
                    .OnDelete(DeleteBehavior.Restrict); // Không xóa khi ComicBook bị xóa (để bảo vệ dữ liệu)
            });
        }
    }
}
