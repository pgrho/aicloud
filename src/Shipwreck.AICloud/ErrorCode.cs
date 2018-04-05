namespace Shipwreck.AICloud
{
    public enum ErrorCode
    {
        UserDoesNotExist = 1001,
        IncorrectPassword = 1002,
        InvalidParameter = 1003,
        SoundFileDoesNotExist = 1004,
        ExceedsTextMaxLength = 1005,
        AuthenticationFailed = 1101,
        InvalidContract = 1102,
        ExceedsQuota = 1103,
        TtsServerSelectionFailed = 1201,
        TtsFailed = 1202,
        SqlFailed = 1203,
        ResponseFailed = 1204,
        Other = 9999,
    }
}