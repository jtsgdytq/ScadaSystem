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
    public class UserSession:ObservableObject
    {
		private User _use= new User { UserName="test",Password="test"};//以防空指针异常
        /// <summary>
        /// 当前用户
        /// </summary>
		public User CurrentUser
        {
            get { return _use; }
            set { SetProperty(ref _use, value); }
        }

        public void ShowMessage(string content, MessageBoxButton button = MessageBoxButton.OK)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Dialog dialog = new Dialog(content, button);
                DialogHost.Show(dialog, "ShellDialog");
                
            });
        }




    }
}
