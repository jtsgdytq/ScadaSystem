using AngleSharp.Dom;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.ViewModels;
using ScadaSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public partial class UserViewModel : ObservableObject
{
    [ObservableProperty]
    List<User> userList = new List<User>();

    [RelayCommand]
    public void Search()
    {
        UserList = SqlSugarHelper.Db.Queryable<ScadaSystem.Models.User>().ToList();
    }

    [RelayCommand]
    public void Load()
    {
        //UserList = SqlSugarHelper.Db.Queryable<ScadaSystem.Models.User>().ToList();
        Search();
    }

    [RelayCommand]
    public async Task Add()
    {
        var viewModel = new UpdateViewModel<User>(new User());
        var view = new UserOperationView { DataContext = viewModel };

        var result = await DialogHost.Show(view, "ShellDialog");

        if (bool.TryParse(result?.ToString(), out bool dialogResult) && dialogResult)
        {
            var entity = viewModel.Entity;

            // 验证输入
            if (string.IsNullOrEmpty(entity.UserName) || string.IsNullOrEmpty(entity.Password))
            {
                MessageBox.Show("用户名和密码不能为空");
                return;
            }
            if (entity.Role != 0 && entity.Role != 1)
            {
                MessageBox.Show("角色必须为 0（管理员）或 1（普通用户）");
                return;
            }

            int count = SqlSugarHelper.Db.Insertable(entity).ExecuteReturnIdentity();
            if (count > 0)
            {
                MessageBox.Show("添加成功");
                Search();
            }
            else
            {
                MessageBox.Show("添加失败");
            }
        }
    }
}

