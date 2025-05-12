using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using ScadaSystem.Models;
using ScadaSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScadaSystem.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : System.Windows.Controls.UserControl
    {
        public MainView()
        {
            InitializeComponent();
            InitData();
            InitRegisterMessage();

        }
        /// <summary>
        /// 接收消息，实现窗口切换
        /// </summary>
        private void InitRegisterMessage()
        {
            Page.Content = App.Current.Services.GetService<IndexView>();

            WeakReferenceMessenger.Default.Register<Menu>(this, (sender, arg) =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var type = assembly.GetType($"{assembly.GetName().Name}.Views.{arg.View}");

                if (type != null)
                {
                    Page.Content = App.Current.Services.GetService(type);
                }
            });
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitData()
        {
           DataContext= App.Current.Services.GetService<MainViewModel>();
        }
    }
}
