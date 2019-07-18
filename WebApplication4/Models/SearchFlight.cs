using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using WebApplication4.Models;

namespace WebApplication4.Models
{
    public class SearchFlight
    {
        public TFlightTime Search (string f_id)
        {
            DBConnectionPool pool = new DBConnectionPool(10);
            OracleConnection conn = pool.fetchConnection();


            string sql = "select * from FLIGHTIME where F_ID='" + f_id +  "'";
            TFlightTime flight = new TFlightTime();

            OracleCommand cmd = new OracleCommand();

            OracleHelper.PrepareCommand(cmd, conn, null, sql, null);

            OracleDataReader objReader = null;
            objReader = cmd.ExecuteReader();


            while (objReader.Read())
            {
                flight.f_id = objReader[0].ToString();
                flight.t_airport = objReader[1].ToString();
                flight.t_terminal = objReader["t_terminal"].ToString();
                flight.ex_takeoff_time = objReader["ex_takeoff_time"].ToString();
                flight.ex_land_time = objReader["ex_land_time"].ToString();
                flight.re_land_time = null;
                flight.re_takeoff_time = null;
                flight.l_airport = objReader["l_airport"].ToString();
                flight.l_termnal = objReader["l_termnal"].ToString();

            }
            return flight;
        }
    }
}