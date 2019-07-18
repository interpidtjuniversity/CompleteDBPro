using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using SqlSugar;
using Sugar.Enties;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class LuggageController : Controller
    {
        // GET: Luggage

        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
            DbType = DbType.Oracle,//设置数据库类型
            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
            InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
        });

        public ActionResult ShowLuggages()
        {
            /***********///查询出luggage
            List<Luggage> Luggages = new List<Luggage>();
            List<object> list = new List<object>();     //对象
            List<string> value = new List<string>();       //值
            Luggage obj = new Luggage();
            SelectReturn sr = DataBaseAccess.GetAllTInfo(obj);
            list = sr.list;
            value = sr.value;
            //obj转Luggage
            int flag = 0;
            foreach (object j in list)
            {
                Luggage luggage = new Luggage();//current
                //手动赋值,暂时没有找到映射的方法
                luggage.F_ID = value[flag++];
                luggage.L_ID = int.Parse(value[flag++]);
                luggage.Weght = int.Parse(value[flag++]);
                luggage.State = value[flag++];
                Luggages.Add(luggage);
            }
            //
            return View("ShowLuggages", Luggages);
        }

        public ActionResult ReturnToShowLuggages()
        {
            string cur_state = Request.Params["s"];
            cur_state = cur_state.Trim();

            if (cur_state == "已登机")
            {
                MessageBox.Show("该行李已经登机啦!");
            }
            else
            {
                int Luggage_ID = int.Parse(Request.Params["l_id"]);
                string Flight_ID = Request.Params["ff_id"];
                Flight_ID = Flight_ID.Trim();
                int w = int.Parse(Request.Params["w"]);
                StringBuilder s = new StringBuilder();
                s.Append("已登机");

                LUGGAGE objlug = new LUGGAGE() { F_ID = Flight_ID, L_ID = Luggage_ID, WEGHT = Convert.ToInt16(w), STATE = s.ToString() };
                db.Updateable(objlug).ExecuteCommand();

            }
            List<Luggage> Luggages = new List<Luggage>();
            List<object> list = new List<object>();     //对象
            List<string> value = new List<string>();       //值
            Luggage obj = new Luggage();
            SelectReturn sr = DataBaseAccess.GetAllTInfo(obj);
            list = sr.list;
            value = sr.value;
            //obj转Luggage
            int flag = 0;
            foreach (object j in list)
            {
                Luggage luggage = new Luggage();//current
                //手动赋值,暂时没有找到映射的方法
                luggage.F_ID = value[flag++];
                luggage.L_ID = int.Parse(value[flag++]);
                luggage.Weght = int.Parse(value[flag++]);
                luggage.State = value[flag++];
                Luggages.Add(luggage);
            }
            //
            return View("ShowLuggages", Luggages);
        }

    }
}