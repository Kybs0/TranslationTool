using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TranslationTool.Views
{
    /// <summary>
    /// CircleLoading.xaml 的交互逻辑
    /// </summary>
    public partial class CircleLoading : UserControl
    {
        public CircleLoading()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty IsActivepProperty = DependencyProperty.Register("IsActive",
            typeof(bool), typeof(CircleLoading), new PropertyMetadata(default(bool)));

        public bool IsActive
        {
            get => (bool)GetValue(IsActivepProperty);
            set => SetValue(IsActivepProperty,value);
        }

        public static readonly DependencyProperty ForegroundBrushProperty = DependencyProperty.Register(
            "ForegroundBrush", typeof(SolidColorBrush), typeof(CircleLoading), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));
        public SolidColorBrush ForegroundBrush
        {
            get => (SolidColorBrush)GetValue(ForegroundBrushProperty);
            set => SetValue(ForegroundBrushProperty, value);
        }

        public static readonly DependencyProperty LoadingSizeProperty = DependencyProperty.Register(
            "LoadingSize", typeof(LoadingSize), typeof(CircleLoading), new PropertyMetadata(LoadingSize.Size24));

        public LoadingSize LoadingSize
        {
            get => (LoadingSize)GetValue(LoadingSizeProperty);
            set => SetValue(LoadingSizeProperty, value);
        }
    }

    public enum LoadingSize
    {
        Size16 = 16,
        Size24 = 24,
        Size32 = 32
    }
}
