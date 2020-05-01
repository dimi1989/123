﻿namespace 三相智慧能源网关调试软件.DLMS._21EMode
{

    public class EMode
    {
        internal const char StartChar = '/';

        internal const char RequestChar = '?';

        internal const char EndChar = '!';

        private string _devicestr = "0010";

        internal const char CompletCr = '\r';

        internal const char CompletLf = '\n';

        internal const byte Ack = 6;

        public string _baudZ;

        internal const char Delimiter = '\\';

        private static string _propBaud;

        public string Devicestr
        {
            get { return _devicestr; }
            set
            {
                if (value.Length <= 32)
                {
                    _devicestr = value;
                }
            }
        }

        public int AckBaudZ
        {
            get
            {
                switch (_baudZ)
                {
                    case "0":
                        return 300;
                    case "1":
                        return 600;
                    case "2":
                        return 1200;
                    case "3":
                        return 2400;
                    case "4":
                        return 4800;
                    case "5":
                        return 9600;
                    case "6":
                        return 19200;
                    case "7":
                        return 38400;
                    case "8":
                        return 57600;
                    case "9":
                        return 115200;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 300:
                        _baudZ = "0";
                        break;
                    case 600:
                        _baudZ = "1";
                        break;
                    case 1200:
                        _baudZ = "2";
                        break;
                    case 2400:
                        _baudZ = "3";
                        break;
                    case 4800:
                        _baudZ = "4";
                        break;
                    case 9600:
                        _baudZ = "5";
                        break;
                    case 19200:
                        _baudZ = "6";
                        break;
                    case 38400:
                        _baudZ = "7";
                        break;
                    case 57600:
                        _baudZ = "8";
                        break;
                    case 115200:
                        _baudZ = "9";
                        break;
                    default:
                        _baudZ = "5";
                        break;
                }
            }
        }

        public static int PropMaxBaud
        {
            get
            {
                switch (_propBaud)
                {
                    case "0":
                        return 300;
                    case "1":
                        return 600;
                    case "2":
                        return 1200;
                    case "3":
                        return 2400;
                    case "4":
                        return 4800;
                    case "5":
                        return 9600;
                    case "6":
                        return 19200;
                    case "7":
                        return 38400;
                    case "8":
                        return 57600;
                    case "9":
                        return 115200;
                    default:
                        return 0;
                }
            }
            set
            {
                switch (value)
                {
                    case 300:
                        _propBaud = "0";
                        break;
                    case 600:
                        _propBaud = "1";
                        break;
                    case 1200:
                        _propBaud = "2";
                        break;
                    case 2400:
                        _propBaud = "3";
                        break;
                    case 4800:
                        _propBaud = "4";
                        break;
                    case 9600:
                        _propBaud = "5";
                        break;
                    case 19200:
                        _propBaud = "6";
                        break;
                    case 38400:
                        _propBaud = "7";
                        break;
                    case 57600:
                        _propBaud = "8";
                        break;
                    case 115200:
                        _propBaud = "9";
                        break;
                    default:
                        _propBaud = "5";
                        break;
                }
            }
        }

        public EMode(int requestBaud, string deviceStr = "")
        {
            Devicestr = deviceStr;
            AckBaudZ = requestBaud;
        }
    }
}
	