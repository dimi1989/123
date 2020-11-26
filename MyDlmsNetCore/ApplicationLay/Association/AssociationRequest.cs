﻿using System.Collections.Generic;
using MyDlmsNetCore.ApplicationLay.ApplicationLayEnums;

//using 三相智慧能源网关调试软件.ViewModel.DlmsViewModels;


namespace MyDlmsNetCore.ApplicationLay.Association
{
    public class AssociationRequest : IToPduBytes
    {
        public ApplicationContextName ApplicationContextName { get; set; }
        public CallingAPTitle CallingAPTitle { get; set; }
        public SenderACSERequirements SenderACSERequirements { get; set; }
        public MechanismName MechanismName { get; set; }
        public CallingAuthenticationValue CallingAuthenticationValue { get; set; }
        public InitiateRequest InitiateRequest { get; set; }

        public AssociationRequest()
        {
        }

        public AssociationRequest(byte[] passWorld, ushort maxReceivePduSize, byte dlmsVersion,string systemTitle,Conformance conformance)
        {
            ApplicationContextName = new ApplicationContextName();
            MechanismName = new MechanismName();
            SenderACSERequirements = new SenderACSERequirements();
            CallingAuthenticationValue = new CallingAuthenticationValue(passWorld);
            CallingAPTitle = new CallingAPTitle(systemTitle);
            InitiateRequest = new InitiateRequest(maxReceivePduSize, dlmsVersion,conformance);
        }

//        public AssociationRequest(DlmsSettingsViewModel dlmsSettingsViewModel)
//        {
//            ApplicationContextName = new ApplicationContextName();
//            MechanismName = new MechanismName();
//            SenderACSERequirements = new SenderACSERequirements();
//            CallingAuthenticationValue = new CallingAuthenticationValue(dlmsSettingsViewModel);
//            CallingAPTitle = new CallingAPTitle(dlmsSettingsViewModel.SystemTitle);
//            InitiateRequest = new InitiateRequest(dlmsSettingsViewModel);
//        }

        /// <summary>
        /// Application Association Request 应用连接请求    
        ///对应 Application Association Response 应用连接响应
        /// </summary>
        /// <returns>Application Association Request报文字节</returns>

        public byte[] ToPduBytes()
        {
            List<byte> appApduAssociationRequest = new List<byte>();
            if (ApplicationContextName != null)
                appApduAssociationRequest.AddRange(ApplicationContextName.ToPduBytes());
            if (CallingAPTitle != null)
            {
                appApduAssociationRequest.AddRange(CallingAPTitle.ToPduBytes());
            }

            if (SenderACSERequirements != null)
            {
                appApduAssociationRequest.AddRange(SenderACSERequirements.ToPduBytes());
            }

            if (MechanismName != null)
            {
                appApduAssociationRequest.AddRange(MechanismName.ToPduBytes());
            }

            if (CallingAuthenticationValue != null)
            {
                appApduAssociationRequest.AddRange(CallingAuthenticationValue.ToPduBytes());
            }

            if (InitiateRequest != null)
            {
                appApduAssociationRequest.AddRange(InitiateRequest.ToPduBytes());
            }
            appApduAssociationRequest.InsertRange(0, new byte[] { (byte)Command.Aarq, (byte)appApduAssociationRequest.Count });
            return appApduAssociationRequest.ToArray();
        }
    }
}