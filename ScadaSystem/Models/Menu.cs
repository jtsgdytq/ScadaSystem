using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Models
{
    [Table("Menu")]
    public class Menu:EntityBase
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        private string _menuName;
        public string MenuName
        {
            get => _menuName;
            set => SetProperty(ref _menuName, value);
        }
        /// <summary>
        /// 菜单图标
        /// </summary>
        private string _icon;
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        /// <summary>
        /// 排序
        /// </summary>
        private int _sort;
        public int Sort
        {
            get => _sort;
            set => SetProperty(ref _sort, value);
        }
        /// <summary>
        /// 菜单视图
        /// </summary>
        private string _view;
        public string View
        {
            get => _view;
            set => SetProperty(ref _view, value);
        }
        /// <summary>
        /// 角色权限
        /// </summary>
        private int _role;
        public int Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

    }
}
