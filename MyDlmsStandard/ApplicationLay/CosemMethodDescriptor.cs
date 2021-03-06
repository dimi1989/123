﻿using MyDlmsStandard.Axdr;

namespace MyDlmsStandard.ApplicationLay
{
    public class CosemMethodDescriptor : IToPduStringInHex, IPduStringInHexConstructor
    {
        public AxdrIntegerUnsigned16 CosemClassId { get; set; }

        public AxdrOctetStringFixed CosemObjectInstanceId { get; set; }
        /// <summary>
        /// 方法ID
        /// </summary>
        public AxdrInteger8 CosemObjectMethodId { get; set; }

        public int Length => CalculateLength();

        private int CalculateLength()
        {
            int num = 0;
            if (CosemClassId != null)
            {
                num += CosemClassId.Length;
            }

            if (CosemObjectInstanceId != null)
            {
                num += CosemObjectInstanceId.Length;
            }

            if (CosemObjectMethodId != null)
            {
                num += CosemObjectMethodId.Length;
            }

            return num;
        }

        public CosemMethodDescriptor()
        {
        }

        public CosemMethodDescriptor(AxdrIntegerUnsigned16 cosemClassId, AxdrOctetStringFixed cosemObjectInstanceId,
            AxdrInteger8 cosemObjectMethodId)
        {
            CosemClassId = cosemClassId;
            CosemObjectInstanceId = cosemObjectInstanceId;
            CosemObjectMethodId = cosemObjectMethodId;
        }

        public string ToPduStringInHex()
        {
            return CosemClassId.ToPduStringInHex() + CosemObjectInstanceId.ToPduStringInHex() +
                   CosemObjectMethodId.ToPduStringInHex();
        }

        public bool PduStringInHexConstructor(ref string pduStringInHex)
        {
            CosemClassId = new AxdrIntegerUnsigned16();
            if (!CosemClassId.PduStringInHexConstructor(ref pduStringInHex))
            {
                return false;
            }

            CosemObjectInstanceId = new AxdrOctetStringFixed(6);
            if (!CosemObjectInstanceId.PduStringInHexConstructor(ref pduStringInHex))
            {
                return false;
            }

            CosemObjectMethodId = new AxdrInteger8();
            if (!CosemObjectMethodId.PduStringInHexConstructor(ref pduStringInHex))
            {
                return false;
            }

            return true;
        }
    }
}