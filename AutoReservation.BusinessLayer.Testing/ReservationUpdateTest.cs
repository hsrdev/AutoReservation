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

        public ReservationUpdateTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task InsertCustomerTest()
        {
            // arrange
            Reservation = new Reservation
            {
                CarId = 1,
                CustomerId = 1,
                From = new DateTime(2019, 12,1),
                To = new DateTime(2019, 12,3)
            };
            // act
            await _target.Insert(Reservation);
            var insertedReservation = await _target.Get(5);
            // assert
            Assert.Equal(Reservation.CarId, insertedReservation.CarId);
        }

        [Fact]
        public async Task UpdateCustomerTest()
        {
            Reservation = await _target.Get(2);
            Reservation.To = new DateTime(2019, 12, 1);
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
        public async Task DeleteCustomerTest()
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
