namespace CarDealer.Data
{
    using Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class CarDealerDbContext : IdentityDbContext<User>
    {
        public CarDealerDbContext(DbContextOptions<CarDealerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Sale> Sales { get; set; }
        
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        
        public DbSet<Part> Parts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Sale>()
                .HasOne(x => x.Car)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.CarId);

            builder
                .Entity<Sale>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.CustomerId);

            builder
                .Entity<Supplier>()
                .HasMany(x => x.Parts)
                .WithOne(x => x.Supplier)
                .HasForeignKey(x => x.SupplierId);

            builder
                .Entity<PartCar>()
                .HasKey(x => new { x.PartId, x.CarId });

            builder
                .Entity<PartCar>()
                .HasOne(x => x.Car)
                .WithMany(x => x.Parts)
                .HasForeignKey(x => x.CarId);

            builder
               .Entity<PartCar>()
               .HasOne(x => x.Part)
               .WithMany(x => x.Cars)
               .HasForeignKey(x => x.PartId);

            base.OnModelCreating(builder);
        }
    }
}
