using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Company")]
    public class Company
    {
        [DBPrimaryKey("ID")]
        [DBMember("ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [DBMember("Account")]
        [Display(Name = "账号")]
        public string Account { get; set; }

        [DBMember("Pwd")]
        [Display(Name = "密码")]
        public string Pwd { get; set; }

        [DBMember("Power")]
        [Display(Name = "权限")]
        public string Power { get; set; }

        public Company(int id, string account, string pwd, string power)
        {
            ID = id;
            Account = account;
            Pwd = pwd;
            Power = power;
        }
        public Company() { }
        public Company(int id)
        {
            ID = id;
        }
    }
}