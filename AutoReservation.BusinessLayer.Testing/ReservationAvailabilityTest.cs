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
        }

        [Fact]
        public async Task ScenarioOkay03Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2-- - |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioOkay04Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //                | ---Date 1--- |
            //| ---Date 2--- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioNotOkay01Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| ---Date 1--- |
            //    | ---Date 2--- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioNotOkay02Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //    | ---Date 1--- |
            //| ---Date 2--- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioNotOkay03Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| ---Date 1--- |
            //| --------Date 2-------- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioNotOkay04Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| --------Date 1-------- |
            //| ---Date 2--- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioNotOkay05Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| ---Date 1--- |
            //| ---Date 2--- |
            // act
            // assert
        }
    }
}
