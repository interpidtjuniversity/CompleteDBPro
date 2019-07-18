using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{

    [DBTable("Users")]
    public class User
    {
        [DBPrimaryKey("ID")]
        [DBMember("ID")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [DBMember("Account")]
        [Display(Name ="账号")]
        public string Account { get; set; }

        [DBMember("Pwd")]
        [Display(Name ="密码")]
        public string Pwd { get; set; }

        [DBMember("Power")]
        [Display(Name = "工作类型")]
        public string Power { get; set; }

        public User(int id, string account, string pwd, string power)
        {
            ID = id;
            Account = account;
            Pwd = pwd;
            Power = power;
        }
        public User() { }
        public User(int id)
        {
            ID = id;
        }
    }
}