using CommunityToolkit.Mvvm.Messaging;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using ScadaSystem.Messages;
using ScadaSystem.Services;
using ScadaSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScadaSystem.Views
{
    /// <summary>
    /// ShellView.xaml 的交互逻辑
    /// </summary>
    public partial class ShellView : MetroWindow 
    {
      
        public ShellView()
        {
            InitializeComponent();
            InitData();
            ChangeViewToLogin();
           
        }
        /// <summary>
        /// 切换到登录界面
        /// </summary>
        private void ChangeViewToLogin()
        {
            //获取登录界面
            container.Content = App.Current.Services.GetService<LoginView>();
            //登陆成功后，切换到主界面 
            WeakReferenceMessenger.Default.Register<LoginMessage>(this, (r, m) => 
            {
                if (m.Value != null)
                {
                    container.Content = App.Current.Services.GetService<MainView>();
                    Width = 1200;
                    Height = 800;
                    SetWindowLocatiom();//设置窗口位置
                }
            });
            //登出消息切换
            WeakReferenceMessenger.Default.Register<LogoutMessage>(this, (r, m) =>
            {
                if (m.Value != null)
                {
                    


                    // 清空用户名和密码
                    var loginVM = App.Current.Services.GetService<LoginViewModel>();
                    loginVM?.ClearCredentials();


                    container.Content = App.Current.Services.GetService<LoginView>();

                    Width = 800;
                    Height = 450;
                    SetWindowLocatiom();
                }
            });

        }

        private void SetWindowLocatiom()
        {
            Left = (SystemParameters.WorkArea.Width - Width) /3;
            Top = (SystemParameters.WorkArea.Width - Width) / 3;
        }

        public void InitData()
        {
            DataContext = App.Current.Services.GetService<ShellViewModel>();
        }

    }
}
