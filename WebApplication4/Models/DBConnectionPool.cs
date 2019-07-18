
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication4.Models
{

    /*
    * 留坑  多线程  同步
    */

    public class DBConnectionPool
    {
        static ConnectionFactory factory;
        static int ConnectionMaxSize;
        static int ConnectionCurSize = 0;
        static List<OracleConnection> pool;
        private static DBConnectionPool sington;
        private static object locker ;
        private static AutoResetEvent event_0;
        static DBConnectionPool()
        {
            factory = new ConnectionFactory();
            ConnectionMaxSize = 50;
            pool = new List<OracleConnection>();
            sington = new DBConnectionPool();
            locker = new object();
            event_0 = new AutoResetEvent(false);
        }
        
        public static DBConnectionPool getInstance()
        {
            return sington;
        }
        public DBConnectionPool(int initialSize = 10)
        {
            ConnectionCurSize = initialSize;
            if (initialSize <= 0 || initialSize > ConnectionMaxSize)
                throw new ArgumentException("Invaild size of Connection pool ");
            for (int i = 0; i < initialSize; ++i)
                pool.Add(factory.CreateConnection());
        }
        public void releaseConnection(OracleConnection connection)
        {

            if (connection != null)
            {
                pool.Add(connection);
            }
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
            connection.Open();
            if (connection.State != System.Data.ConnectionState.Open)
                throw new ArgumentException("DBconnection connection failed!");

        }
        public OracleConnection fetchConnection(int mills = -1)
        {
            OracleConnection connection = null;
            lock (locker)
            {
                if (pool.Any())
                {
                    connection = pool[0];
                    pool.RemoveAt(0);
                    return connection;
                }
                if (ConnectionCurSize < ConnectionMaxSize)
                {
                    ++ConnectionCurSize;
                    connection = factory.CreateConnection();
                    return connection;
                }
            }
            mills = mills <= 0 ? -1 : mills;
            while (true)
            {
                lock (locker)
                {
                    if (pool.Any())
                        break;
                }
                if (!event_0.WaitOne(mills))
                {
                    return null;
                }
            }
            if (connection == null && !pool.Any())
            {
                connection = pool[0];
                pool.RemoveAt(0);
            }
            return connection;
        }
    }
}
