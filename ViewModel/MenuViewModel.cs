﻿#define 张诗华
#undef 张诗华
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using 三相智慧能源网关调试软件.Model;

namespace 三相智慧能源网关调试软件.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        public MenuViewModel()
        {
            if (IsInDesignMode)
            {
                ManagementMenuCollection = new ObservableCollection<MenuModel>
                {
                    new MenuModel() {Title = "命令", FontSize = "20", IconFont = "\xe7b7",},
                    new MenuModel() {Title = "系统日志", FontSize = "20", IconFont = "\xe668",},
                    new MenuModel() {Title = "实时数据", FontSize = "20", IconFont = "\xe6ab",},
                    new MenuModel() {Title = "全局参数", FontSize = "20", IconFont = "\xe606",},
                    new MenuModel() {Title = "命令", FontSize = "20", IconFont = "\xe749",},
                    new MenuModel() {Title = "命令", FontSize = "20", IconFont = "\xe749",}
                };

                BaseMeterMenuCollection = new ObservableCollection<MenuModel>()
                {
                    new MenuModel()
                    {
                        Title = "基表升级", FontSize = "20", IconFont = "\xe600", Assembly = "UpGradeBaseMeterPage",
                        Foreground = "#00FF00"
                    },

                    new MenuModel()
                    {
                        Title = "Telnet", FontSize = "20", IconFont = "\xe600", Assembly = "TelnetPage",
                        Foreground = "#00FF00"
                    },
                };
                SelectCommand = new RelayCommand<MenuModel>(Select);
            }

            else
            {
#if 张诗华
                BaseMeterMenuCollection = new ObservableCollection<MenuModel>()
                {
                    new MenuModel()
                    {
                        Title = "基表串口", FontSize = "20", IconFont = "\xe66c", Assembly = "BaseMeter.SerialPortPage",
                        Foreground = "#FF0000"
                    },

                    new MenuModel()
                    {
                        Title = "基表升级", FontSize = "20", IconFont = "\xe600",
                        Assembly = "BaseMeter.UpGradeBaseMeterPage",
                        Foreground = "#00FF00"
                    },
                };

                ServicesMenuCollection = new ObservableCollection<MenuModel>()
                {
                    new MenuModel()
                    {
                        Title = "Telnet", FontSize = "20", IconFont = "\xe6ee", Assembly = "ServerCenter.TelnetPage",
                        Foreground = "#FF0000"
                    },
                };
#else
                ManagementMenuCollection = new ObservableCollection<MenuModel>
                {
                    new MenuModel()
                    {
                        Title = "网关应用", FontSize = "20", IconFont = "\xe7b7", Assembly = "Management.AppPage",
                        Foreground = "#FF0000"
                    },
                    new MenuModel()
                    {
                        Title = "实时数据", FontSize = "20", IconFont = "\xe6ab", Assembly = "Management.RealTimeDataPage",
                        Foreground = "#6666FF"
                    },
                    new MenuModel()
                    {
                        Title = "LiveChart", FontSize = "20", IconFont = "\xe637",
                        Assembly = "Management.InstantDataLiveChartPage", Foreground = "#0033FF"
                    },
                    new MenuModel()
                    {
                        Title = "全局参数", FontSize = "20", IconFont = "\xe606", Assembly = "Management.GlobalParameterPage",
                        Foreground = "#00CCFF"
                    },
                    new MenuModel()
                    {
                        Title = "命令", FontSize = "20", IconFont = "\xe749", Assembly = "Management.CommandPage",
                        Foreground = "#CC6600"
                    },
                    new MenuModel()
                        {Title = "系统日志", FontSize = "20", IconFont = "\xe668", Assembly = "Management.LogPage", Foreground = "#00BB00"},
                };


                BaseMeterMenuCollection = new ObservableCollection<MenuModel>()
                {
                    new MenuModel()
                    {
                        Title = "基表串口", FontSize = "20", IconFont = "\xe66c", Assembly = "BaseMeter.SerialPortPage",
                        Foreground = "#FF0000"
                    },
                    new MenuModel()
                    {
                        Title = "DLMSSettings", FontSize = "20", IconFont = "\xe606",
                        Assembly = "BaseMeter.HDLCFrameParameterPage",
                        Foreground = "#FF0000"
                    },
                    new MenuModel()
                    {
                        Title = "基表配置", FontSize = "20", IconFont = "\xe600",
                        Assembly = "BaseMeter.UpGradeBaseMeterPage",
                        Foreground = "#00FF00"
                    },
                };

                ServicesMenuCollection = new ObservableCollection<MenuModel>()
                {
                    new MenuModel()
                    {
                        Title = "Telnet", FontSize = "20", IconFont = "\xe6ee", Assembly = "ServerCenter.TelnetPage",
                        Foreground = "#FF0000"
                    },

                    new MenuModel()
                    {
                        Title = "TFTPServer", FontSize = "20", IconFont = "\xe619",
                        Assembly = "ServerCenter.TftpServerPage",
                        Foreground = "#0000FF"
                    },
                    new MenuModel()
                    {
                        Title = "泰昂", FontSize = "20", IconFont = "",
                        Assembly = "IntelligentEquipment.TaiAngPage",
                        Foreground = "#00FF00"
                    },
                };
#endif
                SelectCommand = new RelayCommand<MenuModel>(Select);
            }
        }

        private ObservableCollection<MenuModel> _managementMenuCollection;

        public ObservableCollection<MenuModel> ManagementMenuCollection
        {
            get => _managementMenuCollection;
            set
            {
                _managementMenuCollection = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<MenuModel> _baseMeterMenuCollection;

        public ObservableCollection<MenuModel> BaseMeterMenuCollection
        {
            get => _baseMeterMenuCollection;
            set
            {
                _baseMeterMenuCollection = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<MenuModel> _servicesMenuCollection;

        public ObservableCollection<MenuModel> ServicesMenuCollection
        {
            get => _servicesMenuCollection;
            set
            {
                _servicesMenuCollection = value;
                RaisePropertyChanged();
            }
        }



        private MenuModel _menuModel;

        public MenuModel MenuModel
        {
            get => _menuModel;
            set
            {
                _menuModel = value;
                RaisePropertyChanged();
            }
        }

        private Page _currentPage;

        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand<MenuModel> _selectCommand;

        public RelayCommand<MenuModel> SelectCommand
        {
            get => _selectCommand;
            set
            {
                _selectCommand = value;
                RaisePropertyChanged();
            }
        }


        private void Select(MenuModel menuModel)
        {
            MenuModel = menuModel;
            Type type = GetType();
            Assembly assembly = type.Assembly;
            CurrentPage =  assembly.CreateInstance("三相智慧能源网关调试软件.View" + "." + MenuModel.Assembly) as Page;
        }
    }
}