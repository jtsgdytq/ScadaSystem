using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;
using ScadaSystem.Models;
using ScadaSystem.Ucs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScadaSystem.Services
{
    public class UserSession : ObservableObject
    {
        private User _use = new User { UserName = "test", Password = "test" }; // 防空指针

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser
        {
            get { return _use; }
            set { SetProperty(ref _use, value); }
        }

        public async Task<bool> ShowConfirmAsync(string content)
        {
            var dialog = new Dialog(content, MessageBoxButton.YesNo);
            var result = await DialogHost.Show(dialog, "ShellDialog");

            if (result is bool boolResult)
                return boolResult;

            return false; // 默认认为取消或关闭窗口为 false
        }


        /// <summary>
        /// 不关心结果的消息提示
        /// </summary>
        public void ShowMessage(string content)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var dialog = new Dialog(content, MessageBoxButton.OK);
                DialogHost.Show(dialog, "ShellDialog");
            });
        }
    }





}

