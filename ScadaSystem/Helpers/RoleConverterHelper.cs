using System;
using System.Globalization;
using System.Windows.Data;

namespace ScadaSystem.Helpers
{
    public class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int role)
            {
                return role == 0 ? "管理员" : "普通用户";
            }
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string roleStr)
            {
                return roleStr == "管理员" ? 0 : 1;
            }
            return 1;
        }
    }
}
