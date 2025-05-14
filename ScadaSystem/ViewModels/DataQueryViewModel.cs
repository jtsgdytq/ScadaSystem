using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiniExcelLibs;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ScadaSystem.ViewModels
{
    public partial class DataQueryViewModel : ObservableObject
    {
        UserSession _userSession;
        public DataQueryViewModel(UserSession userSession)
        {
            _userSession = userSession;
            Load();
        }

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
        void Load()
        {
            // 从数据库查询数据并更新属性
            int totalNumber = 1;
            ScadaReadDataList = SqlSugarHelper.Db.Queryable<ScadaReadData>()
                   .ToPageList(CurrentPage, PageSize, ref totalNumber);
            Search();
        }
        /// <summary>
        /// 数据查询
        /// </summary>
        [RelayCommand]
        void Search()
        {
            if(StartTime > EndTime)
            {
                _userSession.ShowMessage("开始时间不能大于结束时间");
                ResetTime();
                return;
            }
            // 从数据库查询数据并更新属性
            int totalNumber = 1;
            ScadaReadDataList = SqlSugarHelper.Db.Queryable<ScadaReadData>()
                   .Where(x => x.CreateTime >= StartTime && x.CreateTime <= EndTime)
                   .ToPageList(CurrentPage, PageSize, ref totalNumber);
            // 计算总页数
            TotalPages = (int)Math.Ceiling((double)totalNumber / PageSize);

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

        #region 实现分页
        /// <summary>
        /// 默认显示一页20条数据
        /// </summary>
        [ObservableProperty]
        private int _pageSize = 20; // 每页显示的记录数
        /// <summary>
        /// 当前页码
        /// </summary>
        [ObservableProperty]
        private int _currentPage=1;

        /// <summary>
        /// 当前页码改变时，重新加载数据
        /// </summary>
        /// <param name="value"></param>
        partial void OnCurrentPageChanged(int value)
        {
            //当前页码改变时，重新加载数据
            Search();
        }

        /// <summary>
        /// 总页码
        /// </summary>
        [ObservableProperty]
        private int _totalPages;

        

        /// <summary>
        /// 跳转到第一页
        /// </summary>
        [RelayCommand]
        public void GoToFirstPage()
        {
            CurrentPage = 1;
        }
        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        [RelayCommand]
        public void GoToLastPage()
        {
            CurrentPage = TotalPages;
        }

        [RelayCommand]
        public void GoToNextPage()
        {
            if(CurrentPage>=1 && CurrentPage < TotalPages)
            {
                CurrentPage++;
            }
        }
        [RelayCommand]
        public void GoToPreviousPage()
        {
            if (CurrentPage > 1&&CurrentPage<TotalPages)
            {
                CurrentPage--;
            }
        }


        #endregion

        #region 实现导出Excel
        [RelayCommand]
        void OutPage()
        {
            ExportToExcel(ScadaReadDataList);
        }

        [RelayCommand]
        void OutAllPage()
        {
            // 导出所有数据,谨慎使用，以防止数据量过大
            var list = SqlSugarHelper.Db.Queryable<ScadaReadData>()
                .ToList();
            ExportToExcel(list);
        }

        private void ExportToExcel<T>(List<T> dataList)
        {
            // 实现导出Excel的逻辑
            // 这里可以使用EPPlus、NPOI等库来实现Excel导出
            if (dataList == null || dataList.Count == 0)
            {
                return;
            }
            //获取当前应用程序的运行目录，导出的文件就会保存在这个路径下。

            //对于 WPF 桌面程序，这通常是 bin\Debug\netX 或发布目录。
           var rootPath=AppDomain.CurrentDomain.BaseDirectory;

            //在当前目录下创建一个名为 ExportedExcel 的文件夹
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            //创建一个 Excel 文件名，文件名为当前时间戳加上 .xlsx 后缀
            var excelPath = rootPath + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            try
            {
                MiniExcel.SaveAs(excelPath, dataList);
               MessageBox.Show("导出成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                // 打开导出的Excel文件
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = excelPath,
                    UseShellExecute = true
                });
            }
            catch (Exception e)
            {
                MessageBox.Show("导出失败", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        #endregion
    }
}