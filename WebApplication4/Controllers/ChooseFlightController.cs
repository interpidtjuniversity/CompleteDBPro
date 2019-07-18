using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication4.Models;
using SqlSugar;
using Sugar.Enties;

namespace WebApplication4.Controllers
{
    public class ChooseFlightController : Controller
    {

        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
            DbType = DbType.Oracle,//设置数据库类型
            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
            InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
        });
        /*  */
        private static List<Flight> Flights;
        // GET: ChooseSeat
        public ActionResult ChooseFlight()
        {
            ShowFlight();
            return View();
        }

        public ActionResult ShowFlight()
        {
            Flights = new List<Flight>();
            Flight obj = new Flight();
            SelectReturn sr = DataBaseAccess.GetAllTInfo(obj);
            List<object> list = sr.list;//object
            List<string> value = sr.value;//值
            //object转Flight
            int flag = 0;   //游标
            foreach (object j in list)
            {
                Flight flight = new Flight();//current
                //手动赋值,暂时没有找到映射的方法
                flight.F_ID = value[flag++];
                flight.P_ID = value[flag++];
                flight.Capacity = int.Parse(value[flag++]);
                flight.Reserve = int.Parse(value[flag++]);
                flight.Checked = int.Parse(value[flag++]);
                flight.Board = int.Parse(value[flag++]);
                flight.Value = int.Parse(value[flag++]);
                flight.Luggage = int.Parse(value[flag++]);
                Flights.Add(flight);
                /*
                JsonSerializer jsonlist = new JsonSerializer();
                StringWriter sw = new StringWriter();
                jsonlist.Serialize(new JsonTextWriter(sw), j);
                string result = sw.GetStringBuilder().ToString();
                MessageBox.Show(result[0].ToString());
                */
            }
            return View("ChooseFlight", Flights);
        }

        public ActionResult GetFlightInfo(string id,int capacity)
        {
            //需要在此函数中传回一个数组,为了方便先传递一个list
            Seat obj = new Seat(id);     //定义Flight_ID为id的航班上的一个座位
            List<string> needs = new List<string>();    //查询的数据需求,此处时座位号
            needs.Add("Seat_Number");
            List<string> res = new List<string>();     //查询结果
            res = DataBaseAccess.GetSingleInfo(obj, needs);     


            int[] SeatArray = new int[capacity];
            for (int i = 0; i < capacity; i++)        //初始化
            {
                SeatArray[i] = int.MinValue;
            }
            for (int i = 0; i < res.Count; i++)
            {
                if (res[i] != "")
                {
                    SeatArray[int.Parse(res[i]) - 1] = 0;
                }
            }

            //数组,航班号,出发地点(机场,航站楼),出发时间,票价
            //机场,航站楼,出发时间
            TFlightTime objsingle = new TFlightTime(id);
            List<string> needssingle = new List<string>();
            needssingle.Add("T_Airport");
            needssingle.Add("T_Terminal");
            needssingle.Add("Ex_TakeOff_Time");
            List<string> ressingle = new List<string>();
            ressingle = DataBaseAccess.GetSingleInfo(objsingle, needssingle);
            //
            //票价
            Flight objflight = new Flight(id);
            List<string> needvalue = new List<string>();
            needvalue.Add("Value");
            List<string> resvalue = new List<string>();
            resvalue = DataBaseAccess.GetSingleInfo(objflight, needvalue);
            //
            List<string> all = new List<string>();
            //
            all.Add(id);
            foreach (string str in ressingle)
            {
                all.Add(str);
            }
            all.Add(resvalue[0]);
            //
            FlightSelectReturn data = new FlightSelectReturn(SeatArray, all);
            return View("Flight", data);
        }

        public ActionResult ReceiveMsg()       //返回登机牌页面                //然后如果乘客有行李,那么添加行李到luggage
        {
            //姓名,性别,ID,航班号,出发时间,座位号

            //获取ID,查询姓名,性别
            int Current_Customer_ID = int.Parse(Request.Params["Customer_ID"]);
            Customer cobj = new Customer(Current_Customer_ID); 
            List<string> SingleInfo = new List<string>();
            SingleInfo.Add("Name");         //0
            SingleInfo.Add("Gender");         //1
            List<string> resInfo = new List<string>();
            resInfo = DataBaseAccess.GetSingleInfo(cobj, SingleInfo);
            resInfo.Add(Current_Customer_ID.ToString());           //2
            //
            //获取当前飞机ID
            string Current_Flight_ID = Request.Params["current_flight"];            //3
            //出发地点
            string airport_terminal = Request.Params["airport_terminal"];           //4
            //出发时间
            string ex_takeoff_time = Request.Params["ex_takeoff_time"];             //5
            //座位号
            int Pickrt = int.Parse(Request.Params["result"]);                       //6
               
            resInfo.Add(Current_Flight_ID);
            resInfo.Add(airport_terminal);
            resInfo.Add(ex_takeoff_time);
            resInfo.Add(Pickrt.ToString());
            //座位,更新已预订为已办理

            SEAT objst = new SEAT() { F_ID = Current_Flight_ID, SEAT_NUMBER = Pickrt.ToString(), C_ID = Current_Customer_ID, STATE = "已办理" };
            db.Updateable(objst).ExecuteCommand();
            /***************************************/
            //在这里将已办理人数加一

            var fstate = db.Queryable<FLIGHT>().Where(it => it.F_ID == Current_Flight_ID).ToList();
            foreach (var i in fstate)
            {
                
                i.CHECKED++;
                db.Updateable(i).ExecuteCommand();
            }
            //
            /***************************************/
            //在这里检查顾客是否有行李
            List<string> weight = new List<string>();
            weight.Add("Lug_Weight");
            var Current_Customer_Luggage = DataBaseAccess.GetSingleInfo(cobj, weight);
            if (Current_Customer_Luggage[0].ToString() != "")
            {
                //顾客ID     Current_Customer_ID
                //航班ID     Current_Flight_ID
                //重量       Current_Customer_Luggage.ToString()
                //费用       y=kx-b
                int cost;
                if (int.Parse(Current_Customer_Luggage[0].ToString()) < 5)
                {
                    cost = 0;
                }
                else
                {
                    cost = (int.Parse(Current_Customer_Luggage[0].ToString()) - 5) * 50;
                }

                //状态
                string state = "已办理";
                LUGGAGE luggage = new LUGGAGE() { F_ID = Current_Flight_ID, L_ID = Current_Customer_ID, WEGHT = Convert.ToInt16(int.Parse(Current_Customer_Luggage[0])), STATE = state };
                db.Insertable(luggage).ExecuteCommand();
                /*************************************/
            }
            return View("ShowBoardingPass", resInfo);
        }

        public ActionResult ShowFlightsState()
        {
            //
            List<string> needs = new List<string>();
            needs.Add("F_ID");
            needs.Add("t_airport");
            needs.Add("t_terminal");
            needs.Add("l_airport");
            needs.Add("l_termnal");
            needs.Add("ex_takeoff_time");
            needs.Add("ex_land_time");
            needs.Add("state");
            List<FlightStateReturn> res = DataBaseAccess.GetFlightsState(needs);
            //
            return View("FlightState", res);
        }

        public ActionResult UpdateFlightState()
        {
            List<string> data = new List<string>();
            data.Add(Request.Params["a"]);
            data.Add(Request.Params["b"]);
            data.Add(Request.Params["c"]);
            data.Add(Request.Params["d"]);
            data.Add(Request.Params["e"]);
            data.Add(Request.Params["f"]);
            return View("UpdateFlightState", data);
        }

        public ActionResult ReturnToShowFlightState()
        {
            //在这里更改后返回
            string nfid = Request.Params["a"];
            //需要将这两项每一项拆分为两部分机场航站楼
            string nt_a_t = Request.Params["b"];
            string[] sArray1 = Regex.Split(nt_a_t, "-", RegexOptions.IgnoreCase);
            string nt_a = sArray1[0];
            string nt_t = sArray1[1];

            string nl_a_t = Request.Params["c"];
            string[] sArray2 = Regex.Split(nl_a_t, "-", RegexOptions.IgnoreCase);
            string nl_a = sArray2[0];
            string nl_t = sArray2[1];
            //
            string ne_t_t = Request.Params["d"];
            string ne_l_t = Request.Params["e"];
            string nstate = Request.Params["f"];
            //
            /*在这里修改数据库*/

            FLIGHTIME objft = new FLIGHTIME() { F_ID = nfid, T_AIRPORT = nt_a, T_TERMINAL = nt_t, EX_TAKEOFF_TIME = Convert.ToDateTime(ne_t_t), EX_LAND_TIME = Convert.ToDateTime(ne_l_t), L_AIRPORT = nl_a, L_TERMNAL = nl_t };
            db.Updateable(objft).ExecuteCommand();


            FSTATE objstate = new FSTATE() { F_ID = nfid, STATE = nstate };
            db.Updateable(objstate).ExecuteCommand();

            /*
            Fstate objstate = new Fstate(nfid, nstate);
            DataBaseAccess.UpdateObject(objstate);
            */
            //
            List<string> needs = new List<string>();
            needs.Add("F_ID");
            needs.Add("t_airport");
            needs.Add("t_terminal");
            needs.Add("l_airport");
            needs.Add("l_termnal");
            needs.Add("ex_takeoff_time");
            needs.Add("ex_land_time");
            needs.Add("state");
            List<FlightStateReturn> res = DataBaseAccess.GetFlightsState(needs);
            //
            return View("FlightState", res);
            //
        }

        public ActionResult ShowCustomerID()
        {
            string fid = Request.Params["fid"];
            //根据fid查c_id
            var ids = db.Queryable<SEAT>().Where(it => it.F_ID == fid & it.STATE == "已购票").ToList();
            //

            string resultstr = "";
            foreach (var id in ids)
            {
                string wrnm = string.Format("<option value= \"{0}\">", id.C_ID.ToString());
                resultstr += wrnm + id.C_ID + "</option>";
            }
            return Content(resultstr);
        }
    }
}