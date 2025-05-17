using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.ViewModels
{
    public partial class ParamsViewModel : ObservableObject
    {
        [ObservableProperty]
        RootParam _rootParamProp;
        GrobalConfig _globalConfig;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        public ParamsViewModel(IOptionsSnapshot<RootParam> optionsSnapshot, GrobalConfig grobalConfig)
        {
            RootParamProp = optionsSnapshot.Value;
            _globalConfig = grobalConfig;
        }

        [RelayCommand]
        void AutoCollection()
        {
            if (RootParamProp.PlcParam.AutoCollect)
            {
                _globalConfig.StopCollection();
                _globalConfig.StartCollectionAsync();
            }
            else
            {
                _globalConfig.StopCollection();
            }
        }


        /// <summary>
        /// _globalConfig.ReadDataDic 里写入随机数据
        /// </summary>

        [RelayCommand]
        void AutoMock()
        {
            if (RootParamProp.PlcParam.AutoMock)
            {
                StartMockData(); // 不再写 PLC，而是直接写 ReadDataDic
            }
            else
            {
                StopMockData();
            }
        }

        private void StopMockData()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void StartMockData()
        {
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var propertiesFloat = typeof(ScadaReadData).GetProperties()
                    .Where(p => p.PropertyType == typeof(float));
                var propertiesBool = typeof(ScadaReadData).GetProperties()
                    .Where(p => p.PropertyType == typeof(bool));

                var random = new Random();

                while (!_cts.Token.IsCancellationRequested)
                {
                    foreach (var prop in propertiesFloat)
                    {
                        var value = (float)(random.NextDouble() * 100); // 生成随机 float
                        _globalConfig.ReadDataDic.AddOrUpdate(prop.Name, value, (key, oldValue) => value);
                    }

                    foreach (var prop in propertiesBool)
                    {
                        var value = random.Next(0, 2) == 0 ? false : true; // 随机 bool
                        _globalConfig.ReadDataDic.AddOrUpdate(prop.Name, value, (key, oldValue) => value);
                    }

                    await Task.Delay(1000); // 每秒模拟一次
                }
            }, _cts.Token);
        }

        #region 模拟参数
      
        //private Task _mockTask;

        //[RelayCommand]
        //void AutoMock()
        //{
        //    if (RootParamProp?.PlcParam?.AutoMock == true)
        //    {
        //        StartMockData();
        //    }
        //    else
        //    {
        //        StopMockData();
        //    }
        //}

        //private void StartMockData()
        //{
        //    // 若已有模拟任务在运行，则跳过
        //    if (_mockTask != null && !_mockTask.IsCompleted)
        //        return;

        //    _cts = new CancellationTokenSource();
        //    var token = _cts.Token;

        //    _mockTask = Task.Run(async () =>
        //    {
        //        try
        //        {
        //            var scadaType = typeof(ScadaReadData);
        //            var floatProps = scadaType.GetProperties().Where(p => p.PropertyType == typeof(float)).ToList();
        //            var boolProps = scadaType.GetProperties().Where(p => p.PropertyType == typeof(bool)).ToList();
        //            var random = new Random();

        //            while (!token.IsCancellationRequested)
        //            {
        //                foreach (var prop in floatProps)
        //                {
        //                    try
        //                    {
        //                        var value = (float)(random.NextDouble() * 4 + 1);
        //                        var address = _globalConfig.readEntities.FirstOrDefault(x => x.En == prop.Name)?.Address;

        //                        if (!string.IsNullOrEmpty(address))
        //                        {
        //                            await _globalConfig.Plc.WriteAsync(address, value);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Debug.WriteLine($"写入 float 属性 {prop.Name} 出错: {ex.Message}");
        //                    }
        //                }

        //                foreach (var prop in boolProps)
        //                {
        //                    try
        //                    {
        //                        var value = random.Next(0, 2) == 1;
        //                        var address = _globalConfig.readEntities.FirstOrDefault(x => x.En == prop.Name)?.Address;

        //                        if (!string.IsNullOrEmpty(address))
        //                        {
        //                            await _globalConfig.Plc.WriteAsync(address, value);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Debug.WriteLine($"写入 bool 属性 {prop.Name} 出错: {ex.Message}");
        //                    }
        //                }

        //                await Task.Delay(1000, token);
        //            }
        //        }
        //        catch (TaskCanceledException)
        //        {
        //            // 正常退出
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"模拟数据线程出错: {ex.Message}");
        //        }
        //    }, token);
        //}

        //private void StopMockData()
        //{
        //    if (_cts != null && !_cts.IsCancellationRequested)
        //    {
        //        _cts.Cancel();
        //        _cts.Dispose();
        //        _cts = null;
        //    }

        //    _mockTask = null;
        //}

        #endregion
    }
}
