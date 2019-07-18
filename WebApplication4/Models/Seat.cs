using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Seat")]
    public class Seat
    {
        [DBPrimaryKey("F_ID")]
        [DBMember("F_ID")]
        [Display(Name = "航班ID")]
        public string F_ID { get; set; }

        [DBMember("Seat_Number")]
        [Display(Name = "座位号")]
        public string Seat_Number { get; set; }

        [DBMember("C_ID")]
        [Display(Name = "客户ID")]
        public int C_ID { get; set; }

        [DBMember("State")]
        [Display(Name = "座位状态")]
        public string State { get; set; }

        public Seat(string fid)
        {
            F_ID = fid;
        }
        public Seat(string fd, string sn, int cd, string ss)
        {
            F_ID = fd;
            Seat_Number = sn;
            C_ID = cd;
            State = ss;
        }
        public Seat() { }
        public Seat(string fd,string ss)
        {
            F_ID = fd;
            State = ss;
        }
    }
}