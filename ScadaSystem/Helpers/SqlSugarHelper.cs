using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Helpers
{
    public static class SqlSugarHelper
    {
        public static  SqlSugarScope Db {  get; set; }

        public static void AddSqlSugarSetup(DbType dbType,string connectionString)
        {
            Db = new SqlSugarScope(new ConnectionConfig()
            {
                ////数据库连接配置
                ConnectionString = connectionString,//连接字符串
                DbType = dbType,//数据库类型
                IsAutoCloseConnection = true //不设成true要手动close
            },
            db =>
            {
                //(A)全局生效配置点，一般AOP和程序启动的配置扔这里面 ，所有上下文生效
                //调试SQL事件，可以删掉
                db.Aop.OnLogExecuting = (sql, pars) =>
                {

                    //获取无参数化SQL 对性能有影响，特别大的SQL参数多的，调试使用
                    Console.WriteLine(UtilMethods.GetSqlString(DbType.SqlServer, sql, pars));

                };

                //多个配置就写下面
                //db.Ado.IsDisableMasterSlaveSeparation=true;

                //注意多租户 有几个设置几个
                //db.GetConnection(i).Aop
            });
        }


        
    }
}
