using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication4.Models;
using Dapper;
using System.Reflection;
using System.Windows.Forms;

namespace WebApplication4.Models
{
    public static class DataBaseAccess
    {
        static DBConnectionPool pool;
        static DataBaseAccess()
        {
            pool = DBConnectionPool.getInstance();
        }
        public static bool insertObj(object obj)
        {
            OracleConnection conn = pool.fetchConnection();
            bool res = OracleHelper.InsertObject(conn, obj);
            pool.releaseConnection(conn);
            return res;
        }
        public static bool updateObj(object obj)
        {
            OracleConnection conn = pool.fetchConnection();
            bool res = OracleHelper.Update(conn,obj);
            pool.releaseConnection(conn);
            return res;
        }
        public static bool deleteObj(object obj)
        {
            OracleConnection conn = pool.fetchConnection();
            bool res = OracleHelper.Delete(conn, obj);
            pool.releaseConnection(conn);
            return res;
        }
        public static bool existObj(object obj)
        {
            List<string> prikey = new List<string>(), prival = new List<string>();
            DBAttribute.GetDBPrimaryElement(obj.GetType(), obj, prikey, prival);
            return exist(obj.GetType(), prikey, prival);
        }
        public static bool exist(Type type,List<string> key, List<string> val)
        {
            OracleConnection conn = pool.fetchConnection();
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("select * from {0} where ", DBAttribute.GetDBTable(type)));
            int len = key.Count();
            for (int i = 0; i < len; ++i)
            {
                if (i != 0)
                    sb.Append(" AND ");
                sb.Append($"{key[i]}='{val[i]}'");

            }
            bool res = OracleHelper.Exists(conn, sb.ToString());
            pool.releaseConnection(conn);
            return res;
        }
        public static OracleDataReader qerySql(string sqlStr)
        {
            return null;
        }
        public static List<T> testQuery<T>( string sqlStr)
        {
            OracleConnection conn = pool.fetchConnection();
            List<T> lt = SqlMapper.Query<T>(conn, sqlStr).ToList();
            pool.releaseConnection(conn);
            return lt;
        }
        public static int ExecuteSql(string SQLString)
        {
            OracleConnection conn = pool.fetchConnection();
            int res = OracleHelper.ExecuteSql(conn, SQLString);
            pool.releaseConnection(conn);
            return res;
        }
        public static SelectReturn GetAllTInfo(object obj)           //找到该obj所对应的表,返回该表所有的行
        {
            OracleConnection conn = pool.fetchConnection();
            List<object> list = new List<object>();
            List<string> value = new List<string>();
            SelectReturn sr = new SelectReturn();
            sr = OracleHelper.Select(conn, list, value, obj);
            pool.releaseConnection(conn);
            return sr;
        }

        public static List<string> GetSingleInfo(object obj, List<string> needs)      //needs装载查询需求
        {
            OracleConnection conn = pool.fetchConnection();
            List<string> res = new List<string>();        //装载查询到的结果
            OracleHelper.Query(conn, obj, needs, res);
            pool.releaseConnection(conn);
            return res;
        }


        public static List<FlightStateReturn> GetFlightsState(List<string> needs)
        {
            OracleConnection conn = pool.fetchConnection();
            List<FlightStateReturn> res = new List<FlightStateReturn>();
            res = OracleHelper.QueryFlightsState(conn, needs);
            pool.releaseConnection(conn);
            return res;
        }


        public static void UpdateObject(object obj)
        {
            OracleConnection conn = pool.fetchConnection();
            OracleHelper.Update(conn, obj);
            pool.releaseConnection(conn);
        }
    }
}
