using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class CarUnavailableException<T> : Exception
    {
        public CarUnavailableException(string message) : base(message) { }
    }
}
