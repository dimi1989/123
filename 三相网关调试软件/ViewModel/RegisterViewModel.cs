﻿using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MySerialPortMaster;
using 三相智慧能源网关调试软件.DLMS;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.ApplicationLayEnums;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.CosemObjects;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.Get;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.Set;
using 三相智慧能源网关调试软件.Model;

namespace 三相智慧能源网关调试软件.ViewModel
{
    /*
        联系邮箱：694965217@qq.com
        创建时间：2020/06/04 16:06:26
        主要用途：
        更改记录：
    */
    public class RegisterViewModel : ViewModelBase
    {
        public ObservableCollection<DLMSSelfDefineRegisterModel> Registers
        {
            get => _registers;
            set
            {
                _registers = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<DLMSSelfDefineRegisterModel> _registers;

        public RelayCommand<DLMSSelfDefineRegisterModel> GetValueCommand
        {
            get => _getValueCommand;
            set
            {
                _getValueCommand = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<DLMSSelfDefineRegisterModel> _getValueCommand;

        public RelayCommand<DLMSSelfDefineRegisterModel> SetValueCommand
        {
            get => _setValueCommand;
            set
            {
                _setValueCommand = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<DLMSSelfDefineRegisterModel> _setValueCommand;

        public RelayCommand<DLMSRegister> GetLogicNameCommand
        {
            get => _getLogicNameCommand;
            set
            {
                _getLogicNameCommand = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<DLMSRegister> _getLogicNameCommand;

        public DLMSClient Client { get; set; }


        public RegisterViewModel()
        {
            ExcelHelper excel = new ExcelHelper("DLMS设备信息.xls");
            var dataTable = excel.GetExcelDataTable("Register$");
            Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<DLMSClient>();
            Registers = new ObservableCollection<DLMSSelfDefineRegisterModel>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                Registers.Add(new DLMSSelfDefineRegisterModel(dataTable.Rows[i][0].ToString())
                    {RegisterName = dataTable.Rows[i][1].ToString()});
            }

            GetValueCommand = new RelayCommand<DLMSSelfDefineRegisterModel>(
                async t =>
                {
                    t.Value = new DLMSDataItem();
                    t.Scalar = 1;
                    t.Unit = Unit.None;
                    GetResponse getResponse = new GetResponse();
                    GetRequest getRequest = new GetRequest
                    {
                        GetRequestNormal = new GetRequestNormal(t.GetValueAttributeDescriptor())
                    };
                    var dataResult = await Client.GetRequest(getRequest.ToPduBytes());
                    if (getResponse.PduBytesToConstructor(dataResult))
                    {
                        t.LastResult = getResponse.GetResponseNormal.Result.DataAccessResult;
                        t.Value = getResponse.GetResponseNormal.Result.Data;
                        if (t.LastResult!=ErrorCode.Ok)
                        {
                            return;
                        }
                        getRequest.GetRequestNormal=new GetRequestNormal(t.GetScalar_UnitAttributeDescriptor());
                           var scalarUnit = await Client.GetRequest(getRequest);
                        var structData = NormalDataParse.ParsePduData(scalarUnit);
                        var unitByte = structData.StringToByte();
                        switch (unitByte.Take(1).ToArray()[0])
                        {
                            case (byte)DataType.Int8:
                                t.Scalar = (sbyte)unitByte.Skip(1).Take(1).ToArray()[0];
                                break;
                        }

                        switch (unitByte.Skip(2).Take(1).ToArray()[0])
                        {
                            case (byte)DataType.Enum:
                                t.Unit = (Unit)unitByte.Skip(3).Take(1).ToArray()[0];
                                break;
                        }
                    }
                });
            SetValueCommand=new RelayCommand<DLMSSelfDefineRegisterModel>(async(t) =>
            {
                t.Value.UpdateValueBytes();
                SetRequest setRequest=new SetRequest();
                setRequest.SetRequestNormal=new SetRequestNormal(t.GetValueAttributeDescriptor(),t.Value);
                var dataResult = await Client.SetRequest(setRequest);
            });
        }
    }
}