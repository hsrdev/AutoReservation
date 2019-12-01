using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class CarUnavailableException : Exception
    {
        public CarUnavailableException(string message) : base(message) { }
    }
}
