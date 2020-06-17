﻿using System.Collections.Generic;
using 三相智慧能源网关调试软件.DLMS.ApplicationLay.ApplicationLayEnums;

namespace 三相智慧能源网关调试软件.DLMS.ApplicationLay
{
    public class DLMSDataItem:IToPduBytes
    {
        public DataType DataType { get; set; }
        public byte[] ValueBytes { get; set; }

        public DLMSDataItem(DataType dataType, byte[] valueBytes)
        {
            DataType = dataType;
            ValueBytes = valueBytes;
        }
        public byte[] ToPduBytes()
        {
            var list = new List<byte> {(byte) DataType};
            list.AddRange(ValueBytes);
            return list.ToArray();
        }
    }
}