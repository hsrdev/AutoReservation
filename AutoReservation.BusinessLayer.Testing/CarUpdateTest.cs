using System;
using System.Threading.Tasks;
using AutoReservation.Dal.Entities;
using AutoReservation.TestEnvironment;
using Xunit;

namespace AutoReservation.BusinessLayer.Testing
{
    public class AutoUpdateTests
        : TestBase
    {
        private readonly AutoManager _target;
        public Car Car { get; set; }
        public Reservation Reservation { get; set; }

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            throw new NotImplementedException("Test not implemented.");
            // arrange
            // act
            // assert
        }
    }
}
