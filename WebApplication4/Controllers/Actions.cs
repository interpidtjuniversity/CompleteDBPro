using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using Sugar.Enties;

namespace WebApplication4.Controllers
{
    public class Actions : BaseDao
    {
        public List<FLIGHTIME> searchDropFlights()
        {
            var systime = SystemTime;
            var dropFlightList = db.Queryable<FLIGHTIME>().Where(it => it.EX_LAND_TIME == systime).ToList();
            return dropFlightList;
        }
        public void dealDropFlightime(FLIGHTIME dropFlightime)
        {

            db.Updateable<FLIGHTIME>().SetColumns(it => new FLIGHTIME() { RE_LAND_TIME = dropFlightime.EX_LAND_TIME })
               .Where(it => it.F_ID == dropFlightime.F_ID).ExecuteCommand();
            db.Updateable<FSTATE>().SetColumns(it => new FSTATE() { STATE = "已降落" })
            .Where(it => it.F_ID == dropFlightime.F_ID).ExecuteCommand();
        }
        public PLANE GetPLANE(string p_id)
        {
            PLANE plane =db.Queryable<PLANE>().Where(it => it.ID == p_id).Single();
            return plane;
        }

        public List<FLIGHTIME> GetFlightTime(string company)
        {
            List<PLANE> planes = db.Queryable<PLANE>().Where(it => it.COMPANY == company).ToList();
            List<FLIGHT> flights = new List<FLIGHT>();
            List<FLIGHTIME> flightTimes = new List<FLIGHTIME>();
            foreach (PLANE p in planes){
                flights = db.Queryable<FLIGHT>().Where(it => it.P_ID == p.ID).ToList();
            }
            foreach (FLIGHT f in flights)
            {
                flightTimes = db.Queryable<FLIGHTIME>().Where(it => it.F_ID == f.F_ID).ToList();
            }
            return flightTimes;
        }

        public List<PLANE> GetPlanes_com(string company)
        {
            List<PLANE> planes = db.Queryable<PLANE>().Where(it => it.COMPANY == company).ToList();
            return planes;
        }


        public void dealDropFlight_return(DateTime ex_takeoftime_re, DateTime ex_landtime_re, string d_ID_re)
        {
            db.Updateable<FLIGHTIME>().SetColumns(it => new FLIGHTIME() { EX_TAKEOFF_TIME = ex_takeoftime_re, EX_LAND_TIME = ex_landtime_re })
             .Where(it => it.F_ID == d_ID_re).ExecuteCommand();
        }
        public void departFstate(string d_ID_re) {
            
            db.Updateable<FSTATE>().SetColumns(it => new FSTATE() { STATE = "待起飞" })
           .Where(it => it.F_ID == d_ID_re).ExecuteCommand();
        }
        public List<FLIGHTIME> searchnewFlightimes()
        {
            var systime = SystemTime;
            var newtime = systime.AddHours(5);
            var newFlightimeList = db.Queryable<FLIGHTIME>().Where(it => it.EX_TAKEOFF_TIME == newtime).ToList();
            return newFlightimeList;
        }
        public void delSeat(string d_ID)
        {
            db.Deleteable<SEAT>(it => it.F_ID == d_ID).ExecuteCommand();
        }
        public void delLuggage(string d_ID)
        {
            db.Deleteable<LUGGAGE>(it => it.F_ID == d_ID).ExecuteCommand();
        }
        public void dropFlightUpdate(string d_ID,string d_ID_re)
        {
            db.Updateable<FLIGHT>().SetColumns(it => new FLIGHT() { BOARD = 0, CHECKED = 0, F_ID = d_ID_re })
             .Where(it => it.F_ID == d_ID).ExecuteCommand();

        }
        public List<CUSTOMER> selectFreeCustomers(int count)
        {
            var CustomerList = db.Queryable<CUSTOMER>().Where(it =>
                   SqlFunc.Subqueryable<SEAT>().Where(s => s.C_ID == it.ID).NotAny()).Take(count).ToList();
            return CustomerList;
        }
        public void InsertSeat(SEAT insertSeat)
        {
            db.Insertable<SEAT>(insertSeat).ExecuteCommand();
        }
       
        public List<FLIGHTIME> searchPreCardFlights(DateTime preCardTime)
        {
            var preCardFlightList = db.Queryable<FLIGHTIME>().Where(it => it.EX_LAND_TIME == preCardTime).ToList();
            return preCardFlightList;
        }
        public List<SEAT> searchPreCardSeats(string F_ID)
        {
            var preCardSeatList = db.Queryable<SEAT>().Where(it => it.F_ID == F_ID && it.STATE == "已购票").ToList();
            return preCardSeatList;
        }
        public void updateSeat(SEAT seat)
        {
            db.Updateable(seat).ExecuteCommand();
        }
        public void updateSeat(List <SEAT> seats)
        {
            db.Updateable(seats).ExecuteCommand();
        }
        public bool judgeFoundSeat(string F_ID,string seat_Number)
        {
            bool found = db.Queryable<SEAT>().Any(it => it.SEAT_NUMBER == seat_Number
                            && it.F_ID == F_ID);
            return found;
        }
        public CUSTOMER getCustomer(decimal C_ID)
        {
            CUSTOMER customer = db.Queryable<CUSTOMER>().Where(it => it.ID == C_ID).Single();
            return customer;
        }
        public void InsertLuggage(LUGGAGE luggage)
        {
            db.Insertable(luggage).ExecuteCommand();
        }
        public FLIGHT getFlight(string F_ID)
        {
           FLIGHT flight= db.Queryable<FLIGHT>().Where(it => it.F_ID == F_ID).Single();
            return flight;
        }
        public void updateFlight(FLIGHT flight)
        {
            db.Updateable(flight).ExecuteCommand();
        }
        public List<FLIGHTIME> GetpreBoardFlightimeList(DateTime preBoardtime)
        {
            List<FLIGHTIME> preBoardFlightimes = db.Queryable<FLIGHTIME>().Where(it => it.EX_LAND_TIME == preBoardtime).ToList();
            return preBoardFlightimes;
        }
        public List<SEAT> GetpreBoardSeats(string F_ID)
        {
            List<SEAT> preBoardSeats= db.Queryable<SEAT>().Where(it => it.F_ID == F_ID && it.STATE == "已办理").ToList();
            return preBoardSeats;
        }
        public void UpdateBoardLuggage(string F_ID)
        {
            db.Updateable<LUGGAGE>()                      //该飞机行李全部装入
                 .SetColumns(it => new LUGGAGE() { STATE = "已登机" })
                 .Where(it => it.F_ID == F_ID).ExecuteCommand();
        }
        
        public List<FLIGHTIME> searchDepartFlightimes()
        {
            var systime = SystemTime;
            var DepartFlightList =db.Queryable<FLIGHTIME>().Where(it => it.EX_TAKEOFF_TIME == systime).ToList();
            return DepartFlightList;
        }
        public FSTATE GetFstate(string F_ID)
        {
            FSTATE fstate= db.Queryable<FSTATE>().Where(it => it.F_ID == F_ID).Single();
            return fstate;
        }
        public void updateFstate(FSTATE fstate)
        {
            db.Updateable(fstate).ExecuteCommand();
        }
        public void updateFlightime(List<FLIGHTIME> flightimes)
        {
            db.Updateable(flightimes).ExecuteCommand();
        }
        public void updateFstate(List<FSTATE> fstates)
        {
            db.Updateable(fstates).ExecuteCommand();
        }
   
       

        public DateTime SystemTime
        {
           get
            {
                var systime = db.Queryable<TIME>().Select((sc) => new { sc.time }).Single();
                return systime.time;
            }
           set
            {
                TIME systime = new TIME();
                systime.TYPE = "系统时间";
                systime.time = value;
                db.Updateable(systime).ExecuteCommand();
            }
        }
    }
      
}

public class BaseDao
{

    public SqlSugar.SqlSugarClient db { get { return GetInstance(); } }
    public void BeginTran()
    {
        db.Ado.BeginTran();
    }
    public void CommitTran()
    {
        db.Ado.CommitTran();
    }
    public void RollbackTran()
    {
        db.Ado.RollbackTran();
    }
    public SqlSugarClient GetInstance()
    {
        SqlSugarClient db = new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
                DbType = SqlSugar.DbType.Oracle,
                IsAutoCloseConnection = true,
                IsShardSameThread = true /*Shard Same Thread*/
            });

        return db;
    }
}
