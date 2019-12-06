using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationDateRangeTest
        : TestBase
    {
        private readonly ReservationManager _target;
        public Reservation Reservation { get; set; }

        public ReservationDateRangeTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01TestAsync()
        {
            // arrange
            Reservation = new Reservation
            {
                CarId = 1,
                CustomerId = 1,
                From = new DateTime(2019, 12, 1),
                To = new DateTime(2019, 12, 3)
            };
            // act
            var insertedReservation = await _target.Insert(Reservation);
            // assert
            Assert.True(_target.DateRangeCheck(Reservation));
            Assert.Equal(Reservation.CarId, insertedReservation.CarId);
        }

        [Fact]
        public void ScenarioOkay02Test()
        {
            // arrange
            Reservation = new Reservation
            {
                From = new DateTime(2019, 11, 1,12, 0, 0),
                To = new DateTime(2019, 12, 1, 15, 0, 0)
            };
            // act & assert
            Assert.True(_target.DateRangeCheck(Reservation));
        }

        [Fact]
        public void ScenarioNotOkay01Test()
        {
            // arrange
            Reservation = new Reservation
            {
                From = new DateTime(2019, 12, 15, 12 , 0, 0),
                To = new DateTime(2019, 12, 16, 8, 0, 0)
            };
            // act & assert
            Assert.False(_target.DateRangeCheck(Reservation));
        }

        [Fact]
        public void ScenarioNotOkay02Test()
        {
            // arrange
            Reservation = new Reservation
            {
                From = new DateTime(2019, 12,16),
                To = new DateTime(2019, 12, 13)
            };
            // act & assert
            Assert.False(_target.DateRangeCheck(Reservation));
        }

        [Fact]
        public void ScenarioNotOkay03Test()
        {
            // arrange
            Reservation = new Reservation
            {
                From = new DateTime(2019, 12, 16),
                To = new DateTime(2019, 12, 16)
            };
            // act & assert
            Assert.False(_target.DateRangeCheck(Reservation));
        }
    }
}