using CommunityToolkit.Mvvm.ComponentModel;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		

	}
}
