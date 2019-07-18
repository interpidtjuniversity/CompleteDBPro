using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    [DBTable("Customer")]
    public class Customer
    {
        [DBPrimaryKey("ID")]
        [DBMember("ID")]
        [Display(Name = "客户ID")]
        public int ID { get; set; }

        [DBMember("Gender")]
        [Display(Name = "性别")]
        public string Gender { get; set; }

        [DBMember("Name")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [DBMember("City")]
        [Display(Name = "居住城市")]
        public string City { get; set; }

        [DBMember("Phone_Number")]
        [Display(Name = "联系方式")]
        public string Phone_Number { get; set; }

        [DBMember("Lug_Weight")]
        [Display(Name = "联系方式")]
        public int Lug_Weight { get; set; }

        public Customer(int id)
        {
            ID = id;
        }

        public Customer(int cd, string sex, string name, string city, string pr,int w)
        {
            ID = cd;
            Gender = sex;
            Name = name;
            City = city;
            Phone_Number = pr;
            Lug_Weight = w;

        }
    }
}