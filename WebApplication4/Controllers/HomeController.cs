using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication4.Models;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        // GET: MainHome1
        public ActionResult Home()
        {
            ShowUsers();
            return View();
        }

        public ActionResult ShowUsers()
        {
            User obj = new User();
            SelectReturn sr = DataBaseAccess.GetAllTInfo(obj);
            List<object> list = sr.list;

            JsonSerializer jsonlist = new JsonSerializer();
            StringWriter sw = new StringWriter();
            jsonlist.Serialize(new JsonTextWriter(sw), list);
            string result = sw.GetStringBuilder().ToString();             //list转化为JSON

            return Content(result);
        }

        public ActionResult DeleteUser()
        {
            int id = int.Parse(Request.Params["result"]);
            User obj = new User(id);
            DataBaseAccess.deleteObj(obj);
            return View("Home");
        }

        //此动作提供一个添加页面
        public ActionResult AddUserPage()
        {
            return View("AddUser");
        }
        //在这里执行添加结果后返回Home

        public ActionResult AddUser()
        {
            return View();
        }

        public ActionResult Search(string id)
        {
            //select* from people where sid like '%C%';
            string sid = Request.Params["SearchContent"];
            

            return View();
        }
    }
}