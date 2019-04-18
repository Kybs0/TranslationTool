using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    }
}
