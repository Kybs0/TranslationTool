using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Translation.Business;
using Application = System.Windows.Application;

namespace TranslationTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        MainWindow _mainWindow;
        public App()
        {
            KillProcess(System.Windows.Forms.Application.ProductName);

            SetAppAutoRun(true);

            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();
            SetNotifyIcon();
            DeleteRunnableCache();
        }

        private void DeleteRunnableCache()
        {
            Task.Run(() =>
            {
                var userDownloadFolder = CustomPathUtil.GetUserDownloadFolder();
                var fileInfos = new DirectoryInfo(userDownloadFolder).GetFiles();
                foreach (var fileInfo in fileInfos)
                {
                    File.Delete(fileInfo.FullName);
                }
            });
        }

        #region 托盘图标

        private NotifyIcon _notifyIcon;
        private void SetNotifyIcon()
        {
            this._notifyIcon = new NotifyIcon();
            this._notifyIcon.BalloonTipText = "翻译小工具";
            this._notifyIcon.ShowBalloonTip(2000);
            this._notifyIcon.Text = "集成金山、有道非官方数据的翻译工具";
            this._notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this._notifyIcon.Visible = true;
            //打开菜单项
            MenuItem open = new MenuItem("打开");
            open.Click += new EventHandler(ShowMainWindow);
            //退出菜单项
            MenuItem exit = new MenuItem("退出");
            exit.Click += new EventHandler(Close);
            //关联托盘控件
            MenuItem[] childen = new MenuItem[] { open, exit };
            _notifyIcon.ContextMenu = new ContextMenu(childen);

            this._notifyIcon.MouseDoubleClick += new MouseEventHandler((o, e) =>
            {
                if (e.Button == MouseButtons.Left) ShowMainWindow(o, e);
            });
        }

        private void ShowMainWindow(object sender, EventArgs e)
        {
            _mainWindow.Visibility = Visibility.Visible;
            _mainWindow.ShowInTaskbar = true;
            _mainWindow.Activate();
        }

        private void Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region 删除原有进程

        /// <summary>
        /// 删除原有进程
        /// </summary>
        /// <param name="processName"></param>
        private void KillProcess(string processName)
        {
            try
            {
                //删除所有同名进程
                Process currentProcess = Process.GetCurrentProcess();
                var processes = Process.GetProcessesByName(processName).Where(process => process.Id != currentProcess.Id);
                foreach (Process thisproc in processes)
                {
                    thisproc.Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region 开机自启动

        private void SetAppAutoRun(bool autoRun)
        {
            try
            {
                string executablePath = System.Windows.Forms.Application.ExecutablePath;
                string exeName = Path.GetFileNameWithoutExtension(executablePath);
                SetAutoRun(autoRun, exeName, executablePath);
            }
            catch (Exception e)
            {
            }
        }
        private bool SetAutoRun(bool autoRun, string exeName, string executablePath)
        {
            bool success = SetAutoRun(Registry.CurrentUser, autoRun, exeName, executablePath);
            if (!success)
            {
                success = SetAutoRun(Registry.LocalMachine, autoRun, exeName, executablePath);
            }
            return success;
        }
        private bool SetAutoRun(RegistryKey rootKey, bool autoRun, string exeName, string executablePath)
        {
            try
            {
                RegistryKey autoRunKey = rootKey.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (autoRunKey == null)
                {
                    autoRunKey = rootKey.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run",RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                if (autoRunKey != null)
                {
                    if (autoRun) //设置开机自启动  
                    {
                        autoRunKey.SetValue(exeName, $"\"{executablePath}\" /background");
                    }
                    else //取消开机自启动  
                    {
                        autoRunKey.DeleteValue(exeName, false);
                    }
                    autoRunKey.Close();
                    autoRunKey.Dispose();
                }
            }
            catch (Exception e)
            {
                rootKey.Close();
                rootKey.Dispose();
                return false;
            }
            rootKey.Close();
            rootKey.Dispose();
            return true;
        }

        #endregion
    }
}
