using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class FlightStateReturn
    {
        public TFlightTime tf;
        public string State;

        public FlightStateReturn(TFlightTime flightTime,string state)
        {
            tf = flightTime;
            State = state;
        }
    }
}