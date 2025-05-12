using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HslCommunication.BasicFramework;
using ScadaSystem.Helpers;
using ScadaSystem.Messages;
using ScadaSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScadaSystem.ViewModels
{
   
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        /// <summary>
        /// 当前用户
        /// </summary>
        public UserSession UserSessionProp { get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userSessionProp"></param>
        public LoginViewModel(UserSession userSessionProp)
        {
            UserSessionProp = userSessionProp;
        }

        /// <summary>
        /// 登录
        /// </summary>
        [RelayCommand]
        public void Login()
        {
            if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("用户名或密码不能为空");
                return;
            }
            //验证用户名和密码
            var user = SqlSugarHelper.Db.Queryable<Models.User>().Where(u => u.UserName == Username && u.Password == Password).First();
            if (user == null)
            {
                MessageBox.Show("用户名或密码错误");
                return;
            }
            else
            {
                //登录成功，保存用户信息
                UserSessionProp.CurrentUser = user;
                //MessageBox.Show("登录成功");
                //发送成功登录消息
                WeakReferenceMessenger.Default.Send(new LoginMessage(user));
                //界面跳转
            }
        }

        /// <summary>
        /// 清空账号和密码
        /// </summary>
        public void ClearCredentials()
        {
            Username = string.Empty;
            Password = string.Empty;
        }

    }
}
