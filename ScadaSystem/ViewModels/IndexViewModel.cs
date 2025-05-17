using CommunityToolkit.Mvvm.ComponentModel;
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

   public partial class IndexViewModel:ObservableObject
    {

        private readonly GrobalConfig _grobalConfig;
        private readonly RootParam _rootParam;
        [ObservableProperty]

        ScadaReadData _scadaReadData=new ScadaReadData();
        private CancellationTokenSource _cts = new();

        public IndexViewModel(GrobalConfig grobalConfig, IOptionsSnapshot<RootParam> options)
        {
            _grobalConfig = grobalConfig;
            _rootParam = options.Value; // Initialize _rootParam first
            StartColletion();
            InitReadDataDic2ScadaReadDataProp();
        }

        /// <summary>
        /// 自动采集判断
        /// </summary>
        private void StartColletion()
        {
            _grobalConfig.InitPLCServer();
            if (_rootParam.PlcParam.AutoCollect)
            {
                _grobalConfig.StartCollectionAsync();
            }
            //_grobalConfig.StartCollectionAsync();
        }
           

        private void InitReadDataDic2ScadaReadDataProp()
        {
            Task.Run(async () => {

                var properties = typeof(ScadaReadData)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(float) || p.PropertyType == typeof(bool));

                while (!_cts.Token.IsCancellationRequested)
                {
                    foreach (var property in properties)
                    {
                        try
                        {
                            if (property.PropertyType == typeof(float))
                            {
                                var value = _grobalConfig.GetValue<float>(property.Name);
                                property.SetValue(_scadaReadData, value);
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                var value = _grobalConfig.GetValue<bool>(property.Name);
                                property.SetValue(_scadaReadData, value);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                        }
                    }

                    await Task.Delay(100);
                }
            });
        }
    }

    
}
