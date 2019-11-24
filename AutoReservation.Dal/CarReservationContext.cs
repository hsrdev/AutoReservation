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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateCarTable(modelBuilder);
            CreateCustomerTable(modelBuilder);
            CreateReservationTable(modelBuilder);
        }

        private void CreateCarTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .ToTable("Cars")
                .Property(e => e.RowVersion)
                .IsRowVersion();
            modelBuilder.Entity<StandardCar>()
                .HasBaseType<Car>();
            modelBuilder.Entity<LuxuryClassCar>()
                .HasBaseType<Car>();
            modelBuilder.Entity<MidClassCar>()
                .HasBaseType<Car>();
        }

        private void CreateCustomerTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToTable("Customers")
                .Property(e => e.RowVersion)
                .IsRowVersion();
        }

        private void CreateReservationTable(ModelBuilder modelBuilder)
        {
            var reservationBuilder = modelBuilder.Entity<Reservation>()
                .ToTable("Reservations");
            reservationBuilder.HasKey(e => e.ReservationNr)
                .HasName("Id");
            reservationBuilder.HasOne(e => e.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(e => e.CustomerId)
                .HasConstraintName("FK_Reservation_CustomerId")
                .IsRequired();
            reservationBuilder.HasOne(e => e.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(e => e.CarId)
                .HasConstraintName("FK_Reservation_CarId")
                .IsRequired();
            reservationBuilder.Property(e => e.RowVersion)
                .IsRowVersion();
        }
    }
}