using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScadaSystem.Views
{
    /// <summary>
    /// ReportView.xaml 的交互逻辑
    /// </summary>
    public partial class ReportView : UserControl
    {
        public ReportView()
        {
            InitializeComponent();
            InitData();
        }

        public void InitData()
        {
            DataContext = App.Current.Services.GetService<ReportViewModel>();
        }
    }
}
