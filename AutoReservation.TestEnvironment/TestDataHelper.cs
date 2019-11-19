using System;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;

namespace AutoReservation.TestEnvironment
{
    public abstract class TestDataHelper
    {
        protected readonly AutoReservationContext Context;
        protected const string InitializationError = "Error while re-initializing database entries.";

        protected TestDataHelper(AutoReservationContext context)
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
            Auto[] autos = {
                new StandardAuto {Marke = "Fiat Punto", Tagestarif = 50},
                new MittelklasseAuto {Marke = "VW Golf", Tagestarif = 120},
                new LuxusklasseAuto {Marke = "Audi S6", Tagestarif = 180, Basistarif = 50},
                new StandardAuto {Marke = "Fiat 500", Tagestarif = 75},
            };

            Kunde[] kunden = {
                new Kunde {Nachname = "Nass", Vorname = "Anna", Geburtsdatum = new DateTime(1981, 05, 05)},
                new Kunde {Nachname = "Beil", Vorname = "Timo", Geburtsdatum = new DateTime(1980, 09, 09)},
                new Kunde {Nachname = "Pfahl", Vorname = "Martha", Geburtsdatum = new DateTime(1990, 07, 03)},
                new Kunde {Nachname = "Zufall", Vorname = "Rainer", Geburtsdatum = new DateTime(1954, 11, 11)},
            };

            int year = DateTime.Now.Year + 1;
            Reservation[] reservationen = {
                new Reservation {Auto = autos[0], Kunde = kunden[0], Von = new DateTime(year, 01, 10), Bis = new DateTime(year, 01, 20)},
                new Reservation {Auto = autos[1], Kunde = kunden[1], Von = new DateTime(year, 01, 10), Bis = new DateTime(year, 01, 20)},
                new Reservation {Auto = autos[2], Kunde = kunden[2], Von = new DateTime(year, 01, 10), Bis = new DateTime(year, 01, 20)},
                new Reservation {Auto = autos[1], Kunde = kunden[0], Von = new DateTime(year, 05, 19), Bis = new DateTime(year, 06, 19)},
            };

            Context.Autos.AddRange(autos);
            Context.Kunden.AddRange(kunden);
            Context.Reservationen.AddRange(reservationen);

            Context.SaveChanges();
        }
    }
}