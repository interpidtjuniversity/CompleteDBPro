using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace WebApplication4.Models
{
    public class RegisterService
    {
        public User UserCheck(User objcheck)
        {
            string account = objcheck.Account;
            string sql = "select account from Users where Account='" + account + "'";

            OracleDataReader objReader = OracleHelper.GetReader(sql);
            if (!objReader.Read())
            {
                objcheck = null;
            }
            return objcheck;
        }
    }
}