﻿using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonServiceLocator;
using 三相智慧能源网关调试软件.ViewModel;
using 三相智慧能源网关调试软件.ViewModel.DlmsViewModels;

namespace 三相智慧能源网关调试软件.MyControl.DLMSControl
{
    /// <summary>
    /// FrontEndProcessorControl.xaml 的交互逻辑
    /// </summary>
    public partial class FrontEndProcessorControl : UserControl
    {
        public FrontEndProcessorControl()
        {
            InitializeComponent();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<DlmsClient>().CurrentSocket = (Socket) ListBox.SelectedItem;
            ServiceLocator.Current.GetInstance<TcpServerViewModel>().CurrentSocketClient =
                ServiceLocator.Current.GetInstance<DlmsClient>().CurrentSocket;
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            ServiceLocator.Current.GetInstance<DlmsClient>().CurrentSocket = (Socket) ListBox.SelectedItem;
            ServiceLocator.Current.GetInstance<TcpServerViewModel>().CurrentSocketClient =
                ServiceLocator.Current.GetInstance<DlmsClient>().CurrentSocket;
        }

        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"ncpa.cpl");
        }
    }
}