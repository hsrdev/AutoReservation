using System;
using System.Collections.Generic;

namespace AutoReservation.Dal.Entities
{
    public class Customer
    {
        //PrimaryKey
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RowVersion { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}