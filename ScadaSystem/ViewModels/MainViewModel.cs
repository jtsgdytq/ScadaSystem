using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HslCommunication.Profinet.Panasonic.Helper;
using ScadaSystem.Helpers;
using ScadaSystem.Messages;
using ScadaSystem.Models;
using ScadaSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Menu = ScadaSystem.Models.Menu;

namespace ScadaSystem.ViewModels
{
    public partial class MainViewModel:ObservableObject
    {

        public UserSession UserSession { get;} 
        public List<Menu> MenuEntities { get; set; } = new List<Menu>();

        public MainViewModel(UserSession _UserSession )
        {
            InitData();
            UserSession = _UserSession;
        }
        /// <summary>
        /// 初始化菜单列表
        /// </summary>
        private void InitData()
        {
            
            MenuEntities = SqlSugarHelper.Db.Queryable<Menu>().ToList();

        }
        /// <summary>
        /// 导航命令
        /// </summary>
        /// <param name="menu"></param>
        [RelayCommand]
        void Navigation(Menu menu)
        {
            WeakReferenceMessenger.Default.Send(menu);
        }
        /// <summary>
        /// 切换用户登录
        /// </summary>
        [RelayCommand]
        void ChangeUser()
        {
            //登出消息
            WeakReferenceMessenger.Default.Send(new LogoutMessage(UserSession.CurrentUser));
        }
    }
}
