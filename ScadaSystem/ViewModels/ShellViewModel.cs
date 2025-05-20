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


        private void InitData()
        {
            if (false)
            {
                // 建库
                //SqlSugarHelper.Db.DbMaintenance.CreateDatabase();
                // 建表
                //SqlSugarHelper.Db.CodeFirst.InitTables<User>();
                //SqlSugarHelper.Db.CodeFirst.InitTables<Menu>();
                //SqlSugarHelper.Db.CodeFirst.InitTables<ScadaReadData>();
               // SqlSugarHelper.Db.CodeFirst.InitTables<FormulaEntity>();
            }

            if (false)
            {
                var menuList = new List<Menu>();
                menuList.Add(new Menu()
                {
                    MenuName = "首页",
                    Icon = "Home",
                    View = "IndexView",
                    Sort = 1,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "设备总控",
                    Icon = "Devices",
                    View = "DeviceView",
                    Sort = 2,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "配方管理",
                    Icon = "AirFilter",
                    View = "FormulaView",
                    Sort = 3,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "参数管理",
                    Icon = "AlphabetCBoxOutline",
                    View = "ParamsView",
                    Sort = 4,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "数据查询",
                    Icon = "DataUsage",
                    View = "DataQueryView",
                    Sort = 5,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "数据趋势",
                    Icon = "ChartFinance",
                    View = "ChartView",
                    Sort = 6,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "报表管理",
                    Icon = "FileReport",
                    View = "ReportView",
                    Sort = 7,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "日志管理",
                    Icon = "NotebookOutline",
                    View = "LogView",
                    Sort = 8,
                });
                menuList.Add(new Menu()
                {
                    MenuName = "用户管理",
                    Icon = "UserMultipleOutline",
                    View = "UserView",
                    Sort = 9,
                });

                SqlSugarHelper.Db.Insertable(menuList).ExecuteCommand();
            }


            if (false)
            {
                // 插入一些用户表里面的数据 root test test123

                var userList = new List<User>();
                userList.Add(new User
                {
                    UserName = "root",
                    Password = "root",
                    Role = 0
                });
                userList.Add(new User
                {
                    UserName = "test",
                    Password = "test",
                    Role = 1
                });
                userList.Add(new User
                {
                    UserName = "test123",
                    Password = "test123",
                    Role = 1
                });

                SqlSugarHelper.Db.Insertable(userList).ExecuteCommand();
            }


            if (false)
            {
                // 插入一些用户表里面的数据 root test test123

                //var scadaReadDatalist = new List<ScadaReadData>();

                //for (int i = 0; i < 100; i++)
                //{
                //    var scadaReadData = new ScadaReadData()
                //    {
                //        DegreasingSprayPumpPressure = GetRandomFloat(0.5f, 5.0f),
                //        DegreasingPhValue = GetRandomFloat(6.0f, 9.0f),
                //        RoughWashSprayPumpPressure = GetRandomFloat(1.0f, 4.0f),
                //        PhosphatingSprayPumpPressure = GetRandomFloat(0.8f, 3.5f),
                //        PhosphatingPhValue = GetRandomFloat(4.0f, 7.0f),
                //        FineWashSprayPumpPressure = GetRandomFloat(1.2f, 4.5f),
                //        MoistureFurnaceTemperature = GetRandomFloat(40.0f, 80.0f),
                //        CuringFurnaceTemperature = GetRandomFloat(120.0f, 200.0f),
                //        FactoryTemperature = GetRandomFloat(15.0f, 35.0f),
                //        FactoryHumidity = GetRandomFloat(30.0f, 80.0f),
                //        ProductionCount = GetRandomFloat(0, 1000),
                //        DefectiveCount = GetRandomFloat(0, 50),
                //        ProductionPace = GetRandomFloat(0.5f, 2.0f),
                //        AccumulatedAlarms = GetRandomFloat(0, 20),
                //        CreateTime = DateTime.Now.AddDays(GetRandomFloat(1f, 10f)),
                //        UpdateTime = DateTime.Now.AddDays(GetRandomFloat(1f, 10f))
                //    };

                //    scadaReadDatalist.Add(scadaReadData);
                //}


                //SqlSugarHelper.Db.Insertable(scadaReadDatalist).ExecuteCommand();
            }

            if (false)
            {
                // 插入一些用户表里面的数据 root test test123
                var formulaEntityList = new List<FormulaEntity>();

                for (int i = 0; i < 10; i++)
                {
                    var formulaEntity = new FormulaEntity()
                    {
                        Name = "配方" + i,
                        Description = "配方描述" + i,
                        IsSelected = false,
                        DegreasingSetPressureUpperLimit
                            = GetRandomFloat(0.5f, 5.0f),
                        DegreasingSetPressureLowerLimit
                            = GetRandomFloat(6.0f, 9.0f),
                        RoughWashingSprayPumpOverloadUpperLimit
                            = GetRandomFloat(1.0f, 4.0f),
                        RoughWashingLevelLowerLimit
                            = GetRandomFloat(0.8f, 3.5f),
                        CeramicCoatingSprayPumpOverloadUpperLimit
                            = GetRandomFloat(4.0f, 7.0f),
                        FineWashingSprayPumpOverloadUpperLimit
                            = GetRandomFloat(1.2f, 4.5f),
                        FineWashingLevelLowerLimit
                            = GetRandomFloat(40.0f, 80.0f),
                        MoistureFurnaceTemperatureUpperLimit
                            = GetRandomFloat(120.0f, 200.0f),
                        MoistureFurnaceTemperatureLowerLimit
                            = GetRandomFloat(120.0f, 200.0f),
                        CoolingRoomCentrifugalFanOverloadUpperLimit
                            = GetRandomFloat(120.0f, 200.0f),
                        CuringOvenTemperatureUpperLimit
                            = GetRandomFloat(120.0f, 200.0f),
                        CuringOvenTemperatureLowerLimit
                            = GetRandomFloat(120.0f, 200.0f),
                        ConveyorSetSpeed = GetRandomFloat(120.0f, 200.0f),
                        ConveyorSetFrequency = GetRandomFloat(120.0f, 200.0f),

                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                    };
                    formulaEntityList.Add(formulaEntity);
                }

                SqlSugarHelper.Db.Insertable(formulaEntityList).ExecuteCommand();
            }
        }

        private float GetRandomFloat(float min, float max)
        {
            return (float)(new Random().NextDouble() * (max - min) + min);
        }
    






        #region  
        /// <summary>
        /// 初始化时创建数据库
        /// </summary>
        private void initData()
        {
            //SeedAsync().Wait();




        }

        private async Task SeedAsync()
        {
            // 1. 初始化表（如果不存在则创建）
            SqlSugarHelper.Db.CodeFirst.InitTables<ScadaReadData>();

            // 2. 准备随机数生成器
            Random rand = new Random();

            // 3. 生成随机数据列表
            List<ScadaReadData> ScadaDataList = new List<ScadaReadData>();

            for (int i = 0; i < 50; i++) // 生成 50 条数据
            {
                var data = new ScadaReadData
                {
                    DegreasingSprayPumpPressure = GetRandomFloat(rand, 0.5f, 2.5f),
                    DegreasingPhValue = GetRandomFloat(rand, 7.0f, 11.0f),
                    RoughWashSprayPumpPressure = GetRandomFloat(rand, 0.5f, 2.5f),
                    PhosphatingSprayPumpPressure = GetRandomFloat(rand, 0.5f, 2.5f),
                    PhosphatingPhValue = GetRandomFloat(rand, 3.5f, 5.5f),
                    FineWashSprayPumpPressure = GetRandomFloat(rand, 0.5f, 2.5f),
                    MoistureFurnaceTemperature = GetRandomFloat(rand, 80f, 120f),
                    CuringFurnaceTemperature = GetRandomFloat(rand, 150f, 200f),
                    FactoryTemperature = GetRandomFloat(rand, 15f, 35f),
                    FactoryHumidity = GetRandomFloat(rand, 30f, 80f),
                    CreateTime = DateTime.Now.AddMinutes(-rand.Next(0, 1000)), // 模拟历史数据
                    UpdateTime = DateTime.Now
                };

                ScadaDataList.Add(data);
            }

            // 4. 批量插入到数据库
            int result = await SqlSugarHelper.Db.Insertable(ScadaDataList).ExecuteCommandAsync();

            Console.WriteLine($"已插入 {result} 条随机SCADA数据");
        }

        private float GetRandomFloat(Random rand, float min, float max)
        {
            return (float)(rand.NextDouble() * (max - min) + min);
        }
    }

    #region
    //if (false)
    //{
    //    SqlSugarHelper.Db.CodeFirst.InitTables<Menu>();
    //    var menuList = new List<Menu>();
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "首页",
    //        Icon = "Home",
    //        View = "IndexView",
    //        Role = 0,
    //        Sort = 1,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "设备总控",
    //        Icon = "Devices",
    //        View = "DeviceView",
    //        Role = 0,
    //        Sort = 2,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "配方管理",
    //        Icon = "AirFilter",
    //        View = "FormulaView",
    //        Role = 0,
    //        Sort = 3,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "参数管理",
    //        Icon = "AlphabetCBoxOutline",
    //        View = "ParamsView",
    //        Role = 0,
    //        Sort = 4,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "数据查询",
    //        Icon = "DataUsage",
    //        View = "DataQueryView",
    //        Role = 0,
    //        Sort = 5,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "数据趋势",
    //        Icon = "ChartFinance",
    //        View = "ChartView",
    //        Role = 0,
    //        Sort = 6,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "报表管理",
    //        Icon = "FileReport",
    //        View = "ReportView",
    //        Role = 0,
    //        Sort = 7,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "日志管理",
    //        Icon = "NotebookOutline",
    //        View = "LogView",
    //        Role = 0,
    //        Sort = 8,
    //    });
    //    menuList.Add(new Menu()
    //    {
    //        MenuName = "用户管理",
    //        Icon = "UserMultipleOutline",
    //        View = "UserView",
    //        Role = 0,
    //        Sort = 9,
    //    });


    // SqlSugarHelper.Db.Insertable(menuList).ExecuteCommand();



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

    #endregion
}






