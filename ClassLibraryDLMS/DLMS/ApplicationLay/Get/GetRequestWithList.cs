﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using ClassLibraryDLMS.DLMS.ApplicationLay.ApplicationLayEnums;
using ClassLibraryDLMS.DLMS.Axdr;
using ClassLibraryDLMS.DLMS.Common;

namespace ClassLibraryDLMS.DLMS.ApplicationLay.Get
{
    public class GetRequestWithList : IToPduBytes
    {
        [XmlIgnore] protected GetRequestType GetRequestType { get; set; } = GetRequestType.WithList;
        public AxdrUnsigned8 InvokeIdAndPriority { get; set; }

        public CosemAttributeDescriptorWithSelection[] AttributeDescriptorList { get; set; }

        public GetRequestWithList()
        {
        }

        public GetRequestWithList(CosemAttributeDescriptorWithSelection[] attributeDescriptorList)
        {
            AttributeDescriptorList = attributeDescriptorList;
            InvokeIdAndPriority = new AxdrUnsigned8("C1");
        }

        public byte[] ToPduBytes()
        {
            List<byte> pduBytes = new List<byte>();

            pduBytes.Add((byte) GetRequestType);
            pduBytes.Add(InvokeIdAndPriority.GetEntityValue());
            int num = AttributeDescriptorList.Length;
            if (num < 127)
            {
                pduBytes.Add((byte) num);
            }
            else if (num < 255)
            {
                pduBytes.Add(0x81);
                pduBytes.Add((byte) num);
            }
            else
            {
                pduBytes.Add(0x82);

                pduBytes.AddRange(BitConverter.GetBytes((byte) num).Reverse().ToArray());
            }

            if (AttributeDescriptorList != null)
            {
                foreach (var cosemAttributeDescriptor in AttributeDescriptorList)
                {
                    pduBytes.AddRange(MyConvert.OctetStringToByteArray(cosemAttributeDescriptor.ToPduStringInHex()));
                }
            }

            return pduBytes.ToArray();
        }
    }
}