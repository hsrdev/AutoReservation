using System;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;

namespace AutoReservation.TestEnvironment
{
    public abstract class TestDataHelper
    {
        protected readonly CarReservationContext Context;
        protected const string InitializationError = "Error while re-initializing database entries.";

        protected TestDataHelper(CarReservationContext context)
        {
            Context = context;
        }

        public virtual void InitializeTestData()
        {
            try
            {
                PrepareDatabase();
                SeedTestData();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(InitializationError, ex);
            }
        }

        protected abstract void PrepareDatabase();

        private void SeedTestData()
        {
            Car[] cars = {
                new StandardCar {Make = "Fiat Punto", DailyRate = 50},
                new MidClassCar() {Make = "VW Golf", DailyRate = 120},
                new LuxuryClassCar() {Make = "Audi S6", DailyRate = 180, BaseRate = 50},
                new StandardCar() {Make = "Fiat 500", DailyRate = 75},
            };

            Customer[] customers = {
                new Customer {LastName = "Nass", FirstName = "Anna", BirthDate = new DateTime(1981, 05, 05)},
                new Customer {LastName = "Beil", FirstName = "Timo", BirthDate = new DateTime(1980, 09, 09)},
                new Customer {LastName = "Pfahl", FirstName = "Martha", BirthDate = new DateTime(1990, 07, 03)},
                new Customer {LastName = "Zufall", FirstName = "Rainer", BirthDate = new DateTime(1954, 11, 11)},
            };

            int year = DateTime.Now.Year + 1;
            Reservation[] reservations = {
                new Reservation {Car = cars[0], Customer = customers[0], From = new DateTime(year, 01, 10), To = new DateTime(year, 01, 20)},
                new Reservation {Car = cars[1], Customer = customers[1], From = new DateTime(year, 01, 10), To = new DateTime(year, 01, 20)},
                new Reservation {Car = cars[2], Customer = customers[2], From = new DateTime(year, 01, 10), To = new DateTime(year, 01, 20)},
                new Reservation {Car = cars[1], Customer = customers[0], From = new DateTime(year, 05, 19), To = new DateTime(year, 06, 19)},
            };

            Context.Cars.AddRange(cars);
            Context.Customers.AddRange(customers);
            Context.Reservations.AddRange(reservations);

            Context.SaveChanges();
        }
    }
}