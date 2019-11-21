using System;

namespace AutoReservation.Dal.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public int DalilyRate { get; set; }
        public DateTime RowVersion { get; set; }
        public int CarClass { get; set; }
        public int BaseRate { get; set; }
    }
}