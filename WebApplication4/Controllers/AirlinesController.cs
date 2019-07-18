using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication4.Models;
using Sugar.Enties;
using SqlSugar;

namespace WebApplication4.Controllers
{


    public class AirlinesController : Controller
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
            DbType = DbType.Oracle,//设置数据库类型
            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
            InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
        });

        //
        // GET: Airlines
        public ActionResult Airlines()
        {
            return View();
        }
        public ActionResult ChangeFlight()
        {
            return View();
        }
        public ActionResult AddPlane()
        { 
            return View();
        }
        public ActionResult FlightInformation()
        {
            List<TFlightTime> Flights = new List<TFlightTime>();
            //TFlightTime obj = new TFlightTime();
            //SelectReturn sr = DataBaseAccess.GetAllTInfo(obj);     
            //List<object> list = sr.list;//object
            //List<string> value = sr.value;//值

            Actions a = new Actions();
            List<FLIGHTIME> flightTimes = a.GetFlightTime("东方航空公司");   
            foreach (FLIGHTIME item in flightTimes)
            {
                TFlightTime flight = new TFlightTime();
                flight.f_id = item.F_ID;
                flight.t_airport = item.T_AIRPORT;
                flight.t_terminal = item.T_TERMINAL;
                flight.ex_takeoff_time = item.EX_TAKEOFF_TIME.ToString();
                flight.ex_land_time = item.EX_LAND_TIME.ToString();
                flight.re_takeoff_time = null;
                flight.re_land_time = null;
                flight.l_airport = item.L_AIRPORT;
                flight.l_termnal = item.L_TERMNAL;
                Flights.Add(flight);
            }
            return View("FlightInformation", Flights);
        }
        public ActionResult query()
        {
            return View();
        }
        public ActionResult ReceiveMsg1()
        {
            //新建航班，多表插入
            string id = Request.Params["id"];
            string pid = Request.Params["pid"];
            string t_airport = Request.Params["t_airport"];
            string t_terminal = Request.Params["t_terminal"];
            string t_time = Request.Params["t_time"];
            string l_airport = Request.Params["l_airport"];
            string l_terminal = Request.Params["l_terminal"];
            string l_time = Request.Params["l_time"];
            Actions a = new Actions();
            PLANE b = a.GetPLANE(pid);
            FLIGHT obj2 = new FLIGHT();
            obj2.F_ID = id;
            obj2.P_ID = pid;
            obj2.CAPACITY = b.CAPACITY;
            obj2.RESERVE = 0;
            obj2.CHECKED = 0;
            obj2.VALUE = 500;
            obj2.LUGGAGE = 50;
            obj2.BOARD = 0;
            FLIGHTIME obj1 = new FLIGHTIME();
            obj1.F_ID = id;
            obj1.T_AIRPORT = t_airport;
            obj1.T_TERMINAL = t_terminal;
            obj1.L_AIRPORT = l_airport;
            obj1.L_TERMNAL = l_terminal;

            obj1.EX_TAKEOFF_TIME = a.SystemTime.AddHours(10);
            obj1.EX_LAND_TIME = a.SystemTime.AddHours(12);
            obj1.RE_TAKEOFF_TIME = null;
            obj1.RE_LAND_TIME = null;

            string d_ID = id;       //降落飞机ID
            char[] str = d_ID.ToCharArray();
            string d_ID_re;   //返回航班ID
            int temp = int.Parse(str[5].ToString());
            if (temp % 2 == 0)
            {
                d_ID_re = d_ID.Substring(0, d_ID.Length - 1) + (temp + 1).ToString();
            }
            else
            {
                d_ID_re = d_ID.Substring(0, d_ID.Length - 1) + (temp - 1).ToString();
            }



            FLIGHTIME obj3 = new FLIGHTIME();
            obj3.F_ID = d_ID_re;
            obj3.T_AIRPORT = l_airport;
            obj3.T_TERMINAL = l_terminal;
            obj3.L_AIRPORT = t_airport;
            obj3.L_TERMNAL = t_terminal;

            obj3.EX_TAKEOFF_TIME = a.SystemTime.AddHours(14);
            obj3.EX_LAND_TIME = a.SystemTime.AddHours(16);
            obj3.RE_TAKEOFF_TIME = null;
            obj3.RE_LAND_TIME = null;

            db.Insertable(obj1).ExecuteCommand();
            db.Insertable(obj2).ExecuteCommand();
            db.Insertable(obj3).ExecuteCommand();
            //DataBaseAccess.insertObj(obj1);
            //DataBaseAccess.insertObj(obj2);
            //DataBaseAccess.insertObj(obj3);
            return View("Airlines");
        }



public ActionResult ReceiveMsg2_1()
        {
            string id = Request.Params["fid"];
            TFlightTime result = new SearchFlight().Search(id);
            List<string> list = new List<string>();
            if (result.f_id != null)
            {
                list.Add(result.f_id);
                list.Add(result.t_airport);
                list.Add(result.ex_takeoff_time);
                list.Add(result.t_terminal);
                list.Add(result.l_airport);
                list.Add(result.ex_land_time);
                list.Add(result.l_termnal);
            }
            else
                list.Add("-1");
            return View("ChangeFlight", list);
        }

        public ActionResult ReceiveMsg2_2()
        {
            //获取当前飞机ID

            return View("Airlines");
        }


        public ActionResult ReceiveMsg3()
        {
            //获取当前飞机ID
            string ID = Request.Params["id"];
            string type = Request.Params["type"];
            string capacity = Request.Params["capacity"];
            string company = "东方航空公司";
            PLANE obj = new PLANE();
            obj.ID = ID;
            obj.PATTERN = type;
            obj.CAPACITY = Convert.ToInt16(int.Parse(capacity));
            obj.COMPANY = company;
            //DataBaseAccess.insertObj(obj);

            db.Insertable(obj).ExecuteCommand();
            return View("AddPlane");
        }
    }
}