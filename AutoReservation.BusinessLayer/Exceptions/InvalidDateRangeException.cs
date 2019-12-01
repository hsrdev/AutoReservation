using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message) : base(message) { }
    }
}