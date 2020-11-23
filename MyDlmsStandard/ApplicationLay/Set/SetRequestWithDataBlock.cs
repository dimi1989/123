﻿using System.Xml.Serialization;
using MyDlmsStandard.ApplicationLay.ApplicationLayEnums;
using MyDlmsStandard.Axdr;

namespace MyDlmsStandard.ApplicationLay.Set
{
    public class SetRequestWithDataBlock : ISetRequest
    {
        [XmlIgnore] public SetRequestType SetRequestType { get; } = SetRequestType.WithDataBlock;
        public AxdrIntegerUnsigned8 InvokeIdAndPriority { get; set; }
        public DataBlockSA DataBlockSA { get; set; }


        public string ToPduStringInHex()
        {
            return "03" + InvokeIdAndPriority.ToPduStringInHex() + DataBlockSA.ToPduStringInHex();
        }
    }
}