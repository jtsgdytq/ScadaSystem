using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Masuit.Tools;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;

namespace ScadaSystem.ViewModels
{
    public partial class FormulaViewModel:ObservableObject      
    {
        [ObservableProperty]
        ObservableCollection<FormulaEntity> _formulaList = new();


        [ObservableProperty]
        FormulaEntity? _selectedFormula;

        [ObservableProperty]
        FormulaEntity _currentFormula = new();
        GrobalConfig _grobalConfig;
        UserSession _userSession;

        public FormulaViewModel(GrobalConfig grobalConfig, UserSession userSession)
        {
            _grobalConfig = grobalConfig;
            _userSession = userSession;
            QueryFormula();
        }

        /// <summary>
        /// 查询配方
        /// </summary>
        [RelayCommand]
        void QueryFormula()
        {

            FormulaList.Clear();
            SqlSugarHelper.Db.Queryable<FormulaEntity>()
                .OrderBy(x=>x.CreateTime,SqlSugar.OrderByType.Desc)
                .ToList()
                .ForEach(x=>FormulaList.Add(x));
        }
        /// <summary>
        /// 选中的配方
        /// </summary>
        /// <param name="formula"></param>
        [RelayCommand]
        void SelectFormula(FormulaEntity formula)
        {
            //将所有配方设置为未选中
            FormulaList.ForEach(x => x.IsSelected = false);
            formula.IsSelected = true;
            SelectedFormula=formula;

            CurrentFormula=formula.DeepClone();


        }
        /// <summary>
        /// 新建配方
        /// </summary>
        [RelayCommand]
        
        void NewFormula() 
        {
            SelectedFormula = null;
            CurrentFormula=new();
        }

        [RelayCommand]

        void SaveFormula() 
        {
            try
            {
                if(string.IsNullOrEmpty(CurrentFormula.Name)||string.IsNullOrEmpty(CurrentFormula.Description))
                {
                    _userSession.ShowMessage("配方名字和描述不能为空");
                }
                ///现有配方上修改
                else if(SelectedFormula !=null)
                {
                    var existFormula = FormulaList.FirstOrDefault(x=>x.Id==SelectedFormula.Id);
                    if(existFormula!=null)
                    {
                        CurrentFormula.UpdateTime = DateTime.Now;
                         var index= FormulaList.IndexOf(existFormula);
                        FormulaList[index] = CurrentFormula;
                    }

                }
                //新配方
                else
                {
                    CurrentFormula.UpdateTime= DateTime.Now;
                    CurrentFormula.CreateTime= DateTime.Now;
                    FormulaList.Add(CurrentFormula);
                }

               var rows= SqlSugarHelper.Db.Storageable<FormulaEntity>(FormulaList).ExecuteCommand();
                if(rows > 0)
                {
                    _userSession.ShowMessage("保存成功");
                }
                else
                {
                    _userSession.ShowMessage("保存失败");
                }

            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
                _userSession.ShowMessage("未知错误");
            }
        }

        [RelayCommand]
        async void DeleteFormula()
        {
            if (SelectedFormula == null)
            {
                _userSession.ShowMessage("请选择要删除的配方");
                return;
            }

            bool confirm = await _userSession.ShowConfirmAsync("是否删除？");

            if (confirm)
            {
                FormulaList.Remove(SelectedFormula);
                var dbResult = SqlSugarHelper.Db.Deleteable(SelectedFormula).ExecuteCommand();
                if (dbResult > 0)
                {
                    // 清空已删除项
                    SelectedFormula = null;
                    CurrentFormula = new FormulaEntity();
                    QueryFormula();
                    _userSession.ShowMessage("删除成功");
                }

                else
                {
                    _userSession.ShowMessage("删除失败");
                }
            }
        }


        [RelayCommand]
        void DownloadFormula()
        {
            try
            {
                foreach (var prop in typeof(FormulaEntity).GetProperties())
                {
                    // 寻找对应的 PLC 地址
                    var address = _grobalConfig.writeEntities
                        .FirstOrDefault(x => x.En == prop.Name)
                        ?.Address;

                    if (!address.IsNullOrEmpty())
                    {
                        // 判定 PLC 是否连接，如果连接我们则直接下发
                        if (_grobalConfig.PLCConnnection)
                        {
                            var value = prop.GetValue(CurrentFormula);

                            if (value != null)
                            {
                                _grobalConfig.Plc.WriteAsync(address, (float)value);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }

    
}
