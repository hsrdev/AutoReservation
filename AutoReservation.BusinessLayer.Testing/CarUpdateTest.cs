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

        public AutoUpdateTests()
        {
            _target = new AutoManager();
        }

        [Fact]
        public async Task InsertCarTest()
        {
            Car = new LuxuryClassCar()
            {
                Make = "Volvo",
                DailyRate = 100
            };
            await _target.Insert(Car);
            var insertedCar = await _target.Get(Car.Id);
            Assert.Equal(Car.Id, insertedCar.Id);
        }

        [Fact]
        public async Task UpdateCarTest()
        {
            Car = await _target.Get(5);
            Car.DailyRate = 500;
            await _target.Update(Car);
            var updatedCar = await _target.Get(Car.Id);
            Assert.Equal(Car.DailyRate, updatedCar.DailyRate);
        }
    }
}
