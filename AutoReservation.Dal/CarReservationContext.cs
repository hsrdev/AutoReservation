using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoReservation.Dal
{
    public class CarReservationContext : CarReservationContextBase
    {
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
            var carBuilder = modelBuilder.Entity<Car>();
            carBuilder.ToTable("Cars");
            carBuilder.HasKey(e => e.Id).HasName("Id");
            carBuilder.Property(e => e.BaseRate).HasColumnName("BaseRate");
            carBuilder.Property(e => e.CarClass).HasColumnName("CarClass");
            carBuilder.Property(e => e.DailyRate).HasColumnName("DailyRate");
            carBuilder.Property(e => e.Make).HasColumnName("Make").HasColumnType("NVARCHAR(20)");
            carBuilder.Property(e => e.RowVersion).HasColumnName("RowVersion").HasColumnType("TIMESTAMP");
        }

        private void CreateCustomerTable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new CustomerTypeConfig()
            );
            var customerBuilder = modelBuilder.Entity<Customer>();
            customerBuilder.HasKey(e => e.Id).HasName("Id");
            customerBuilder.Property(e => e.BirthDate).HasColumnName("BirthDate").HasColumnType("DATETIME2(7)");
            customerBuilder.Property(e => e.FirstName).HasColumnName("FirstName").HasColumnType("NVARCHAR(20)");
            customerBuilder.Property(e => e.LastName).HasColumnName("LastName").HasColumnType("NVARCHAR(20)");
            customerBuilder.Property(e => e.RowVersion).HasColumnName("RowVersion").HasColumnType("TIMESTAMP");
        }

        private void CreateReservationTable(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(
                new ReservationTypConfig()
            );
            var reservationBuilder = modelBuilder.Entity<Reservation>();
            reservationBuilder.HasKey(e => e.ReservationNr).HasName("Id");
            reservationBuilder.Property(e => e.From).HasColumnName("FromDate").HasColumnType("DATETIME2(7)");
            reservationBuilder.Property(e => e.To).HasColumnName("ToDate").HasColumnType("DATETIME2(7)");
            reservationBuilder.HasOne(e => e.Customer).WithMany(c => c.Reservations).HasForeignKey(e => e.CustomerId)
                .HasConstraintName("FK_Reservation_CustomerId");
            reservationBuilder.HasOne(e => e.Car).WithMany(c => c.Reservations).HasForeignKey(e => e.CarId)
                .HasConstraintName("FK_Reservation_CarId");
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