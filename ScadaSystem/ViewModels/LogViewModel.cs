using AngleSharp.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignColors.Recommended;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using ScadaSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OscarClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogEntry = ScadaSystem.Models.LogEntry;

namespace ScadaSystem.ViewModels
{
   public partial class LogViewModel:ObservableObject
    {

        [ObservableProperty]
        ObservableCollection<FileInfo> _logFiles = new();
        [ObservableProperty]
        ObservableCollection<LogEntry> _logEntries = new();
        [ObservableProperty]
        FileInfo _selectedFile;
       
        [RelayCommand]
        void FolderInfo()
        {
            try
            {
                LogFiles.Clear();

                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

                if (Directory.Exists(logPath))
                {
                    // 遍历一周的日志文件
                    var currentDate = DateTime.Now;
                    var oneWeekAgo = currentDate.AddDays(-7);

                    // 获取所有以日期命名的子文件夹
                    var dateFolders = Directory.GetDirectories(logPath)
                        .Where(dir => DateTime.TryParse(Path.GetFileName(dir), out _))
                        .Select(dir => new DirectoryInfo(dir));

                    var recentFolders = dateFolders.Where(dir =>
                    {
                        if (DateTime.TryParse(dir.Name, out DateTime folderDate))
                        {
                            return folderDate >= oneWeekAgo && folderDate <= currentDate;
                        }

                        return false;
                    });

                    // 获取满足日期的文件夹下的所有日志文件
                    var logFiles = recentFolders.SelectMany(dir =>
                    dir.GetFiles("*.log", SearchOption.AllDirectories))
                        .OrderByDescending(f => f.LastWriteTime);

                    LogFiles = new ObservableCollection<FileInfo>(logFiles);
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
            }
        }

        partial void OnSelectedFileChanged(FileInfo value)
        {
            if (value != null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        const int MaxLines = 500;
                        var lines = new List<string>();

                        // 读取文件的最后 N 行，避免一次性加载整个文件
                        using (var fs = new FileStream(value.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var reader = new StreamReader(fs))
                        {
                            var allLines = new LinkedList<string>();

                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                if (!string.IsNullOrWhiteSpace(line))
                                {
                                    allLines.AddLast(line);
                                    if (allLines.Count > MaxLines)
                                        allLines.RemoveFirst();
                                }
                            }

                            lines = allLines.ToList();
                        }

                        var entries = new List<LogEntry>();
                        foreach (var line in lines)
                        {
                            var entry = LogEntry.Parse(line);
                            if (entry != null)
                                entries.Add(entry);
                        }

                        // 回到 UI 线程更新日志列表
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            LogEntries.Clear();
                            foreach (var entry in entries)
                            {
                                LogEntries.Add(entry); // 可以进一步优化成分页显示
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                });
            }
        }




    }
}
