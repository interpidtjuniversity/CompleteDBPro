using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using System.Windows.Forms;
using System.Text;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        /*
        public ActionResult Home()
        {
            //[1] 获取数据
            string loginId = Request.Params["LoginId"];
            string loginpwd = Request.Params["Loginpwd"];//接受form提交的数据
            User objAdmin = new User()
            {
                sid = loginId,
                skey = loginpwd
            };//对象初始化器（对属性赋值）
            //[2] 业务处理  调用数据访问类   使用数据访问类中的方法
            objAdmin = new LoginService().AdminLogin(objAdmin);
            if (objAdmin != null)
            {
                ViewData["info"] = "欢迎登录！" + objAdmin.sid;
                return Redirect("~/Home/Home");
            }
            else
            {
                ViewData["info"] = "用户名或密码错误";
                return View("Login");
            }
        }
        */
        public ActionResult CheckGroundWorkerLogin()                  //账号密码暂时存入文件
        {
            string loginId = Request.Params["LoginId"];
            string loginpwd = Request.Params["Loginpwd"];//接受form提交的数据
            User objAdmin = new User()
            {
                Account = loginId,
                Pwd = loginpwd
            };//对象初始化器（对属性赋值）
            //[2] 业务处理  调用数据访问类   使用数据访问类中的方法
            objAdmin = new LoginService().AdminLogin(objAdmin);
            if (objAdmin != null)
            {
                /*******************/
                //账号密码存入文件
                string[] info = new string[2];
                info[0] = objAdmin.Account;
                info[1] = objAdmin.Pwd;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"F:\vs2017python\asp DBProject\WebApplication4\Info.txt", false)) 
                {
                    foreach (string str in info)
                    {
                        file.WriteLine(str);
                    }
                }

                //
                ViewData["info"] = "欢迎登录！" + objAdmin.ID;
                Workers worker = new Workers(objAdmin.ID);
                List<string> needs = new List<string>();    //查询的数据需求,此处时座位号
                needs.Add("Name");
                needs.Add("Phone");
                needs.Add("Kind");
                List<string> res = new List<string>();     //查询结果,GroundWorkerHome 用到的查询结果
                res = DataBaseAccess.GetSingleInfo(worker, needs);
                res.Add(objAdmin.ID.ToString());
                return View("GroundWorkerHome", res);
            }
            else
            {
                ViewData["info"] = "用户名或密码错误";
                return View("Login");
            }
        }


        public ActionResult CheckCompanyLogin()
        {
            string loginId = Request.Params["LoginId"];
            string loginpwd = Request.Params["Loginpwd"];//接受form提交的数据
            Company objAdmin = new Company()
            {
                Account = loginId,
                Pwd = loginpwd
            };//对象初始化器（对属性赋值）
            //[2] 业务处理  调用数据访问类   使用数据访问类中的方法
            objAdmin = new LoginService().AdminLogin(objAdmin);
            if (objAdmin != null)
            {
                return Redirect("~/Airlines/Airlines");
                //return View("Airlines");
            }
            else
            {
                ViewData["info"] = "用户名或密码错误";
                return View("Login");
            }
        }

        public ActionResult ReturnToPersonMain()
        {
            string[] data = System.IO.File.ReadAllLines(@"F:\vs2017python\asp DBProject\WebApplication4\Info.txt");
            string ac = data[0];
            string pwd = data[1];

            User objAdmin = new User()
            {
                Account = ac,
                Pwd = pwd
            };

            objAdmin = new LoginService().AdminLogin(objAdmin);

            Workers worker = new Workers(objAdmin.ID);
            List<string> needs = new List<string>();    //查询的数据需求,此处时座位号
            needs.Add("Name");
            needs.Add("Phone");
            needs.Add("Kind");
            List<string> res = new List<string>();     //查询结果,GroundWorkerHome 用到的查询结果
            res = DataBaseAccess.GetSingleInfo(worker, needs);
            res.Add(objAdmin.ID.ToString());
            return View("GroundWorkerHome", res);
        }
    }
}