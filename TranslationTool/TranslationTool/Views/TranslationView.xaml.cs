using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TranslationTool.ViewModels;
using Control = System.Windows.Forms.Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace TranslationTool.Views
{
    /// <summary>
    /// TranslationView.xaml 的交互逻辑
    /// </summary>
    public partial class TranslationView : UserControl
    {
        public TranslationView()
        {
            InitializeComponent();
        }
        private void ApiComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as TranslationViewModel)?.SearchCommand.Execute(null);
        }

        private void SearchingHintTextBlock_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SearchingTextBox.Focus();
        }

        private void SearchingTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var text = SearchingTextBox.Text;
            if ((Control.ModifierKeys & Keys.Alt) != 0 && e.Key == Key.Enter)
            {
                SearchingTextBox.Text = text + "\r\n";
                return;
            }
            if (e.Key == Key.Enter)
            {
                var replacedText = text.Substring(SearchingTextBox.CaretIndex, text.Length - SearchingTextBox.CaretIndex).Replace("\r\n", string.Empty);
                //最后Enter触发请求
                if (string.IsNullOrWhiteSpace(replacedText))
                {
                    (this.DataContext as TranslationViewModel)?.SearchCommand.Execute(null);
                }
            }
        }
    }
}
