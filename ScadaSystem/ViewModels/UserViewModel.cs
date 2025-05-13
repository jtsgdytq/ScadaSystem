using AngleSharp.Dom;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Masuit.Tools;
using MaterialDesignThemes.Wpf;
using ScadaSystem.Helpers;
using ScadaSystem.Models;
using ScadaSystem.Services;
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
    UserSession userSession { get; }

    [ObservableProperty]
    List<User> userList = new List<User>();
    public UserViewModel(UserSession userSession)
    {
        this.userSession = userSession;
    }
    /// <summary>
    /// 搜索用户
    /// </summary>
    [RelayCommand]
    public void Search()
    {
        UserList = SqlSugarHelper.Db.Queryable<ScadaSystem.Models.User>().ToList();
    }
    /// <summary>
    /// 加载用户
    /// </summary>
    [RelayCommand]
    public void Load()
    {
        //UserList = SqlSugarHelper.Db.Queryable<ScadaSystem.Models.User>().ToList();
        Search();
    }
    /// <summary>
    /// 添加用户
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task Add()
    {
        var viewModel = new UpdateViewModel<User>(new User());
        var view = new UserOperationView { DataContext = viewModel };
        // 如果是普通用户，则无权限添加用户
        if (userSession.CurrentUser.Role==1)
        {
            MessageBox.Show("普通用户无权限添加用户");
            return;
        }

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
    /// <summary>
    /// 编辑用户
    /// </summary>
    /// <param name="user"></param>
    [RelayCommand]
    public async Task Edit(User user)
    {
        // 权限判断：普通用户无权限编辑
        if (userSession.CurrentUser.Role == 1)
        {
            MessageBox.Show("普通用户无权限编辑用户");
            return;
        }

        // 深拷贝 user，防止未保存时污染原始数据
        var userClone = user.DeepClone(); // 你需要实现 DeepClone() 方法
        var viewModel = new UpdateViewModel<User>(userClone);
        var view = new UserOperationView { DataContext = viewModel };

        var result = await DialogHost.Show(view, "ShellDialog");

        if (bool.TryParse(result?.ToString(), out bool dialogResult) && dialogResult)
        {
            var entity = viewModel.Entity;

            // 输入验证
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

            // 将修改同步回原始 user 实体
            user.UserName = entity.UserName;
            user.Password = entity.Password;
            user.Role = entity.Role;
            user.UpdateTime = DateTime.Now;

            int count = await SqlSugarHelper.Db.Updateable(user).ExecuteCommandAsync();

            if (count > 0)
            {
                MessageBox.Show("编辑成功");
                Search();
            }
            else
            {
                MessageBox.Show("编辑失败");
            }
        }
    }


    /// <summary>
    /// 删除用户
    /// </summary>
    [RelayCommand]
    void Delete(User user)
    {

        if (userSession.CurrentUser.Role == 1)
        {
            MessageBox.Show("普通用户无权限删除用户");
            return;
        }
        if(userSession.CurrentUser.UserName ==user.UserName)
        {
            MessageBox.Show("不能删除自己");
            return;
        }

       
        var result = MessageBox.Show("确定要删除吗？", "提示", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            // 删除用户
            int count = SqlSugarHelper.Db.Deleteable(user).ExecuteCommand();
            if (count > 0)
            {
                MessageBox.Show("删除成功");
                Search();
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }
    }
}

