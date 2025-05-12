using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Models
{
    public class EntityBase : ObservableObject
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        private int _Id;

        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]//设置主键和自增
        public int Id
        {
            get => _Id;
            set => SetProperty(ref _Id, value);
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime _CreateTime = DateTime.Now;
        
        public DateTime CreateTime
        {
            get => _CreateTime;
            set => SetProperty(ref _CreateTime, value);
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        private DateTime _UpdateTime = DateTime.Now;
        
        public DateTime UpdateTime
        {
            get => _UpdateTime;
            set => SetProperty(ref _UpdateTime, value);
        }
    }
}
