using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public interface IConnectionFactory
    {
        OracleConnection CreateConnection();
    }
}
