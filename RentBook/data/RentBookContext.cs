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
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)  
                .WithMany(c => c.Rentals) 
                .HasForeignKey(r => r.CustomerID); 

            modelBuilder.Entity<RentalDetail>(entity =>
            {
                entity.HasKey(rd => rd.RentalDetailID);

                entity.Property(rd => rd.Quantity).IsRequired(); 
                entity.Property(rd => rd.PricePerDay).HasColumnType("decimal(10, 2)"); 

                entity.HasOne(rd => rd.Rental)
                    .WithMany(r => r.RentalDetails) 
                    .HasForeignKey(rd => rd.RentalID) 
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(rd => rd.ComicBook) 
                    .WithMany(c => c.RentalDetails) 
                    .HasForeignKey(rd => rd.ComicBookID) 
                    .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}
