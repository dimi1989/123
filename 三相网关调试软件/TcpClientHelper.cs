﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace 三相智慧能源网关调试软件
{
    public class TcpClientHelper : ValidateModelBase
    {
        public Socket ClientSocket { get; set; }
        private readonly byte[] _messageByteServer = new byte[1024];

        [Required(ErrorMessage = "不能为空！")]
        [RegularExpression("^((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})(\\.((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})){3}$$",
            ErrorMessage = "请输入正确的IP地址！")]
        public string LocalIp
        {
            get => _localIp;
            set
            {
                _localIp = value;
                OnPropertyChanged();
            }
        }

        private string _localIp;


        [Required(ErrorMessage = "不能为空！")]
        [RegularExpression("^((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})(\\.((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})){3}$$",
            ErrorMessage = "请输入正确的IP地址！")]
        public string ServerIpAddress
        {
            get => _serverIpAddress;
            set
            {
                _serverIpAddress = value;
                OnPropertyChanged();
            }
        }

        private string _serverIpAddress;


        [Required(ErrorMessage = "不能为空！")]
        public int ServerPortNum
        {
            get => _serverPortNum;
            set
            {
                _serverPortNum = value;
                OnPropertyChanged();
            }
        }

        private int _serverPortNum;
        private IPEndPoint _ipEndPoint;

        public IPEndPoint IpEndPoint
        {
            get => _ipEndPoint;
            set
            {
                _ipEndPoint = value;
                OnPropertyChanged();
            }
        }

        private bool _connectResult;

        public bool ConnectResult
        {
            get => ClientSocket.Connected;
            private set
            {
                _connectResult = value;
                OnPropertyChanged();
            }
        }

        private string _sendMsg;

        public string MySendMessage
        {
            get => _sendMsg;
            set
            {
                _sendMsg = value;
                OnPropertyChanged();
            }
        }

        private string _receiveMsg;

        public string MyReceiveMessage
        {
            get => _receiveMsg;
            set
            {
                _receiveMsg = value;
                OnPropertyChanged();
            }
        }

        public event Action<Socket, byte[]> ReceiveByte;
        public event Action<Socket, byte[]> SendDataToServerByte;

        protected virtual void OnReceiveByte(Socket serverSocket, byte[] bytes)
        {
            ReceiveByte?.Invoke(serverSocket, bytes);
//            Messenger.Default.Send((serverSocket, bytes), "ClientReceiveDataEvent");
            StrongReferenceMessenger.Default.Send((serverSocket, bytes).ToTuple(), "ClientReceiveDataEvent");
        }

        protected virtual void OnSendDataToServerByte(Socket serverSocket, byte[] bytes)
        {
            SendDataToServerByte?.Invoke(serverSocket, bytes);
            //            Messenger.Default.Send((serverSocket, bytes), "ClientSendDataEvent");
            StrongReferenceMessenger.Default.Send((serverSocket, bytes).ToTuple(), "ClientSendDataEvent");
        }

        public TcpClientHelper(string serverIpAddress, int serverPortNum)
        {
            ServerIpAddress = serverIpAddress;
            ServerPortNum = serverPortNum;
            IpEndPoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPortNum);
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LocalIp = GetHostIp();
        }

        public static string GetHostIp()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] addressList = hostEntry.AddressList;
            string result = "";
            if (addressList.Length == 0)
            {
                return result;
            }

            IPAddress[] array = addressList;
            foreach (IPAddress iPAddress in array)
            {
                if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = iPAddress.ToString();
                }
            }

            return result;
        }

        public void ConnectToServer()
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        StrongReferenceMessenger.Default.Send($"{ClientSocket.LocalEndPoint}正在尝试连接{ClientSocket.RemoteEndPoint}",
                            "ClientStatus");
                        ClientSocket.Connect(ServerIpAddress, ServerPortNum);

                        ConnectResult = true;
                        StrongReferenceMessenger.Default.Send($"成功连接至{ClientSocket.RemoteEndPoint}", "ClientStatus");

                        Task.Run(ReceiveData);
                    }
                    catch (Exception e)
                    {
                        ConnectResult = false;
//                        Messenger.Default.Send("连接不成功 from  ConnectToServer()", "ClientStatus");
//                        Messenger.Default.Send("异常" + e.Message + "from  ConnectToServer()", "ClientNetErrorEvent");
                        StrongReferenceMessenger.Default.Send("连接不成功 from  ConnectToServer()", "ClientStatus");
                        StrongReferenceMessenger.Default.Send("异常" + e.Message + "from  ConnectToServer()",
                            "ClientNetErrorEvent");
                    }
                });
            }
            catch (Exception e)
            {
                ConnectResult = false;
                StrongReferenceMessenger.Default.Send("异常" + e.Message + "from  ConnectToServer()", "ClientNetErrorEvent");
                throw;
            }
        }

        private void ReceiveData()
        {
            try
            {
                for (; ClientSocket.Connected;)
                {
                    int receiveDataLen = ClientSocket.Receive(_messageByteServer);
                    bool flag2 = receiveDataLen == 0;
                    if (flag2)
                    {
                        string str2 = $"{DateTime.Now}  {ClientSocket.RemoteEndPoint} 服务端主动断开了当前链接..." + "\r\n";
                        StrongReferenceMessenger.Default.Send(str2, "ClientStatus");
                        Disconnect();
                        break;
                    }

                    byte[] receiveBytes = _messageByteServer.Take(receiveDataLen).ToArray();
                    OnReceiveByte(ClientSocket, receiveBytes);
                }
            }
            catch (Exception e)
            {
                //   Messenger.Default.Send(e.Message + "from  ReceiveData()", "ClientNetErrorEvent");
                StrongReferenceMessenger.Default.Send(e.Message + "from  ReceiveData()", "ClientNetErrorEvent");
            }
        }

        public void SendDataToServer(string inputSendData)
        {
            if (ConnectResult == false)
            {
                return;
            }

            bool flag = inputSendData == null;
            if (flag)
            {
                throw new ArgumentNullException("inputSendData");
            }

            try
            {
                byte[] sendBytes = Encoding.Default.GetBytes(inputSendData);
                ClientSocket.Send(sendBytes);
                MySendMessage += (inputSendData + Environment.NewLine);
                // Messenger.Default.Send(sendBytes, "ClientSendDataEvent");
                StrongReferenceMessenger.Default.Send(sendBytes, "ClientSendDataEvent");
            }
            catch (Exception ex)
            {
                // Messenger.Default.Send(ex.Message, "ClientStatusNetErrorEvent");
                StrongReferenceMessenger.Default.Send(ex.Message, "ClientStatusNetErrorEvent");
            }
        }

        public void SendDataToServer(byte[] inputBytesData)
        {
            bool flag = inputBytesData.Length == 0;
            if (flag)
            {
                throw new ArgumentNullException("inputBytesData");
            }

            try
            {
                ClientSocket.Send(inputBytesData);
                OnSendDataToServerByte(ClientSocket, inputBytesData);
            }
            catch (Exception ex)
            {
                StrongReferenceMessenger.Default.Send("异常" + ex.Message, "ClientNetErrorEvent");
            }
        }

        public void SendDataToServer(Socket socket, byte[] inputBytesData)
        {
            bool flag = inputBytesData.Length == 0;
            if (flag)
            {
                throw new ArgumentNullException("inputBytesData");
            }

            try
            {
                socket.Send(inputBytesData);
                OnSendDataToServerByte(socket, inputBytesData);
            }
            catch (Exception ex)
            {
                //Messenger.Default.Send("异常" + ex.Message, "ClientNetErrorEvent");
                StrongReferenceMessenger.Default.Send("异常" + ex.Message, "ClientNetErrorEvent");
            }
        }

        /// <summary>
        /// 发送数据至服务端，并加上换行符
        /// </summary>
        /// <param name="inputSendData"></param>
        public void SendDataToServerWithNewLine(string inputSendData)
        {
            if (ConnectResult == false)
            {
                return;
            }

            bool flag = inputSendData == null;
            if (flag)
            {
                throw new ArgumentNullException("inputSendData");
            }

            try
            {
                byte[] sendBytes = Encoding.Default.GetBytes(inputSendData + Environment.NewLine);
                ClientSocket.Send(sendBytes);
                OnSendDataToServerByte(ClientSocket, sendBytes);
            }
            catch (Exception ex)
            {
                StrongReferenceMessenger.Default.Send(ex.Message, "ClientErrorEvent");
            }
        }

        public void Disconnect()
        {
            if (ConnectResult == false)
            {
                return;
            }

            ClientSocket.Disconnect(false);
            ConnectResult = false;
//            Messenger.Default.Send("关闭连接成功", "ClientStatus");
            StrongReferenceMessenger.Default.Send("关闭连接成功", "ClientStatus");
        }

        public void CloseAll()
        {
            if (ConnectResult == false)
            {
                return;
            }

            ClientSocket?.Close();
            ConnectResult = false;
//            Messenger.Default.Send("关闭连接成功", "ClientStatus");
            StrongReferenceMessenger.Default.Send("关闭连接成功", "ClientStatus");
        }

        public void Dispose()
        {
            ((IDisposable) ClientSocket).Dispose();
        }
    }
}