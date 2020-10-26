﻿using System;
using System.Linq;
using ClassLibraryDLMS.DLMS.ApplicationLay.ApplicationLayEnums;
using ClassLibraryDLMS.DLMS.Axdr;
using ClassLibraryDLMS.DLMS.Common;

namespace ClassLibraryDLMS.DLMS.ApplicationLay.Association
{
    public class UserInformation
    {
        public InitiateRequest InitiateRequest { get; set; }

        public byte[] ToPduBytes()
        {
            return InitiateRequest.ToPduBytes();
        }
    }

    public class InitiateResponse : IPduBytesToConstructor
    {
        public AxdrUnsigned8 NegotiatedDlmsVersionNumber { get; set; }
       public Conformance NegotiatedConformance { get; set; }

        public AxdrUnsigned16 ServerMaxReceivePduSize { get; set; }
        public AxdrInteger16 VaaName { get; set; }


        public bool PduBytesToConstructor(byte[] pduBytes)
        {
            if (pduBytes[0] != 0xBE) return false;
            if (pduBytes[1] > pduBytes.Length - 2) return false;
            pduBytes = pduBytes.Skip(2).ToArray();
            var pdustring = MyConvert.ByteArrayToOctetString(pduBytes.ToArray());
            if (pduBytes[0] == 0x04) //user-information(OCTETSTRING,Universal)选项的编码
            {
                var pdu = pduBytes.Skip(2).Take(pduBytes[1]).ToArray();
                if (pdu[0] == 0x08)
                {
                    //negotiated - quality - of - service  pdu[1];

                    NegotiatedDlmsVersionNumber = new AxdrUnsigned8();
                    var pduStringInHex = MyConvert.ByteArrayToOctetString(pdu.Skip(2).ToArray()) ;
                    if (!NegotiatedDlmsVersionNumber.PduStringInHexConstructor(ref pduStringInHex))
                    {
                        return false;
                    }

                    if (pduStringInHex.StartsWith("5F1F04"))
                    {
                        pduStringInHex = pduStringInHex.Substring(6);
                        var nego =MyConvert.OctetStringToByteArray(pduStringInHex.Substring(0, 8));
                        var negovalue = BitConverter.ToUInt32(nego.Reverse().ToArray(), 0);
                        NegotiatedConformance = (Conformance) negovalue;
                        pduStringInHex = pduStringInHex.Substring(8);
                        ServerMaxReceivePduSize = new AxdrUnsigned16();
                        ServerMaxReceivePduSize.PduStringInHexConstructor(ref pduStringInHex);
                        VaaName = new AxdrInteger16();
                        VaaName.PduStringInHexConstructor(ref pduStringInHex);
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }
    }
}