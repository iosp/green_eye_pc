using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class Gateway
    {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "start")]
        public Position[] start { get; set; }

        [DataMember(Name = "end")]
        public Position[] end { get; set; }

        [DataMember(Name = "minAlt")]
        public float minAlt { get; set; }

        [DataMember(Name = "maxAlt")]
        public float maxAlt { get; set; }

        [DataMember(Name = "inbound")]
        public bool inbound { get; set; }
    }
}
