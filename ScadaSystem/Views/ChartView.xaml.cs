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
    /// ChartView.xaml 的交互逻辑
    /// </summary>
    public partial class ChartView : UserControl
    {

        private readonly ChartViewModel _chartViewModel;
        public ChartView()
        {
            InitializeComponent();
            _chartViewModel= App.Current.Services.GetService<ChartViewModel>();
            DataContext = _chartViewModel;
            Loaded += ChartView_Loaded;
        }

        private void ChartView_Loaded(object sender, RoutedEventArgs e)
        {
            _chartViewModel.InitPlot(WpfPlotShow);
        }
    }
}
