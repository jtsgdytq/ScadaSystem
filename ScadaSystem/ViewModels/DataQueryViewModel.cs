using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace ScadaSystem.ViewModels
{
    public partial class DataQueryViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ScadaReadData> _scadaReadDataList = new List<ScadaReadData>();
        [ObservableProperty]
        DateTime _startTime = DateTime.Now.AddDays(-30);
        [ObservableProperty]
        DateTime _endTime = DateTime.Now;

        /// <summary>
        /// 异步加载数据
        /// </summary>
        [RelayCommand]
        async Task Load()
        {
            // 从数据库查询数据并更新属性
            ScadaReadDataList = await Task.Run(() => SqlSugarHelper.Db.Queryable<ScadaReadData>().ToList());
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        [RelayCommand]
        void Search()
        {
            if(StartTime > EndTime)
            {
                MessageBox.Show("开始时间不能大于结束时间");
                ResetTime();
                return;
            }
            // 从数据库查询数据并更新属性
            ScadaReadDataList = SqlSugarHelper.Db.Queryable<ScadaReadData>()
                .Where(x => x.CreateTime >= StartTime && x.CreateTime <= EndTime)
                .ToList();

        }
        /// <summary>
        /// 重置时间
        /// </summary>
        [RelayCommand]
        void ResetTime()
        {
            StartTime = DateTime.Now.AddDays(-30);
            EndTime = DateTime.Now;
        }
    }
}