using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem.Models
{
    [SqlSugar.SugarTable("User")]
    public class User:EntityBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string _UserName;
        public string UserName
        {
            get => _UserName;
            set => SetProperty(ref _UserName, value);
        }
        /// <summary>
        /// 密码
        /// </summary>
        private string _Password;
        public string Password
        {
            get => _Password;
            set => SetProperty(ref _Password, value);
        }
        /// <summary>
        /// 角色
        /// </summary>
        /// 1:普通用户, 0:管理员
        private int _Role;
            public int Role
            {
                get => _Role;
                set => SetProperty(ref _Role, value);
            }
    }
}
