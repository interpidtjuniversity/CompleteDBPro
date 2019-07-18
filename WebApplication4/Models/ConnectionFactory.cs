
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class ConnectionFactory:IConnectionFactory
    {

        string connString;


        public ConnectionFactory()
        {
            
            StreamReader sr = new StreamReader("ConnectionString.json");
            char[] buff = new char[1024];
            sr.ReadBlock(buff, 0, 1024);
            string te =new string(buff);
            var tem = ParseJson.parseSimpleParam(te);
            connString = tem["ConnectionStrings"];
        }
        public OracleConnection CreateConnection()
        {
            OracleConnection connection = null;
            if (string.IsNullOrWhiteSpace(connString))
                throw new ArgumentNullException("DBconnection string is NULL");
            connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;";
            connection = new OracleConnection(connString);
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            if (connection.State != System.Data.ConnectionState.Open)
                throw new ArgumentException("DBconnection connection failed!");

            return connection;
        }
    }
}
