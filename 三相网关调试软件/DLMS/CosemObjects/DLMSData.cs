﻿using 三相智慧能源网关调试软件.DLMS.ApplicationLayEnums;

namespace 三相智慧能源网关调试软件.DLMS.CosemObjects
{
    public class DLMSData : DLMSObject, IDLMSBase
    {
        public object Value { get; set; }

        public DLMSData(string logicalName)
        {
            LogicalName = logicalName;
            ObjectType = ObjectType.Data;
        }

        public byte[] GetValue() => GetAttributeData(2);

        public byte[] SetValue(DLMSDataItem ddi) => SetAttributeData(2, ddi);
        public string[] GetNames() => new[] {LogicalName, "Value"};

        public int GetAttributeCount() => 2;


        public int GetMethodCount() => 0;


        public DataType GetDataType(int index)
        {
            return new DataType();
            //switch (index)
            //{
            //    case 1:
            //        return DataType.OctetString;
            //    case 2:
            //    {
            //        DataType dataType = base.GetDataType(index);
            //        if (dataType == DataType.None && Value != null)
            //        {
            //            dataType = GXCommon.GetDLMSDataType(Value.GetType());
            //        }
            //        return dataType;
            //    }
            //    default:
            //        throw new ArgumentException("GetDataType failed. Invalid attribute index.");
            //}
        }
    }
}