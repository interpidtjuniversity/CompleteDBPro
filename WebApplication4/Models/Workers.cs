using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Workers")]
    public class Workers
    {
        [DBPrimaryKey("ID")]
        [DBMember("ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [DBMember("Name")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [DBMember("Gend")]
        [Display(Name = "性别")]
        public string Gend { get; set; }

        [DBMember("City")]
        [Display(Name = "城市")]
        public string City { get; set; }

        [DBMember("Phone")]
        [Display(Name = "电话号码")]
        public int Phone { get; set; }

        [DBMember("Kind")]
        [Display(Name = "职务")]
        public string Kind { get; set; }

        public Workers(int id)
        {
            ID = id;
        }
    }
}