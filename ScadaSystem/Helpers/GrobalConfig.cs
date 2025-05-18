using HslCommunication.Profinet.Siemens;
using Microsoft.Extensions.Options;
using MiniExcelLibs;

using ScadaSystem.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HslCommunication;
using Microsoft.Extensions.Logging;

namespace ScadaSystem.Helpers
{
    public class GrobalConfig : IDisposable
    {
        
        public SiemensS7Net Plc;
        /// <summary>
        /// 公开字典
        /// </summary>
        public ConcurrentDictionary <string,object> ReadDataDic {  get; set; }=new ConcurrentDictionary<string,object>();
       

        public List<ReadEntity> readEntities { get; set; }= new List<ReadEntity>();
        public List<WriteEntity> writeEntities { get; set; } = new List<WriteEntity>();

        private readonly IOptionsSnapshot<RootParam> options;
        private readonly ILogger logger;

        private  CancellationTokenSource _cts= new CancellationTokenSource();
        /// <summary>
        /// PLC连接状态
        /// </summary>
        public bool PLCConnnection { get; set; } 


    public GrobalConfig(IOptionsSnapshot<RootParam> _options, ILogger<GrobalConfig> logger)
        {
            this.logger = logger;
            options = _options;

            InitPLC();//初始化PLC

            InitExcelsAddress();
            
        }

        private void InitExcelsAddress()
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory + "Configs\\";
            var readPath = rootPath + "TulingRead.xlsx";
            var writePath = rootPath + "TulingWrite.xlsx";

            if (!File.Exists(readPath) || !File.Exists(writePath))
            {
                logger.LogError($"找不到读/写文件路径: {readPath}\n{writePath}");
                return;
            }

            try
            {
                // ✅ 修复点：检查字段是否为空，而不是路径是否为空
                readEntities = MiniExcel.Query<ReadEntity>(readPath)
                    .Where(x => !string.IsNullOrWhiteSpace(x.En) && !string.IsNullOrWhiteSpace(x.Address))
                    .ToList();

                writeEntities = MiniExcel.Query<WriteEntity>(writePath)
                    .Where(x => !string.IsNullOrWhiteSpace(x.En) && !string.IsNullOrWhiteSpace(x.Address))
                    .ToList();

                logger.LogInformation($"成功加载读实体：{readEntities.Count} 条，写实体：{writeEntities.Count} 条");
            }
            catch (Exception e)
            {
                logger.LogError($"MiniExcel 读取文件异常：{e.Message}");
            }
        }


        private void InitPLC()
        {
            Plc=new SiemensS7Net(SiemensPLCS.S1200,options.Value.PlcParam.PlcIp);
            Plc.Port = options.Value.PlcParam.PlcPort;
            Plc.Slot=options.Value.PlcParam.PlcSlot;
            Plc.ConnectTimeOut=options.Value.PlcParam.PlcConnectTimeOut;
        }

        public async Task InitPLCServer()
        {
            try
            {
                var OpResult = await Plc.ConnectServerAsync();
                
                PLCConnnection = OpResult.IsSuccess;
                if (!OpResult.IsSuccess)
                {
                    logger.LogError($"PLC连接失败{options.Value.PlcParam.PlcIp}:{options.Value.PlcParam.PlcPort}");
                }

            }
            catch (Exception e)
            {
                logger.LogError($"PLC连接异常{e.Message}");
            }
        }

        /// <summary>
        /// 开始采集，写入读取字典 ReadDataDic
        /// </summary>
        /// <param name="externalToken"></param>
        /// <returns></returns>
        /// 
        public Task StartCollectionAsync(CancellationToken? externalToken = null)
        {
            StopCollection();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(
            externalToken ?? CancellationToken.None);

            return Task.Run(async () => 
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        // 批量读取赋值给字典
                        await UpdateControlData();
                        await UpdateMonitorData();
                        await UpdateProcessData();
                        await SaveAsync();

                        await Task.Delay(options.Value.PlcParam.PlcCycleInterval, _cts.Token);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.Message);
                    }
                }

            },_cts.Token);
        }

        private async Task UpdateControlData()
        {
            await UpdatePlcToReadDataDic<float> ("Data", "DBD");
        }
        private async Task UpdateMonitorData()
        {
            await UpdatePlcToReadDataDic<bool>("Monitor", "DBX");
        }
        private async Task UpdateProcessData()
        {
            await UpdatePlcToReadDataDic<bool>("Control", "DBX");
        }

        private async Task UpdatePlcToReadDataDic<T>(string module, string addressType)
        {
            try
            {
                var addresses = readEntities.Where(x => x.Module == module
                && x.Address.Contains(addressType)).ToList();

                if ((addresses.Count <1))
                {
                    return;
                }

                OperateResult<T[]>? result;

                if (typeof(T) == typeof(bool))
                {
                    result = await Plc
                        .ReadBoolAsync(addresses
                        .First().Address, (ushort)addresses.Count) as OperateResult<T[]>;
                }
                else if (typeof(T) == typeof(float))
                {
                    result = await Plc
                        .ReadFloatAsync(addresses
                            .First().Address, (ushort)addresses.Count) as OperateResult<T[]>;
                }
                else
                {
                    logger.LogError("不支持传入的类型");

                    return;
                }

                // 将 result 结果放入到字典中
                if (!result.IsSuccess)
                {
                    logger.LogError("数据读取失败");

                    return;
                }

                for (int i = 0; i < addresses.Count; i++)
                {
                    if (ReadDataDic.ContainsKey(addresses[i].En))
                    {
                        ReadDataDic[addresses[i].En] = result.Content[i];
                    }
                    else
                    {
                        ReadDataDic.TryAdd(addresses[i].En, result.Content[i]);
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }



        private async Task SaveAsync()
        {
            
        }


        /// <summary>
        /// 获取实时数据
        /// </summary>

        public T GetValue<T>(string key)
        {
            if(ReadDataDic.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            else
            {
                logger.LogError($"字典中不存在键 {key}");
                return default(T);
            }
            {
                return (T)ReadDataDic[key];
            }
           
        }


        public void StopCollection()
        {
            try
            {
                if ( _cts!= null)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                }
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
