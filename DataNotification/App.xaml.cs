﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace DataNotification
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
     


        protected override void OnStartup(StartupEventArgs e)
        {
        
            DispatcherHelper.Initialize();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            base.OnStartup(e);
        }


        //仅能捕获 Task 中抛出的未处理异常 事件的触发有延时，依赖垃圾回收
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Logger.Error("TaskError" + e.Exception);

            e.SetObserved();
        }

        //能捕获 所有线程（Task 除外） 抛出的未处理异常 默认情况无法阻止程序崩溃（可通过 legacyUnhandledExceptionPolicy 配置异常策略 ）
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error("无法处理的异常啊" + e.ExceptionObject);
        }

        //能够捕获 UI 线程抛出的未处理异常 可通过事件参数 e.Handled = true 来阻止程序崩溃
        private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error($"Current_DispatcherUnhandledException:{e.Exception}");

            e.Handled = true;
        }
    }
}