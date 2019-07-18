using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WebApplication4.Models
{
    public class LoginService
    {
        public User AdminLogin(User objAdmin)
        {
            string account = objAdmin.Account;
            string pw = objAdmin.Pwd;
            string sql = "select ID from Users where Account='" + account + "'and Pwd='" + pw + "'";

            OracleDataReader objReader = OracleHelper.GetReader(sql);
            if (!objReader.Read())
            {
                objAdmin = null;
            }
            else
            {
                objAdmin.ID = int.Parse(objReader[0].ToString());
            }
            return objAdmin;
        }

        public Company AdminLogin(Company objAdmin)
        {
            string account = objAdmin.Account;
            string pw = objAdmin.Pwd;
            string sql = "select ID from Company where Account='" + account + "'and Pwd='" + pw + "'";

            OracleDataReader objReader = OracleHelper.GetReader(sql);
            if (!objReader.Read())
            {
                objAdmin = null;
            }
            else
            {
                objAdmin.ID = int.Parse(objReader[0].ToString());
            }
            return objAdmin;
        }
    }
}