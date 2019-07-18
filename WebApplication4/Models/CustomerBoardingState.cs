using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class CustomerBoardingState
    {
        public string Flight_ID;
        public int? Customer_ID;
        public string gender;
        public string name;
        public long? phone;
        public string state;
        public string seat;

        public CustomerBoardingState(int b, string c, string d, long e, string fid, string f,string s)
        {
            Customer_ID = b;
            name = c;
            gender = d;
            phone = e;
            Flight_ID = fid;
            state = f;
            seat = s;

        }

        public CustomerBoardingState() { }
    }
}