﻿using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MyDlmsStandard;
using NLog;
using Quartz;
using 三相智慧能源网关调试软件.ViewModel.DlmsViewModels;

namespace 三相智慧能源网关调试软件.Model.Jobs
{
    public abstract class ProfileGenericJobBase : ObservableObject, IProfileGenericJob
    {
        protected ProfileGenericJobBase()
        {
            Client = SimpleIoc.Default.GetInstance<DlmsClient>();
        }

        public DlmsClient Client { get; set; }
        public string JobName { get; set; }
        public int Period { get; set; }


        public CustomCosemProfileGenericModel CustomCosemProfileGenericModel
        {
            get => _customCosemProfileGenericModel;
            set
            {
                _customCosemProfileGenericModel = value;
                OnPropertyChanged();
            }
        }

        private CustomCosemProfileGenericModel _customCosemProfileGenericModel;


        public virtual async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (Client.Socket.SocketClientList.Count == 0)
                {
                    return;
                }
                foreach (var socket in Client.Socket.SocketClientList)
                {
                    var so = socket;
                    await Task.Run(async () =>
                    {
                        var tempClient = Client;
                        tempClient.CurrentSocket = so;
                        tempClient.DlmsSettingsViewModel.InterfaceType = InterfaceType.WRAPPER;
                        tempClient.DlmsSettingsViewModel.CommunicationType = CommunicationType.FrontEndProcess;
                        var netLogViewModel = SimpleIoc.Default.GetInstance<NetLogViewModel>();
                        netLogViewModel.MyServerNetLogModel.Log =
                            "正在执行" + JobName + "\r\n";
                        netLogViewModel.MyServerNetLogModel.Log =
                            "正在执行初始化请求";
                        await tempClient.InitRequest();

                        await Task.Delay(2000);
                        netLogViewModel.MyServerNetLogModel.Log =
                            "正在执行读取曲线捕获对象";
                        await tempClient.GetRequestAndWaitResponse(CustomCosemProfileGenericModel
                            .GetCaptureObjectsAttributeDescriptor());
                        await Task.Delay(2000);
                        netLogViewModel.MyServerNetLogModel.Log =
                            "正在执行读取曲线Buffer";
                        await tempClient.GetRequestAndWaitResponseArray(CustomCosemProfileGenericModel
                            .GetBufferAttributeDescriptorWithSelectionByRange());
                        await Task.Delay(2000);
                        netLogViewModel.MyServerNetLogModel.Log =
                            "正在执行释放请求";
                        await tempClient.ReleaseRequest(true);
                    });
                }
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Error(JobName + e.Message);
            }
        }
    }
}