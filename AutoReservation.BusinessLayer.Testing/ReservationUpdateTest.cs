using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationUpdateTest
        : TestBase
    {
        private readonly ReservationManager _target;
        public Reservation Reservation { get; set; }
        public int Year = DateTime.Now.Year + 1;

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task GetReservationWithCarAndCustomer()
        {
            // arrange & act
            Reservation = await _target.Get(1);
            // assert
            Assert.Equal(1, Reservation.CarId);
            Assert.NotNull(Reservation.Car);
            Assert.NotNull(Reservation.Customer);
        }

        [Fact]
        public async Task UpdateReservationTest()
        {
            Reservation = await _target.Get(2);
            Reservation.To = new DateTime(Year, 12, 01);
            await _target.Update(Reservation);
            var updatedReservation = await _target.Get(2);
            Assert.Equal(Reservation.To, updatedReservation.To);
        }

        [Fact]
        public async Task NotExistingAccessTest()
        {
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(5)
            );
           
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            // arrange
            Reservation = await _target.Get(1);
            // act
            await _target.Delete(Reservation);
            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _target.Get(Reservation.ReservationNr)
            );


        }

        [Fact]
        public async Task InsertreservationWithCarAndCustomerTest()
        {
            var carManager = new CarManager();
            var customerManager = new CustomerManager();
            Reservation = new Reservation
            {
                Car = await carManager.Get(1),
                Customer = await customerManager.Get(1),
                From = new DateTime(Year, 12, 01),
                To = new DateTime(Year, 12, 03)
            };

            var insertedReservation = await _target.Insert(Reservation);

            Assert.Equal(Reservation.From, insertedReservation.From);
            Assert.NotNull(insertedReservation.Car);
            Assert.NotNull(insertedReservation.Customer);
            Assert.Equal(Reservation.CustomerId, insertedReservation.CustomerId);
            Assert.Equal(Reservation.CarId, insertedReservation.CarId);
        }
    }
}