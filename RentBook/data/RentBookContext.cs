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
         .HasOne<Customer>()
         .WithMany()
         .HasForeignKey(r => r.CustomerID)
         .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Rental)
                .WithMany(r => r.RentalDetails)
                .HasForeignKey(rd => rd.RentalID);

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.ComicBook)
                .WithMany(cb => cb.RentalDetails)
                .HasForeignKey(rd => rd.ComicBookID);
        }
    }
}
