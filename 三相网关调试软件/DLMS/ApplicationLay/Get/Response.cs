﻿using 三相智慧能源网关调试软件.DLMS.ApplicationLay.ApplicationLayEnums;

namespace 三相智慧能源网关调试软件.DLMS.ApplicationLay.Get
{
    public class Response 
    {
        public GetResponse GetResponse { get; set; }
        public ExceptionResponse ExceptionResponse { get; set; }

        public bool PduStringInHexConstructor(ref string pduStringInHex)
        {
            string a = pduStringInHex.Substring(0, 2);
            if (a == "C4")
            {
                GetResponse=new GetResponse();
                return GetResponse.PduStringInHexConstructor(ref pduStringInHex);
            }

            if (a == "D8")
            {
                ExceptionResponse = new ExceptionResponse();
                return ExceptionResponse.PduStringInHexConstructor(ref pduStringInHex);
            }

            return false;
        }
    }
}