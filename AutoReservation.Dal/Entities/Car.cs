using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Car
    {
        public Car(string make, int dailyRate, Reservation reservation)
        {
            Make = make;
            DailyRate = dailyRate;
            Reservations.Add(reservation);
        }

        //PrimaryKey
        public int Id { get; set; }
        public string Make { get; set; }
        public int DailyRate { get; set; }
        public byte[] RowVersion { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}