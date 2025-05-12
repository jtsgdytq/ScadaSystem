using ControlzEx.Theming;
using Microsoft.Extensions.DependencyInjection;
using ScadaSystem.Services;
using ScadaSystem.ViewModels;
using ScadaSystem.Views; 
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

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
        return services.BuildServiceProvider();
    }
}

