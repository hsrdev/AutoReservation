using System;
using System.Collections;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Car
    {
        //PrimaryKey
        public int Id { get; set; }
        public string Make { get; set; }
        public int DailyRate { get; set; }
        public DateTime RowVersion { get; set; }
        public int CarClass { get; set; }
        public int BaseRate { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}