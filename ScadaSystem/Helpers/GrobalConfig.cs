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
using NLog;
namespace ScadaSystem.Helpers
{
    class GrobalConfig : IDisposable
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




        public GrobalConfig(IOptionsSnapshot<RootParam> _options, ILogger logger)
        {
            options = _options;

            InitPLC();//初始化PLC

            InitExcelsAddress();
            this.logger = logger;
        }

        private void InitExcelsAddress()
        {
            var RootPath=AppDomain.CurrentDomain.BaseDirectory+"Configs\\";
            var ReadPath = RootPath + "TulingRead.xlsx";
            var WritePath = RootPath + "TulingWrite.xlsx";
            if (!File.Exists(ReadPath) || !File.Exists(WritePath))
            {
                logger.Error($"找不到读/写文件路径{ReadPath}\n{WritePath}");
                return; 
            }
            try
            {
                var ReadEntitice= MiniExcel.Query<ReadEntity>(ReadPath).Where(x=>!string.IsNullOrEmpty(ReadPath)).ToList();
                var WriteEntitice = MiniExcel.Query<ReadEntity>(ReadPath).Where(x => !string.IsNullOrEmpty(WritePath)).ToList();


            }
            catch (Exception e)
            {

                logger.Error($"MiniExcel 读取文件异常{e.Message}");
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
                if (!OpResult.IsSuccess)
                {
                    logger.Error($"PLC连接失败{options.Value.PlcParam.PlcIp}:{options.Value.PlcParam.PlcPort}");
                }

            }
            catch (Exception e)
            {

                logger.Error($"PLC连接异常{e.Message}");
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
                        logger.Error(e.Message);
                    }
                }

            },_cts.Token);
        }

        private async Task UpdateControlData()
        {
            
        }
        private async Task UpdateMonitorData()
        {
           
        }
        private async Task UpdateProcessData()
        {
            
        }
        private async Task SaveAsync()
        {
            
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
                logger.Error(e.Message);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
