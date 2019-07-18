using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;//oracle数据提供程序
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using static WebApplication4.Models.DBAttribute;

namespace WebApplication4.Models
{
    public class OracleHelper
    {      
        public static OracleDataReader GetReader(string sql)
        {
            string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;";
            OracleConnection con = new OracleConnection(connString);
            try
            {
                con.Open();
                OracleCommand com = new OracleCommand(sql, con);
                return com.ExecuteReader(CommandBehavior.CloseConnection);//dataReader关闭后自动关闭
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //插入一个对象
        public static bool InsertObject(OracleConnection connection, object obj)
        {
            StringBuilder StrSql = new StringBuilder();                 //字符串拼接器

            List<string> All = new List<string>();                      //要插入数据项的表的所有成员Insert Into ** (A,B,C,D....)
            Dictionary<string, OracleParameter> sop = GetDBMember(obj, null, null, All);
            List<OracleParameter> op = new List<OracleParameter>();             //插入命令的集合/属性等于值..属性等于值..属性等于值
            try
            {
                StrSql.Append(string.Format("insert into {0}", GetDBTable(obj.GetType())));//DBAttribute.GetDBTable(obj.GetType()))
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            //    throw new ArgumentException("Invaild Argument!\n\n from Insert(OracleConnection connection,object obj) \n");
            }
            StrSql.Append(string.Format("( {0} )", string.Join(",", All.ToArray())));
            StrSql.Append(string.Format(" values ({0})", ":" + string.Join(",:", All.ToArray())));     //??????all.ToArray()??????
            //CommandText//insert into DBAttribute.getDBTable(obj.GetType()) {all.ToArray()} values (,:  ,:  ,: ....))/参数按顺序自动匹配Parameters 里的值
            foreach (string str in All)
            {
                op.Add(sop[str]);
            }
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, StrSql.ToString(), op.ToArray());
            int tem = cmd.ExecuteNonQuery();

            return tem != -1 && tem != 0;
        }

        public static Dictionary<string, OracleParameter> GetDBMember(object obj, List<string> key = null,
             List<string> primaryKey = null, List<string> allMember = null)             //allMember一个插入命令里的所有成员
        {
            Type type = obj.GetType();

            //返回obj的所有公共属性存储在propertyInfoList
            PropertyInfo[] propertyInfoList = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            //OracleParameter通过Value的赋值构成操作op
            //lo<属性名,<属性名:属性值>  >
            Dictionary<string, OracleParameter> lo = new Dictionary<string, OracleParameter>();
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                //得到属性的属性值
                object val = propertyInfo.GetValue(obj);
                //属性,字典,属性值,key,primaryKey,list
                createOraParam(propertyInfo, lo, val, key, primaryKey, allMember);
            }
            return lo;
        }

        public static void createOraParam(PropertyInfo propertyInfo, Dictionary<string, OracleParameter> lo, object val, List<string> key = null,
            List<string> primaryKey = null, List<string> allMember = null)
        {
            //原子操作
            OracleParameter op = new OracleParameter();
            bool primary = false;
            foreach (Attribute attr in Attribute.GetCustomAttributes(propertyInfo))     //[]里的备注
            {
                if (attr.GetType() == typeof(DBPrimaryKeyAttribute))
                {
                    primary = true;
                    continue;
                }
                if (attr.GetType() == typeof(DBMemberAttribute))
                {
                    op.DbType = TypeConvert.TypeToDBType(propertyInfo.PropertyType);
                    op.OracleDbType = TypeConvert.DbTypeToOracleDbType(op.DbType);
                    op.ParameterName = ((DBMemberAttribute)attr).key;
                    op.Value = val;
                    lo.Add(op.ParameterName, op);
                    if (primaryKey != null && primary)
                    {
                        primaryKey.Add(op.ParameterName);
                    }
                    else if (key != null)
                    {
                        key.Add(op.ParameterName);
                    }
                    if (allMember != null)
                    {
                        allMember.Add(op.ParameterName);
                    }
                }
            }
        }
        public static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms, CommandType type = CommandType.Text)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = type;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        public static int ExecuteSql(OracleConnection connection, string SQLString, OracleParameter[] op = null, CommandType type = CommandType.Text)
        {

            try
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, SQLString, op, type);

                int rows = cmd.ExecuteNonQuery();
                return rows;

            }
            catch (OracleException E)
            {
                throw new Exception(E.Message);
            }
        }
        public static bool Update(OracleConnection connection, object obj)
        {
            //不允许更改主码
            StringBuilder strSql = new StringBuilder();
            List<string> key = new List<string>(), pri = new List<string>();
            Dictionary<string, OracleParameter> opd = GetDBMember(obj, key, pri);
            List<OracleParameter> op = new List<OracleParameter>();

            strSql.Append(string.Format("update {0} set ", DBAttribute.GetDBTable(obj.GetType())));
            for (int i = 0; i < key.Count(); ++i)
            {
                strSql.Append(string.Format(" {0} = :{0} ,", key[i]));
                op.Add(opd[key[i]]);
            }
            --strSql.Length;
            strSql.Append(" where ");
            for (int i = 0; i < pri.Count(); ++i)
            {
                strSql.Append(string.Format(" {0} = :{0} AND", pri[i]));
                op.Add(opd[pri[i]]);
            }
            strSql.Length -= 3;

            return ExecuteSql(connection, strSql.ToString(), op.ToArray()) > 0;


        }

        public static bool Delete(OracleConnection connection, object obj)
        {
            Type type = obj.GetType();
            StringBuilder strSql = new StringBuilder();
            try
            {
                strSql.Append(string.Format("delete from {0}", DBAttribute.GetDBTable(type)));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
             //   throw new ArgumentException("Invaild Argument!\n\n from Delete(OracleConnection connection, object obj) \n");
            }
            List<string> propertyPrimaryKeyList = new List<string>();
            List<string> propertyPrimaryValueList = new List<string>();
            DBAttribute.GetDBPrimaryElement(type, obj, propertyPrimaryKeyList, propertyPrimaryValueList);
            if (!(propertyPrimaryKeyList.Any() && propertyPrimaryValueList.Any()))
            {
                throw new ArgumentException("Invaild Argument!\n\n from Delete(OracleConnection connection, object obj) \n");
            }
            strSql.Append(string.Format(" where "));
            for (int i = 0; i < propertyPrimaryKeyList.Count; ++i)
            {
                strSql.Append(string.Format(" {0}={1} AND", propertyPrimaryKeyList[i], propertyPrimaryValueList[i]));
            }
            strSql.Length -= 3;
            return ExecuteSql(connection, strSql.ToString()) > 0;
        }

        public static void Delete(OracleConnection connection, string table, List<string> key, List<string> value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format("delete from {0}", table));
            if (value != null && value.Any())
            {
                if (key.Count != value.Count)
                    throw new ArgumentException("Invaild Argument! \n\n from Delete(OracleConnection connection, string table,List<string> key,List<string>value) \n");
                strSql.Append(string.Format(" where "));
                for (int i = 0; i < key.Count; ++i)
                {
                    strSql.Append(string.Format(" {0}={1} AND", key[i], value[i]));
                }
            }
            ExecuteSql(connection, strSql.ToString());
        }

        public static bool Exists(OracleConnection connection, string SQLString)
        {
            try
            {
                using (OracleCommand cmd = new OracleCommand(SQLString, connection))
                {
                    object obj = cmd.ExecuteScalar();
                    return !((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)));
                }
            }
            catch (OracleException e)
            {
                throw new Exception(e.Message);
            }
        }

        //从一个表中选择所有的项,用于显示信息
        public static SelectReturn Select(OracleConnection connection, List<object> list, List<string> value, object obj)   //value为数据库查询结果而不是查询条件
        {
            StringBuilder StrSql = new StringBuilder();                 //字符串拼接器
            try
            {
                StrSql.Append(string.Format("select * from {0}", GetDBTable(obj.GetType())));//DBAttribute.GetDBTable(obj.GetType()))
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, StrSql.ToString(), null);
            OracleDataReader odr = null;
            odr = cmd.ExecuteReader();

            Type type = obj.GetType();
            //返回obj的所有公共属性存储在propertyInfoList
            PropertyInfo[] PropertyInfoList = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            int TableSize = PropertyInfoList.Length;
            //
                 //可在此测试表的属性
            //
            while (odr.Read())
            {
                object temp = Clone(obj);

                PropertyInfo[] propertyInfoList = temp.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < TableSize; i++)
                {
//                    propertyInfoList[i].SetValue(temp, odr[i]);
                    //odr[i]为每次循环中读到的字段的值可以考虑返回该值
                    propertyInfoList[i].SetValue(temp, Convert.ChangeType(odr[i], propertyInfoList[i].PropertyType));
                    value.Add(odr[i].ToString());
                }
                list.Add(temp);
            }
            return new SelectReturn(list, value);
        }

        public static object Clone(object sampleObject)           //拷贝一个对象
        {
            Type t = sampleObject.GetType();
            PropertyInfo[] properties = t.GetProperties();
            object p = t.InvokeMember("", BindingFlags.CreateInstance, null, sampleObject, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(sampleObject, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }


        public static void Query(OracleConnection connection, object obj, List<string> needs, List<string> res)
        {
            StringBuilder StrSql = new StringBuilder();                 //字符串拼接器
            try
            {
                StrSql.Append(string.Format("select"));//DBAttribute.GetDBTable(obj.GetType()))
                for (int i = 0; i < needs.Count; i++)
                {
                    if (i == 0)
                    {
                        StrSql.Append(string.Format(" {0}", needs[i]));
                    }
                    else {
                        StrSql.Append(string.Format(",{0}", needs[i]));
                    }
                }
                StrSql.Append(string.Format(" from {0}", GetDBTable(obj.GetType())));//DBAttribute.GetDBTable(obj.GetType()))

                List<string> propertyPrimaryKeyList = new List<string>();
                List<string> propertyPrimaryValueList = new List<string>();
                //DBAttribute.GetDBElement(obj.GetType(), obj, propertyPrimaryKeyList, propertyPrimaryValueList);
                DBAttribute.GetDBPrimaryElement(obj.GetType(), obj, propertyPrimaryKeyList, propertyPrimaryValueList);
                if (!(propertyPrimaryKeyList.Any() && propertyPrimaryValueList.Any()))
                {
                    throw new ArgumentException("Invaild Argument!\n\n from Delete(OracleConnection connection, object obj) \n");
                }
                StrSql.Append(string.Format(" where "));

                for (int i = 0; i < propertyPrimaryKeyList.Count; ++i)
                {
                    if (propertyPrimaryValueList[i] != null)
                    {
                        StrSql.Append(string.Format(" {0}={1} AND", propertyPrimaryKeyList[i], propertyPrimaryValueList[i]));
                    }
                }
                StrSql.Length -= 3;              //减去AND
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, StrSql.ToString(), null);
            OracleDataReader odr = null;
            odr = cmd.ExecuteReader();
            //odr读取数据并存入res
            while (odr.Read())
            {
                for (int i = 0; i < needs.Count; i++)
                {
                    res.Add(odr[i].ToString());
                }
            }
        }

        public static List<FlightStateReturn> QueryFlightsState(OracleConnection connection, List<string> needs)
        {
            List<FlightStateReturn> res = new List<FlightStateReturn>();

            StringBuilder StrSql = new StringBuilder();
            try
            {
                StrSql.Append(string.Format("select"));
                for (int i = 0; i < needs.Count; i++)
                {
                    if (i == 0)
                    {
                        StrSql.Append(string.Format(" {0}", needs[i]));
                    }
                    else
                    {
                        StrSql.Append(string.Format(",{0}", needs[i]));
                    }
                }
                StrSql.Append(string.Format(" from {0}", "fstate natural join flightime"));//DBAttribute.GetDBTable(obj.GetType()))
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException(e.Message);
            }

            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, StrSql.ToString(), null);

            OracleDataReader odr = null;
            odr = cmd.ExecuteReader();
            //odr读取数据并存入res
            while (odr.Read())
            {
                TFlightTime tfe = new TFlightTime();
                int flag = 0;

                tfe.f_id = odr[flag++].ToString();
                tfe.t_airport = odr[flag++].ToString();
                tfe.t_terminal = odr[flag++].ToString();
                tfe.l_airport = odr[flag++].ToString();
                tfe.l_termnal = odr[flag++].ToString();
                tfe.ex_takeoff_time = odr[flag++].ToString();
                tfe.ex_land_time = odr[flag++].ToString();

                FlightStateReturn fsr = new FlightStateReturn(tfe, odr[flag].ToString());
                res.Add(fsr);
            }
            return res;
        }
    }
}