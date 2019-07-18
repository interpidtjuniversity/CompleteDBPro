using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Flight")]
    public class Flight
    {
        [DBPrimaryKey("F_ID")]
        [DBMember("F_ID")]
        [Display(Name = "航班ID")]
        public string F_ID { get; set; }

        [DBMember("P_ID")]
        [Display(Name = "飞机ID")]
        public string P_ID { get; set; }

        [DBMember("Capacity")]
        [Display(Name = "最大容量")]
        public int Capacity { get; set; }

        [DBMember("Reserve")]
        [Display(Name = "预订人数")]
        public int Reserve { get; set; }

        [DBMember("Checked")]
        [Display(Name = "过安检人数")]
        public int Checked { get; set; }

        [DBMember("Board")]
        [Display(Name = "登机时人数")]
        public int Board { get; set; }

        [DBMember("Value")]
        [Display(Name = "价格")]
        public int Value { get; set; }

        [DBMember("Luggage")]
        [Display(Name = "最大行李重量")]
        public int Luggage { get; set; }


        public Flight(string fid,string pid,int capacity,int rn,int psin,int bn,int price,int mbw)
        {
            F_ID = fid;
            P_ID = pid;
            Capacity = capacity;
            Reserve = rn;
            Checked = psin;
            Board = bn;
            Value = price;
            Luggage = mbw;
        }
        public Flight(string id) {
            F_ID = id;
        }
        public Flight()
        { }
    }
}