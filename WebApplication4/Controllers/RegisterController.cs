using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using System.Windows.Forms;
using System.IO;


namespace WebApplication4.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Check()
        {
            string username = Request.Params["username"];
            User objcheck = new User()
            {
                Account = username,
                Pwd = ""
            };
            objcheck = new RegisterService().UserCheck(objcheck);
      //    bool res = DataBaseAccess.existObj(objcheck);
            if (objcheck != null)
            {
                return Content("No");
            }
            else
            {
                return Content("Yes");
            }
        }
        /*
        public string SignUpHandle()
        {
            string id = Request.Params["RegisterId"];
            string pwd = Request.Params["Registerpwd"];//接受form提交的数据
            //people tem = new people(p["ID"], p["password"]);
            User a = new User
            {
                sid = id,
                skey = pwd,
                rem_check = 0
            };
            
            if (DataBaseAccess.insertObj(a))
            {
                return "SignUp Successful!";
            }
            else
                return "SignUp failed!";

        }
        */
        private string getID()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str.Substring(0, 15);
        }
    }
}