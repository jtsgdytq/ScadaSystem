using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ScadaSystem.ViewModels
{
   public partial class DeviceViewModel:ObservableObject
    {
        [ObservableProperty]
        private ScadaReadData _scadaReadData = new ScadaReadData();
        
        [ObservableProperty]
        string _logContent = string.Empty;

        private readonly GrobalConfig _grobalConfig;
        private UserSession _userSession;

        public DeviceViewModel(GrobalConfig grobalConfig, UserSession userSession)
        {
            _grobalConfig = grobalConfig;
            _userSession = userSession;
            LogContent = $"程序运行中...{Environment.NewLine}程序启动成功...";
        }

        [RelayCommand]
        void DeviceControl(string ParamName )
        {
            if(!_grobalConfig.PLCConnnection)
            {
                _userSession.ShowMessage("PLC未连接");
                return;
            }

            var address = _grobalConfig.writeEntities.FirstOrDefault(x => x.En == ParamName)?.Address;
            if (address == null)
            {
                _userSession.ShowMessage($"未找到地址: {ParamName}");
                return;
            }

            var result = _grobalConfig.Plc.Write(address, true);
            if(result != null)
            {
                // 记录日志
                LogContent += $"写入{ParamName} 地址{address} 写入值:true{Environment.NewLine}";
            }
        }
        [RelayCommand]

        void ClearLog()
        {
            LogContent = string.Empty ;
        }
        /// <summary>
        /// 脱脂工位
        /// </summary>
        /// <param name="ParamName"></param>
        private bool CanToggleCollection()
        {
            return _grobalConfig.PLCConnnection;
        }

        [RelayCommand(CanExecute = nameof(CanToggleCollection))]
        void DegreasingStationOpen(string ParamName)
        {
            if (!_grobalConfig.PLCConnnection)
            {
                _userSession.ShowMessage("PLC未连接");
                return;
            }

            var address = _grobalConfig.writeEntities.FirstOrDefault(x => x.En == ParamName);
            if (address == null)
            {
                _userSession.ShowMessage($"未找到地址: {ParamName}");
                return;
            }

            bool value =(bool) ScadaReadData.GetType().GetRuntimeProperty(address.En)?.GetValue(ScadaReadData);
            var result = _grobalConfig.Plc.Write(address.Address, value);

            if (result.IsSuccess)
            {
                // 记录日志
                LogContent += $"写入{ParamName} 地址{address.Address} 写入值:{value}{Environment.NewLine}";
            }

        }
    }
}
