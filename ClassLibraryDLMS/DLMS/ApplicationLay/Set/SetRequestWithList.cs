﻿using System.Text;
using ClassLibraryDLMS.DLMS.Axdr;

namespace ClassLibraryDLMS.DLMS.ApplicationLay.Set
{
    public class SetRequestWithList
    {
        public AxdrUnsigned8 InvokeIdAndPriority { get; set; }
        public CosemAttributeDescriptorWithSelection[] AttributeDescriptorList { get; set; }
        public DLMSDataItem[] ValueList { get; set; }
        public string ToPduStringInHex()
        {
			StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(InvokeIdAndPriority.ToPduStringInHex());
            int num = AttributeDescriptorList.Length;
            if (num <= 127)
            {
                stringBuilder.Append(num.ToString("X2"));
            }
            else if (num <= 255)
            {
                stringBuilder.Append("81" + num.ToString("X2"));
            }
            else
            {
                stringBuilder.Append("82" + num.ToString("X4"));
            }
            CosemAttributeDescriptorWithSelection[] array = AttributeDescriptorList;
            foreach (CosemAttributeDescriptorWithSelection cosemAttributeDescriptorWithSelection in array)
            {
                stringBuilder.Append(cosemAttributeDescriptorWithSelection.ToPduStringInHex());
            }
            num = ValueList.Length;
            if (num <= 127)
            {
                stringBuilder.Append(num.ToString("X2"));
            }
            else if (num <= 255)
            {
                stringBuilder.Append("81" + num.ToString("X2"));
            }
            else
            {
                stringBuilder.Append("82" + num.ToString("X4"));
            }
            DLMSDataItem[] array2 = ValueList;
            foreach (DLMSDataItem dlmsDataItem in array2)
            {
                stringBuilder.Append(dlmsDataItem.ToPduStringInHex());
            }
            return stringBuilder.ToString();
		}
    }
}