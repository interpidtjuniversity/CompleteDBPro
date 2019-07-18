using Sugar.Enties;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
public class DbContext<T> where T : class, new()
{
    public DbContext()
    {
        Db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-463IM3Q)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User ID=system;Password=Cy19991116;",
            DbType = DbType.Oracle,
            InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
            IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

        });
        //调式代码 用来打印SQL 
        Db.Aop.OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine(sql + "\r\n" +
                Db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
            Console.WriteLine();
        };

    }
    //注意：不能写成静态的
    public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
	public SimpleClient<T> CurrentDb { get { return new SimpleClient<T>(Db); } }//用来操作当前表的数据

   public SimpleClient<WORKERS> WORKERSDb { get { return new SimpleClient<WORKERS>(Db); } }//用来处理WORKERS表的常用操作
   public SimpleClient<TIME> TIMEDb { get { return new SimpleClient<TIME>(Db); } }//用来处理TIME表的常用操作
   public SimpleClient<CUSTOMER> CUSTOMERDb { get { return new SimpleClient<CUSTOMER>(Db); } }//用来处理CUSTOMER表的常用操作
   public SimpleClient<PLANE> PLANEDb { get { return new SimpleClient<PLANE>(Db); } }//用来处理PLANE表的常用操作
   public SimpleClient<FCREW> FCREWDb { get { return new SimpleClient<FCREW>(Db); } }//用来处理FCREW表的常用操作
   public SimpleClient<FLIGHTIME> FLIGHTIMEDb { get { return new SimpleClient<FLIGHTIME>(Db); } }//用来处理FLIGHTIME表的常用操作
   public SimpleClient<FLIGHT> FLIGHTDb { get { return new SimpleClient<FLIGHT>(Db); } }//用来处理FLIGHT表的常用操作
   public SimpleClient<FSTATE> FSTATEDb { get { return new SimpleClient<FSTATE>(Db); } }//用来处理FSTATE表的常用操作
   public SimpleClient<GROUND> GROUNDDb { get { return new SimpleClient<GROUND>(Db); } }//用来处理GROUND表的常用操作
   public SimpleClient<LUGGAGE> LUGGAGEDb { get { return new SimpleClient<LUGGAGE>(Db); } }//用来处理LUGGAGE表的常用操作
   public SimpleClient<SEAT> SEATDb { get { return new SimpleClient<SEAT>(Db); } }//用来处理SEAT表的常用操作
   public SimpleClient<TERMINAL> TERMINALDb { get { return new SimpleClient<TERMINAL>(Db); } }//用来处理TERMINAL表的常用操作
   public SimpleClient<USERS> USERSDb { get { return new SimpleClient<USERS>(Db); } }//用来处理USERS表的常用操作


   /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetList()
    {
        return CurrentDb.GetList();
    }

    /// <summary>
    /// 根据表达式查询
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetList(Expression<Func<T, bool>> whereExpression)
    {
        return CurrentDb.GetList(whereExpression);
    }


    /// <summary>
    /// 根据表达式查询分页
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel)
    {
        return CurrentDb.GetPageList(whereExpression, pageModel);
    }

    /// <summary>
    /// 根据表达式查询分页并排序
    /// </summary>
    /// <param name="whereExpression">it</param>
    /// <param name="pageModel"></param>
    /// <param name="orderByExpression">it=>it.id或者it=>new{it.id,it.name}</param>
    /// <param name="orderByType">OrderByType.Desc</param>
    /// <returns></returns>
    public virtual List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel pageModel, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return CurrentDb.GetPageList(whereExpression, pageModel,orderByExpression,orderByType);
    }


    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <returns></returns>
    public virtual List<T> GetById(dynamic id)
    {
        return CurrentDb.GetById(id);
    }

    /// <summary>
    /// 根据主键删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Delete(dynamic id)
    {
        return CurrentDb.Delete(id);
    }


    /// <summary>
    /// 根据实体删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Delete(T data)
    {
        return CurrentDb.Delete(data);
    }

    /// <summary>
    /// 根据主键删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Delete(dynamic[] ids)
    {
        return CurrentDb.AsDeleteable().In(ids).ExecuteCommand()>0;
    }

    /// <summary>
    /// 根据表达式删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Delete(Expression<Func<T, bool>> whereExpression)
    {
        return CurrentDb.Delete(whereExpression);
    }


    /// <summary>
    /// 根据实体更新，实体需要有主键
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Update(T obj)
    {
        return CurrentDb.Update(obj);
    }

    /// <summary>
    ///批量更新
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Update(List<T> objs)
    {
        return CurrentDb.UpdateRange(objs);
    }

    /// <summary>
    /// 插入
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Insert(T obj)
    {
        return CurrentDb.Insert(obj);
    }


    /// <summary>
    /// 批量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual bool Insert(List<T> objs)
    {
        return CurrentDb.InsertRange(objs);
    }


    //自已扩展更多方法 
}


