using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Luggage")]
    public class Luggage
    {
        [DBPrimaryKey("F_ID")]
        [DBMember("F_ID")]
        [Display(Name = "航班ID")]
        public string F_ID { get; set; }

        [DBPrimaryKey("L_ID")]
        [DBMember("L_ID")]
        [Display(Name = "行李ID")]
        public int L_ID { get; set; }

        [DBMember("Weght")]
        [Display(Name = "重量")]
        public long Weght { get; set; }

        [DBMember("State")]
        [Display(Name = "行李状态")]
        public string State { get; set; }

        public Luggage() { }

        public Luggage(string f_id, int l_id, long w, string s)
        {
            F_ID = f_id;
            L_ID = l_id;
            Weght = w;
            State = s;
        }
    }
}