using System;
using System.IO;
using System.Speech.Synthesis;
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
            SaveFile(pronounceUrl, (this.DataContext as SearchWordViewModel).CurrentWord);
        }

        private void SaveFile(string pronounceUrl, string saveFileName)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.OverwritePrompt = true;
            var extension = Path.GetExtension(pronounceUrl);
            saveFileDialog.FileName = saveFileName + extension;
            saveFileDialog.Filter = $"音频文件(*{extension})|*{extension}";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveFileDialog.ShowDialog(Window.GetWindow(this)) == true)
            {
                var fileName = saveFileDialog.FileName;
                File.Copy(pronounceUrl, fileName, true);
            }
        }

        private void DownloadSentenceButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is SentenceModel sentenceModel)
            {
                var extension = ".mp3";
                var hasAudioFile = !string.IsNullOrEmpty(sentenceModel.EnglishSentenceUri) &&
                                   File.Exists(sentenceModel.EnglishSentenceUri);
                if (hasAudioFile)
                {
                    extension = Path.GetExtension(sentenceModel.EnglishSentenceUri);
                }
                var sentence = sentenceModel.EnglishSentence.Length > 3
                    ? sentenceModel.EnglishSentence.Substring(3, sentenceModel.EnglishSentence.Length - 3)
                    : sentenceModel.EnglishSentence;

                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.FileName = sentence + extension;
                saveFileDialog.Filter = $"音频文件(*{extension})|*{extension}";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (saveFileDialog.ShowDialog(Window.GetWindow(this)) == true)
                {
                    var fileName = saveFileDialog.FileName;
                    if (hasAudioFile)
                    {
                        File.Copy(sentenceModel.EnglishSentenceUri, fileName, true);
                    }
                    else
                    {
                        ExportSystemAudioFile(fileName, sentenceModel.EnglishSentence);
                    }
                }
            }
        }

        /// <summary>
        /// 导出系统音频
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="text"></param>
        private void ExportSystemAudioFile(string filePath, string text)
        {
            using (SpeechSynthesizer speechSyn = new SpeechSynthesizer())
            {
                speechSyn.Volume = 50;
                speechSyn.Rate = 0;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                speechSyn.SetOutputToWaveFile(filePath);
                speechSyn.Speak(text);
                speechSyn.SetOutputToDefaultAudioDevice();
            }
        }

        private readonly SpeechSynthesizer _speechSyn = new SpeechSynthesizer();
        private void SpeakSentenceButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is SentenceModel sentenceModel)
            {
                if (!string.IsNullOrEmpty(sentenceModel.EnglishSentenceUri) && File.Exists(sentenceModel.EnglishSentenceUri))
                {
                    (this.DataContext as SearchWordViewModel)?.SpeekCommand.Execute(sentenceModel.EnglishSentenceUri);
                }
                else
                {
                    //系统读音
                    var currentSpokenPrompt = _speechSyn.GetCurrentlySpokenPrompt();
                    if (currentSpokenPrompt != null)
                    {
                        _speechSyn.SpeakAsyncCancel(currentSpokenPrompt);
                    }
                    _speechSyn.SpeakAsync(sentenceModel.EnglishSentence);
                }
            }
        }
    }
}
