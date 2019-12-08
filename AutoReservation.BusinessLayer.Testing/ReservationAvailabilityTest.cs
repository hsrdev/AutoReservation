    using System;
using System.Threading.Tasks;
    using AutoReservation.Dal.Entities;
    using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;
        public Reservation Reservation { get; set; }
        public int Year = DateTime.Now.Year + 1;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01,20),
                To = new DateTime(Year, 01, 30)
            };
            // act & assert
            Assert.True(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 2,
                From = new DateTime(Year, 03, 01),
                To = new DateTime(Year, 03, 10)
            };
            // act & assert
            Assert.True(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            // arrange
            //               | ---Date 1--- |
            //| ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 01),
                To = new DateTime(Year, 01, 10)
            };
            // act & assert
            Assert.True(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 2,
                From = new DateTime(Year, 01, 01),
                To = new DateTime(Year, 01, 08)
            };
            // act & assert
            Assert.True(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 15),
                To = new DateTime(Year, 01, 30)
            };
            // act & assert
            Assert.False(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 01),
                To = new DateTime(Year, 01, 15)
            };
            // act & assert
            Assert.False(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 10),
                To = new DateTime(Year, 01, 30)
            };
            // act & assert
            Assert.False(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 10),
                To = new DateTime(Year, 01, 15)
            };
            // act & assert
            Assert.False(await _target.AvailabilityCheck(Reservation));
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            Reservation = new Reservation
            {
                CarId = 1,
                From = new DateTime(Year, 01, 10),
                To = new DateTime(Year, 01, 20)
            };
            // act & assert
            Assert.False(await _target.AvailabilityCheck(Reservation));
        }
    }
}
