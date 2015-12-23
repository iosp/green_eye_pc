using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class PlatformState
    {
        [DataMember(Name = "position")]
        public Position position { get; set; }

        [DataMember(Name = "alt")]
        public long alt { get; set; }

        [DataMember(Name = "velocity")]
        public Velocity velocity { get; set; }

        [DataMember(Name = "deployState")]
        public string deployState { get; set; }

        [DataMember(Name = "missionState")]
        public string missionState { get; set; }

        [DataMember(Name = "failureState")]
        public string failureState { get; set; }

        [DataMember(Name = "energy")]
        public double energy { get; set; }
    }
}
