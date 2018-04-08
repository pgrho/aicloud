using System;
using System.Runtime.Serialization;

namespace Shipwreck.AICloud
{
    [DataContract]
    public class AICloudCounts
    {
        [DataMember]
        public DateTime BaseDate { get; set; }

        [DataMember]
        public int BaseCount { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember ]
        public int OverCount { get; set; }

        [DataMember]
        public int OverCost { get; set; }

        [DataMember]
        public bool OverFlag { get; set; }
    }
}