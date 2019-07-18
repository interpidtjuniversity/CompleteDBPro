using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
   [DBTable("Fstate")]
    public class Fstate
    {
        [DBPrimaryKey("F_ID")]
        [DBMember("F_ID")]
        [Display(Name = "航班ID")]
        public string F_ID { get; set; }

        [DBMember("State")]
        [Display(Name = "航班状态")]
        public string State { get; set; }

        public Fstate() { }
        public Fstate(string a, string b)
        {
            F_ID = a;
            State = b;

        }
    }
}