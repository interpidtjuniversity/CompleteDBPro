using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Windows.Forms;

namespace WebApplication4.Models
{
    public class DBAttribute
    {
        enum DBAttributeType { Table,Primary,Member};
        public class DBTableAttribute : Attribute                   //Table属性
        {
            public string table;
            public DBTableAttribute(string t)
            {
                table = t;
            }
        }

        public class DBMemberAttribute : Attribute                  //成员属性
        {
            public string key;
            public DBMemberAttribute(string t)
            {
                key = t;
            }
        }

        public class DBPrimaryKeyAttribute : Attribute             //主码属性
        {
            public string key;
            public DBPrimaryKeyAttribute(string t)
            {
                key = t;
            }
        }
       
        public static string GetDBTable(Type type)
        {
            foreach (Attribute attr in Attribute.GetCustomAttributes(type))
            {
                if (attr.GetType() == typeof(DBTableAttribute))
                {
                    return ((DBTableAttribute)attr).table;
                }
            }
            throw new ArgumentException("Invaild Argument!\n\n from public static string getTableAttribute(Type type)\n");
        }

        public static void GetDBElement(Type type, object obj, List<string> key, List<string> value, List<string> primaryKey, List<string> primaryValue)
        {
            PropertyInfo[] propertyInfoList = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(propertyInfo))
                {
                    if (attr.GetType() == typeof(DBPrimaryKeyAttribute))
                    {
                        object val = propertyInfo.GetValue(obj);
                        if (val != null)
                        {
                            primaryValue.Add("'" + val.ToString() + "'");
                            primaryKey.Add(((DBPrimaryKeyAttribute)attr).key);
                        }
                        break;
                    }
                    if (attr.GetType() == typeof(DBMemberAttribute))
                    {

                        object val = propertyInfo.GetValue(obj);
                        if (val != null)
                        {
                            value.Add("'" + val.ToString() + "'");
                            key.Add(((DBMemberAttribute)attr).key);
                        }

                    }
                }
            }
        }

        public static void GetDBElement(Type type, Type target, object obj, List<string> key, List<string> value)
        {
            PropertyInfo[] propertyInfoList = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(propertyInfo))
                {
                    if (attr.GetType() == target)
                    {
                        key.Add(propertyInfo.Name);
                        object val = propertyInfo.GetValue(obj);
                        if (val != null)
                            value.Add("'" + val.ToString() + "'");
                        else
                            value.Add(null);
                        break;
                    }
                }
            }
        }
        public static void GetDBElement(Type type, object obj, List<string> key, List<string> value)
        {
            GetDBElement(type, typeof(DBMemberAttribute), obj, key, value);
        }
        public static void GetDBPrimaryElement(Type type, object obj, List<string> key, List<string> value)
        {
            GetDBElement(type, typeof(DBPrimaryKeyAttribute), obj, key, value);

        }
    }
}