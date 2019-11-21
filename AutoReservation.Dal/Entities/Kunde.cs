using System;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public int ReservationNr { get; set; }
        //ForeignKey
        public int CarId { get; set; }
        //ForeignKey
        public int CustomerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime RowVersion { get; set; }
    }
}