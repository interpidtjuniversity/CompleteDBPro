using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    //该类为OracleHelper Select方法的返回类型
    public class SelectReturn
    {
        public List<object> list;     //查询到的对象
        public List<string> value;    //查询到的值

        public SelectReturn()
        {
            list = new List<object>();
            value = new List<string>();
        }

        public SelectReturn(List<object> l, List<string> v)
        {
            list = l;
            value = v;
        }
    }
}