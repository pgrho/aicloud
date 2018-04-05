using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Shipwreck.AICloud
{
    [Serializable]
    public class AICloudException : Exception
    {
        public AICloudException()
            : this(ErrorCode.Other)
        {
        }

        public AICloudException(ErrorCode code = ErrorCode.Other, string rawMessage = null, string rawDetail = null)
            : base("AITalk Web APIがエラーを返しました。")
        {
            Code = code;
            RawMessage = rawMessage;
            RawDetail = rawDetail;
        }

        public AICloudException(string message)
            : this(message, ErrorCode.Other)
        {
        }

        public AICloudException(string message, ErrorCode code = ErrorCode.Other, string rawMessage = null, string rawDetail = null)
            : base(message ?? "AITalk Web APIがエラーを返しました。")
        {
            Code = code;
            RawMessage = rawMessage;
            RawDetail = rawDetail;
        }

        public AICloudException(string message, Exception innerException)
            : this(message, innerException, ErrorCode.Other)
        {
        }

        public AICloudException(string message, Exception innerException, ErrorCode code = ErrorCode.Other, string rawMessage = null, string rawDetail = null)
            : base(message ?? "AITalk Web APIがエラーを返しました。", innerException)
        {
            Code = code;
            RawMessage = rawMessage;
            RawDetail = rawDetail;
        }

        protected AICloudException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Code = (ErrorCode)info.GetInt32(nameof(Code));
            RawMessage = info.GetString(nameof(RawMessage));
            RawDetail = info.GetString(nameof(RawDetail));
        }

        public ErrorCode Code { get; }

        public string RawMessage { get; }

        public string RawDetail { get; }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue(nameof(Code), (int)Code);

            if (RawMessage != null)
            {
                info.AddValue(nameof(RawMessage), RawMessage);
            }
            if (RawDetail != null)
            {
                info.AddValue(nameof(RawDetail), RawDetail);
            }

            base.GetObjectData(info, context);
        }
    }
}