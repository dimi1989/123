﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using 三相智慧能源网关调试软件.DLMS;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.CosemObjects;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.Get;

namespace 三相智慧能源网关调试软件.ViewModel
{
    public class ClockViewModel : ViewModelBase
    {
        public DLMSClient Client { get; set; }
        public DLMSClock Clock { get; set; }

        public ClockViewModel()
        {
            Client = CommonServiceLocator.ServiceLocator.Current.GetInstance<DLMSClient>();
            ExcelHelper excel = new ExcelHelper("DLMS设备信息.xls");
            var dataTable = excel.GetExcelDataTable("Clock$");
            Clock = new DLMSClock(dataTable.Rows[0][0].ToString());
            GetTimeCommand = new RelayCommand(async () =>
            {
                GetRequest getRequest=new GetRequest();
                getRequest.GetRequestNormal=new GetRequestNormal(Clock.GetTimeAttributeDescriptor());
                var dataResult = await Client.GetRequestAndWaitResponse(getRequest);
                
                if (dataResult!=null)
                {
                    var DisplayFormat = OctetStringDisplayFormat.DateTime;
                    Clock.Time =
                        NormalDataParse.HowToDisplayOctetString(
                            dataResult.GetResponseNormal.Result.Data.ValueBytes, DisplayFormat);
                }
//                if (r.PduBytesToConstructor(dataResult))
//                {
//                    var DisplayFormat = OctetStringDisplayFormat.DateTime;
//                    Clock.Time =
//                        NormalDataParse.HowToDisplayOctetString(r.GetResponseNormal.Result.Data.ValueBytes,
//                            DisplayFormat);
////                   Clock.Time = r.GetResponseNormal.Result.Data.ValueString;
//                }
            });
            GetTimeZoneCommand = new RelayCommand(async () =>
            {
                GetRequest getRequest = new GetRequest();
                getRequest.GetRequestNormal = new GetRequestNormal(Clock.GetTimeZoneAttributeDescriptor());
                var dataResult = await Client.GetRequest(getRequest);
                var r = new GetResponse();
                r.PduBytesToConstructor(dataResult);
                Clock.TimeZone = int.Parse(r.GetResponseNormal.Result.Data.ValueDisplay.ValueString);
            });
        }


        public RelayCommand GetTimeCommand
        {
            get => _getTimeCommand;
            set
            {
                _getTimeCommand = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _getTimeCommand;


        public RelayCommand GetTimeZoneCommand
        {
            get => _getTimeZoneCommand;
            set
            {
                _getTimeZoneCommand = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _getTimeZoneCommand;
    }
}