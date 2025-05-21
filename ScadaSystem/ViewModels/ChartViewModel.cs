using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScottPlot;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.ViewModels
{
   public partial class ChartViewModel:ObservableObject
    {
        private WpfPlot _plot;

        [ObservableProperty]
        DateTime _startTime = DateTime.Now.AddDays(-30);
        [ObservableProperty]
        DateTime _endTime= DateTime.Now;
        public void InitPlot(WpfPlot plot)
        {
            _plot = plot;
            ConfigurePlot();
        }

        private void ConfigurePlot()
        {
            if (_plot == null) return;
            _plot.Plot.Title ( "ScadaData Show");
            _plot.Plot.XLabel("Point");
            _plot.Plot.YLabel("Value");
            _plot.Plot.ShowLegend();
        }
        [RelayCommand]
        void Search()
        {
            if (StartTime > EndTime) return;
            _plot.Plot.Clear();
            try
            {
                var data = SqlSugarHelper.Db.Queryable<ScadaReadData>()
               .Where(x => x.CreateTime > StartTime && x.CreateTime <= EndTime)
               .OrderBy(x => x.CreateTime, SqlSugar.OrderByType.Asc)
               .ToList();
                if (data.Count > 0)
                {
                    // 4. 将数据添加到 plot 中
                    var degreasingSprayPumpPressureY = data.Select(x => x.DegreasingSprayPumpPressure).ToArray();
                    var degreasingPhValueY = data.Select(x => x.DegreasingPhValue).ToArray();

                    // 5. 设置线条样式
                    List<LinePattern> patterns = [];
                    patterns.AddRange(LinePattern.GetAllPatterns());

                    // 6. 添加数据
                    var sig1 = _plot.Plot.Add.Signal(degreasingSprayPumpPressureY);
                    sig1.LineWidth = 3;
                    sig1.LegendText = "DegreasingSprayPumpPressure";
                    sig1.LinePattern = patterns[1]; // Dotted

                    var sig2 = _plot.Plot.Add.Signal(degreasingPhValueY);
                    sig2.LineWidth = 3;
                    sig2.LegendText = "DegreasingPhValueY";
                    sig2.LinePattern = patterns[3]; // Dashed

                    

                }

                // 7. 缩放
                _plot.Plot.ScaleFactor = 2;
                // 8. 刷新显示
                _plot.Refresh();
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
