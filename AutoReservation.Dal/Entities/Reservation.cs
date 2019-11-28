using System;

namespace AutoReservation.Dal.Entities
{
    public class Reservation
    {
        public Reservation(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        //PrimaryKey
        public int ReservationNr { get; set; }
        //ForeignKey
        public int CarId { get; set; }
        //ForeignKey
        public int CustomerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte[] RowVersion { get; set; }
        public Car Car { get; set; }
        public Customer Customer { get; set; }
    }
}