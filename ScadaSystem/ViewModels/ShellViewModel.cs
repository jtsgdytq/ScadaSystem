using CommunityToolkit.Mvvm.ComponentModel;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.ViewModels
{
    public partial class ShellViewModel : ObservableObject
    {
        public ShellViewModel()
        {
            InitData();
        }
        /// <summary>
        /// 初始化时创建数据库
        /// </summary>
        private void InitData()
        {
            if (false)
            {
                SqlSugarHelper.Db.CodeFirst.InitTables<Menu>();
                var menuList = new List<Menu>();
                menuList.Add(new Menu()
                {
                    MenuName = "首页",
                    Icon = "Home",
                    View = "IndexView",
                    Role = 0,
                    Sort = 1,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "设备总控",
                    Icon = "Devices",
                    View = "DeviceView",
                    Role = 0,
                    Sort = 2,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "配方管理",
                    Icon = "AirFilter",
                    View = "FormulaView",
                    Role = 0,
                    Sort = 3,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "参数管理",
                    Icon = "AlphabetCBoxOutline",
                    View = "ParamsView",
                    Role = 0,
                    Sort = 4,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "数据查询",
                    Icon = "DataUsage",
                    View = "DataQueryView",
                    Role = 0,
                    Sort = 5,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "数据趋势",
                    Icon = "ChartFinance",
                    View = "ChartView",
                    Role = 0,
                    Sort = 6,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "报表管理",
                    Icon = "FileReport",
                    View = "ReportView",
                    Role = 0,
                    Sort = 7,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "日志管理",
                    Icon = "NotebookOutline",
                    View = "LogView",
                    Role = 0,
                    Sort = 8,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "用户管理",
                    Icon = "UserMultipleOutline",
                    View = "UserView",
                    Role = 0,
                    Sort = 9,
                });


                SqlSugarHelper.Db.Insertable(menuList).ExecuteCommand();
            }

            #region 
            //SqlSugarHelper.Db.DbMaintenance.DropTable("User");
            //SqlSugarHelper.Db.CodeFirst.InitTables(typeof(User));

            //if (false)
            //{ //创建数据库
            //    SqlSugarHelper.Db.DbMaintenance.CreateDatabase();
            //    创建表
            //    SqlSugarHelper.Db.CodeFirst.InitTables(new Type[] { typeof(Models.User) });
            //    添加默认数据 }

            //}
            //添加默认数据  0:管理员 1:普通用户
            //var user = new Models.User()//管理员
            //{
            //    UserName = "admin",
            //    Password = "123456",
            //    Role = 0,

            //};
            //var user2 = new Models.User()//普通用户
            //{
            //    UserName = "user",
            //    Password = "123456",
            //    Role = 1,

            //};
            //var user3 = new Models.User()
            //{
            //    UserName = "zhangasn",
            //    Password = "123456",
            //    Role = 0,

            //};
            // SqlSugarHelper.Db.Insertable(user).ExecuteCommand();
            //SqlSugarHelper.Db.Insertable(user2).ExecuteCommand();
            #endregion
        }
    }
}
  
