using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoReservation.Dal
{
    public class CarReservationContext : CarReservationContextBase
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public CarReservationContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateCarTable(modelBuilder);
            CreateCustomerTable(modelBuilder);
            CreateReservationTable(modelBuilder);
        }

        private void CreateCarTable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new CarTypeConfig()
            );
            modelBuilder.Entity<Car>()
                .ToTable("Cars");
            modelBuilder.Entity<StandardCar>()
                .HasBaseType<Car>();
            modelBuilder.Entity<LuxuryClassCar>()
                .HasBaseType<Car>();
            modelBuilder.Entity<MidClassCar>()
                .HasBaseType<Car>();

        }

        private void CreateCustomerTable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new CustomerTypeConfig()
            );
            modelBuilder.Entity<Customer>()
                .ToTable("Customer");
        }

        private void CreateReservationTable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new ReservationTypConfig()
            );
            var reservationBuilder = modelBuilder.Entity<Reservation>();
            reservationBuilder.HasKey(e => e.ReservationNr).HasName("Id");
            reservationBuilder.HasOne(e => e.Customer).WithMany(c => c.Reservations).HasForeignKey(e => e.CustomerId)
                .HasConstraintName("FK_Reservation_CustomerId").IsRequired();
            reservationBuilder.HasOne(e => e.Car).WithMany(c => c.Reservations).HasForeignKey(e => e.CarId)
                .HasConstraintName("FK_Reservation_CarId").IsRequired();
        }
    }

    internal class CarTypeConfig : IEntityTypeConfiguration<Car>
    {
        public void Configure(
            EntityTypeBuilder<Car> builder)
        {
            builder
                .Property(p => p.RowVersion).IsRowVersion();
        }
    }

    internal class CustomerTypeConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(
            EntityTypeBuilder<Customer> builder)
        {
            builder
                .Property(p => p.RowVersion).IsRowVersion();
        }
    }

    internal class ReservationTypConfig : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(
            EntityTypeBuilder<Reservation> builder)
        {
            builder
                .Property(p => p.RowVersion).IsRowVersion();
        }
    }
}