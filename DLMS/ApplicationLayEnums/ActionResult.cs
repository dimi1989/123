﻿namespace 三相智慧能源网关调试软件.DLMS.ApplicationLayEnums
{
    public enum ActionResult
    {
        Success = 0,
        HardwareFault = 1,
        TemporaryFailure = 2,
        ReadWriteDenied = 3,
        ObjectUndefined = 4,
        ObjectClassInconsistent = 9,
        ObjectUnavailable = 11,
        TypeUnmatched = 12,
        ScopeOfAccessViolated = 13,
        DataBlockUnavailable = 14,
        LongActionAborted = 15,
        NoLongActionInProgress = 16,
        OtherReason = 250
    }
}