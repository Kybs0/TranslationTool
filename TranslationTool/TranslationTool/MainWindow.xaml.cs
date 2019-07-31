﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Translation.Business.Util;
using TranslationTool.Helper;
using TranslationTool.Views;
using Application = System.Windows.Forms.Application;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;

namespace TranslationTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitViewByOperationConfig();
        }

        private void InitViewByOperationConfig()
        {
            var appConfig = IniFileHelper.IniReadValue(CustomUtils.UserLayoutSection, CustomUtils.LastViewKey);
            if (appConfig == nameof(SearchWordView))
            {
                TheSearchWordView.Visibility = Visibility.Visible;
                TheTranslationView.Visibility = Visibility.Collapsed;
            }
            else if (appConfig == nameof(TranslationView))
            {
                TheSearchWordView.Visibility = Visibility.Collapsed;
                TheTranslationView.Visibility = Visibility.Visible;
            }
        }

        private const double SlideTime = 300;
        private void ShowTranslationViewButton_OnClick(object sender, RoutedEventArgs e)
        {
            IniFileHelper.IniWriteValue(CustomUtils.UserLayoutSection, CustomUtils.LastViewKey, nameof(TranslationView));
            TheSearchWordView.Visibility = Visibility.Collapsed;
            TheTranslationView.Visibility = Visibility.Visible;
        }

        private void BackToWordViewButton_OnClick(object sender, RoutedEventArgs e)
        {
            IniFileHelper.IniWriteValue(CustomUtils.UserLayoutSection, CustomUtils.LastViewKey, nameof(SearchWordView));
            TheSearchWordView.Visibility = Visibility.Visible;
            TheTranslationView.Visibility = Visibility.Collapsed;
        }

        #region 窗口事件

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Hide(sender, e);
        }
        private void Hide(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.Visibility = Visibility.Hidden;
        }

        private void HeaderGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #endregion

    }
}
