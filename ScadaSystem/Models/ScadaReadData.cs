using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Models
{
    public class ScadaReadData:EntityBase
    {
        private float _degreasingSprayPumpPressure;
        /// <summary>
        /// 脱脂喷淋泵压力值
        /// </summary>
        public float DegreasingSprayPumpPressure
        {
            get => _degreasingSprayPumpPressure;
            set => SetProperty(ref _degreasingSprayPumpPressure, value);
        }

        private float _degreasingPhValue;
        /// <summary>
        /// 脱脂pH值
        /// </summary>
        public float DegreasingPhValue
        {
            get => _degreasingPhValue;
            set => SetProperty(ref _degreasingPhValue, value);
        }

        private float _roughWashSprayPumpPressure;
        /// <summary>
        /// 粗洗喷淋泵压力值
        /// </summary>
        public float RoughWashSprayPumpPressure
        {
            get => _roughWashSprayPumpPressure;
            set => SetProperty(ref _roughWashSprayPumpPressure, value);
        }

        private float _phosphatingSprayPumpPressure;
        /// <summary>
        /// 陶化喷淋泵压力值
        /// </summary>
        public float PhosphatingSprayPumpPressure
        {
            get => _phosphatingSprayPumpPressure;
            set => SetProperty(ref _phosphatingSprayPumpPressure, value);
        }

        private float _phosphatingPhValue;
        /// <summary>
        /// 陶化pH值
        /// </summary>
        public float PhosphatingPhValue
        {
            get => _phosphatingPhValue;
            set => SetProperty(ref _phosphatingPhValue, value);
        }

        private float _fineWashSprayPumpPressure;
        /// <summary>
        /// 精洗喷淋泵压力值
        /// </summary>
        public float FineWashSprayPumpPressure
        {
            get => _fineWashSprayPumpPressure;
            set => SetProperty(ref _fineWashSprayPumpPressure, value);
        }

        private float _moistureFurnaceTemperature;
        /// <summary>
        /// 水分炉测量温度
        /// </summary>
        public float MoistureFurnaceTemperature
        {
            get => _moistureFurnaceTemperature;
            set => SetProperty(ref _moistureFurnaceTemperature, value);
        }

        private float _curingFurnaceTemperature;
        /// <summary>
        /// 固化炉测量温度
        /// </summary>
        public float CuringFurnaceTemperature
        {
            get => _curingFurnaceTemperature;
            set => SetProperty(ref _curingFurnaceTemperature, value);
        }

        private float _factoryTemperature;
        /// <summary>
        /// 厂内温度
        /// </summary>
        public float FactoryTemperature
        {
            get => _factoryTemperature;
            set => SetProperty(ref _factoryTemperature, value);
        }

        private float _factoryHumidity;
        /// <summary>
        /// 厂内湿度
        /// </summary>
        public float FactoryHumidity
        {
            get => _factoryHumidity;
            set => SetProperty(ref _factoryHumidity, value);
        }
    }
}
