using CommunityToolkit.Mvvm.ComponentModel;
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
        }
    }
}
