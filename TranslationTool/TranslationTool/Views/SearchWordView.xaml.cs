using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Translation.Business.Util;
using TranslationTool.ViewModels;

namespace TranslationTool.Views
{
    /// <summary>
    /// SearchWordView.xaml 的交互逻辑
    /// </summary>
    public partial class SearchWordView : UserControl
    {
        public SearchWordView()
        {
            InitializeComponent();

            //DetailCheckBox.Visibility = Visibility.Visible;
            //ApiComboBox.Visibility = Visibility.Visible;
            InitViewByOperationConfig();
        }

        private void InitViewByOperationConfig()
        {
            //设置上次选中的单词tab
            var itemHeaderName = IniFileHelper.IniReadValue(CustomUtils.UserLayoutSection, CustomUtils.LastWorkTab);
            var itemCollection = TheTabControl.Items;
            foreach (var tabItemObject in itemCollection)
            {
                if (tabItemObject is TabItem tabItem && tabItem.Header.ToString().Equals(itemHeaderName))
                {
                    tabItem.IsSelected = true;
                }
            }
        }

        private void SearchingTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (this.DataContext as SearchWordViewModel)?.SearchCommand.Execute(null);
            }
        }

        private void ApiComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as SearchWordViewModel)?.SearchCommand.Execute(null);
        }

        private void SearchingHintTextBlock_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchingTextBox.Focus();
        }

        private void TabItem_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TabItem item)
            {
                IniFileHelper.IniWriteValue(CustomUtils.UserLayoutSection, CustomUtils.LastWorkTab, item.Header.ToString());
            }
        }
        /// <summary>
        /// 下载音频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadAudioButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pronounceUrl = (sender as MenuItem).DataContext as string;
            if (string.IsNullOrEmpty(pronounceUrl) || !File.Exists(pronounceUrl))
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            var extension = Path.GetExtension(pronounceUrl);
            saveFileDialog.FileName = (this.DataContext as SearchWordViewModel).CurrentWord + extension;
            saveFileDialog.Filter = $"音频文件(*{extension})|*{extension}";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog(Window.GetWindow(this)) == true)
            {
                var fileName = saveFileDialog.FileName;
                File.Copy(pronounceUrl, fileName, true);
            }
        }
    }
}
