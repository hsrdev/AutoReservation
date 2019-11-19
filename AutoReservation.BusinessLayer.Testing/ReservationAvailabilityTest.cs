using System;
using System.Threading.Tasks;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class ReservationAvailabilityTest
        : TestBase
    {
        private readonly ReservationManager _target;

        public ReservationAvailabilityTest()
        {
            _target = new ReservationManager();
        }

        [Fact]
        public async Task ScenarioOkay01Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| ---Date 1--- |
            //               | ---Date 2--- |
            // act
            // assert
        }

        [Fact]
        public async Task ScenarioOkay02Test()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            //| ---Date 1--- |
            //                 | ---Date 2--- |
            // act
            // assert
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
