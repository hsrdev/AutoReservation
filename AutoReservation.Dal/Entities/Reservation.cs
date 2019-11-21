using System;

namespace AutoReservation.Dal.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RowVersion { get; set; }
    }
}