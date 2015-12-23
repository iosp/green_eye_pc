using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class Platform
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "sensors")]
        public Sensor[] sensors { get; set; }

        [DataMember(Name = "state")]
        public PlatformState state { get; set; }

        [DataMember(Name = "route")]
        public Route route { get; set; }
    }
}
