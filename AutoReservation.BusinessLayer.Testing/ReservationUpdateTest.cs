using System;
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
        public async Task InsertReservationTest()
        {
            // arrange
            Reservation = new Reservation
            {
                CarId = 1,
                CustomerId = 1,
                From = new DateTime(Year, 12, 01),
                To = new DateTime(Year, 12, 03)
            };
            // act
            var insertedReservation = await _target.Insert(Reservation);
            // assert
            Assert.Equal(Reservation.CarId, insertedReservation.CarId);
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
            try
            {
                // arrange & act
                Reservation = await _target.Get(5);
            }
            catch (Exception e)
            {
                // assert
                Assert.Equal("Sequence contains no elements", e.Message);
            }
        }

        [Fact]
        public async Task DeleteReservationTest()
        {
            try
            {
                // arrange
                Reservation = await _target.Get(1);
                // act
                await _target.Delete(Reservation);
                var deletedReservation = await _target.Get(Reservation.ReservationNr);
            }
            catch (Exception e)
            {
                // assert
                Assert.Equal("Sequence contains no elements", e.Message);
            }
        }
    }
}