﻿using ClassLibraryDLMS.DLMS.Axdr;

namespace ClassLibraryDLMS.DLMS.ApplicationLay.Set
{
    public class SetResponseForDataBlock:IPduStringInHexConstructor
    {
        public AxdrUnsigned8 InvokeIdAndPriority { get; set; }
        public AxdrUnsigned32 BlockNumber { get; set; }

        public char ToPduStringInHex()
        {
            throw new System.NotImplementedException();
        }

        public bool PduStringInHexConstructor(ref string pduStringInHex)
        {
			if (string.IsNullOrEmpty(pduStringInHex))
            {
                return false;
            }
            InvokeIdAndPriority = new AxdrUnsigned8();
            if (!InvokeIdAndPriority.PduStringInHexConstructor(ref pduStringInHex))
            {
                return false;
            }
            BlockNumber = new AxdrUnsigned32();
            if (!BlockNumber.PduStringInHexConstructor(ref pduStringInHex))
            {
                return false;
            }
            return true;
		}
    }
}