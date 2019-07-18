using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("FLIGHTIME")]
    public class TFlightTime
    {
        [DBPrimaryKey("f_id")]
        [DBMember("f_id")]
        [Display(Name = "航班ID")]
        public string f_id { get; set; }

        [DBMember("t_airport")]
        [Display(Name = "当前机场")]
        public string t_airport { get; set; }

        [DBMember("t_terminal")]
        [Display(Name = "当前航站楼")]
        public string t_terminal { get; set; }

        [DBMember("ex_takeoff_time")]
        [Display(Name = "预计起飞时间")]
        public string ex_takeoff_time { get; set; }

        [DBMember("ex_land_time")]
        [Display(Name = "预计到达时间")]
        public string ex_land_time { get; set; }

        [DBMember("re_takeoff_time")]
        [Display(Name = "实际起飞时间")]
        public string re_takeoff_time { get; set; }

        [DBMember("re_land_time")]
        [Display(Name = "实际到达时间")]
        public string re_land_time { get; set; }

        [DBMember("l_airport")]
        [Display(Name = "目的机场")]
        public string l_airport { get; set; }

        [DBMember("l_termnal")]
        [Display(Name = "目的航站楼")]
        public string l_termnal { get; set; }




        public TFlightTime(string id, string ta, string tt, string ett, string elt, 
            string rtt, string rlt, string la, string lt)
        {
            f_id = id;
            t_airport = ta;
            t_terminal = tt;
            ex_takeoff_time = ett;
            ex_land_time = elt;
            re_takeoff_time= rtt;
            re_land_time = rlt;
            l_airport = la;
            l_termnal = lt;
        }
        public TFlightTime(string id) {
            f_id = id;
        }
        public TFlightTime() { }
    }
}