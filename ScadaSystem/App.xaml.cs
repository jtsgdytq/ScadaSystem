using ControlzEx.Theming;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.Services;
using ScadaSystem.ViewModels;
using ScadaSystem.Views;
using SqlSugar;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ScadaSystem;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public new static App Current => (App)Application.Current;

    public IServiceProvider Services { get; private set; }

    public App()
    {
        Services = ConfigureService();
        this.InitializeComponent();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        //设置启动窗口
        Services.GetService<ShellView>().Show();
        // 切换主题，变为 AliceBlue
        ThemeManager.Current.ChangeTheme(this, ThemeManager.Current.AddTheme(
            RuntimeThemeGenerator.Current.GenerateRuntimeTheme("Light", Colors.AliceBlue)
            ));
    }

    /// <summary>
    /// 统一进行依赖注入的地方
    /// </summary>
    /// <returns></returns>
    private IServiceProvider ConfigureService()
    {
        var services = new ServiceCollection();
       

        // 配置类实现
        ConfigureJsonByBinder(services);
        /// <summary>
        ///  注册UI层
        //shell主界面
        services.AddSingleton<ShellView>();
        services.AddSingleton<ShellViewModel>();
        //主界面
        services.AddSingleton<MainView>();
        services.AddSingleton<MainViewModel>();
        //登录界面
        services.AddSingleton<LoginView>();
        services.AddSingleton<LoginViewModel>();
        //设备界面
        services.AddSingleton<DeviceView>();
        services.AddSingleton<DeviceViewModel>();
        //首页界面
        services.AddSingleton<IndexView>();
        services.AddSingleton<IndexViewModel>();
        
        services.AddSingleton<FormulaView>();
        services.AddSingleton<FormulaViewModel>();

        services.AddSingleton<ParamsView>();
        services.AddSingleton<ParamsViewModel>();

        services.AddSingleton<DataQueryView>();
        services.AddSingleton<DataQueryViewModel>();
        //ChartView
        services.AddSingleton<ChartView>();
        services.AddSingleton<ChartViewModel>();
        //ReportView
        services.AddSingleton<ReportView>();
        services.AddSingleton<ReportViewModel>();
        //LogView
        services.AddSingleton<LogView>();
        services.AddSingleton<LogViewModel>();
        //User
        services.AddSingleton<UserView>();
        services.AddSingleton<UserViewModel>();

        //注册服务层
        services.AddSingleton<UserSession>();

        //PLC数据
        services.AddSingleton<GrobalConfig>();

        return services.BuildServiceProvider();
    }

    private void ConfigureJsonByBinder(ServiceCollection services)
    {
        //使用 ConfigurationBuilder 加载 appsettings.json
        var cfgBuilder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory+"\\Configs")
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = cfgBuilder.Build();
        //将 IConfiguration 注册到 DI 容器中
        services.AddSingleton<IConfiguration>(configuration).AddLogging(log =>
        {
            log.ClearProviders();
            log.SetMinimumLevel(LogLevel.Trace);
            log.AddNLog();
        });
        //获取日志中的参数
        var nlogConfig = configuration.GetSection("NLog");
        //将 NLog 配置绑定到 NLog.Config.LoggingConfiguration
        LogManager.Configuration = new NLog.Config.LoggingConfiguration();

        //配置数据库
        var parsResult = Enum.TryParse<SqlSugar.DbType>(configuration["SqlParam:DbType"], out var result);
        var connectionString = configuration["SqlParam:ConnectionString"];
        if(parsResult)
        {
            SqlSugarHelper.AddSqlSugarSetup(result, connectionString);
        }
        
        services.AddOptions()
            .Configure<RootParam>(e => configuration.Bind(e))
            .Configure<SqlParam>(e => configuration.GetSection("SqlParam").Bind(e))
            .Configure<SystemParam>(e => configuration.GetSection("SystemParam").Bind(e))
            .Configure<PlcParam>(e => configuration.GetSection("PlcParam").Bind(e));



    }
}

