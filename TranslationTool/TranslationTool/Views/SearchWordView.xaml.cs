using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    }
}
