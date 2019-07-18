using SqlSugar;
using Sugar.Enties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class BoardPassController : Controller
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
            DbType = DbType.Oracle,//设置数据库类型
            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
            InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
        });


        // GET: BoardPass
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
            needs.Add("state");                         //此数值需要和系统时间Time相减;
            List<FlightStateReturn> res = DataBaseAccess.GetFlightsState(needs);
            //
            return View("CheckBoardingStates", res);
        }

        public ActionResult ShowCustomerBoardingStates()
        {
            List<CustomerBoardingState> list = new List<CustomerBoardingState>();
            //航班号(根据航班号查找C_ID)            <乘客ID,姓名,性别,电话号码>(根据C_ID查找customer),状态(根据F_ID,C_ID,来查找seat)
            String Flight_ID = Request.Params["a"];
            /*******///找乘客
            var ids = db.Queryable<SEAT>().Where(it => it.F_ID == Flight_ID).ToList();
            foreach (var id in ids)
            {
                //
                var customer = db.Queryable<CUSTOMER>().Where(it => it.ID == id.C_ID).ToList();
                var state = db.Queryable<SEAT>().Where(it => it.F_ID == Flight_ID & it.C_ID == id.C_ID).ToList();
                CustomerBoardingState cbs = new CustomerBoardingState(customer[0].ID, customer[0].NAME,
                    customer[0].GENDER, customer[0].PHONE_NUMBER, Flight_ID, state[0].STATE, id.SEAT_NUMBER);
                list.Add(cbs);
            }
            CustomerBoardingState tail = new CustomerBoardingState();
            list.Add(tail);
            /***********************************/
            //MessageBox.Show(list.Count.ToString());
            //
            /***********************************/
            return View("CustomerBoardingStates", list);
        }

        public ActionResult ReturnToShowCustomerBoardingStates()
        {
            string fid = Request.Params["f_id"];
            fid = fid.Trim();
            int cid = int.Parse(Request.Params["c_id"]);
            string seat = Request.Params["seat"];
            seat = seat.Trim();
            string cur_state = Request.Params["s"];
            cur_state = cur_state.Trim();

            if (cur_state == "已登机")
                MessageBox.Show("该乘客已经登机啦!");
            else if (cur_state == "已购票")
                MessageBox.Show("该乘客还未办理登机牌");
            else
            {
                //更新座位
                SEAT objs = new SEAT() { F_ID = fid, C_ID = cid, SEAT_NUMBER = seat, STATE = "已登机" };
                db.Updateable(objs).ExecuteCommand();
                /**********************/
                //在这里将已登机人数加1

                var fstate = db.Queryable<FLIGHT>().Where(it => it.F_ID == fid).ToList();
                foreach (var i in fstate)
                {
                    i.BOARD++;
                    db.Updateable(i).ExecuteCommand();
                }
            }
            //


            List<CustomerBoardingState> list = new List<CustomerBoardingState>();
            //航班号(根据航班号查找C_ID)            <乘客ID,姓名,性别,电话号码>(根据C_ID查找customer),状态(根据F_ID,C_ID,来查找seat)
            String Flight_ID = fid;
            /*******///找乘客
            var ids = db.Queryable<SEAT>().Where(it => it.F_ID == Flight_ID).ToList();
            foreach (var id in ids)
            {
                //
                var customer = db.Queryable<CUSTOMER>().Where(it => it.ID == id.C_ID).ToList();
                var state = db.Queryable<SEAT>().Where(it => it.F_ID == Flight_ID & it.C_ID == id.C_ID).ToList();
                CustomerBoardingState cbs = new CustomerBoardingState(Convert.ToInt32(customer[0].ID), customer[0].NAME, customer[0].GENDER, 
                    Convert.ToInt32(customer[0].PHONE_NUMBER), Flight_ID, state[0].STATE, id.SEAT_NUMBER);
                list.Add(cbs);
            }
            CustomerBoardingState tail = new CustomerBoardingState();
            list.Add(tail);
            //
            return View("CustomerBoardingStates", list);
        }
    }
}