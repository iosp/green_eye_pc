using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class MissionConfig
    {
        [DataMember(Name = "maxActivePlatforms")]
        public long maxActivePlatforms { get; set; }

        [DataMember(Name = "duration")]
        public long duration { get; set; }

        [DataMember(Name = "workingHeight")]
        public double workingHeight { get; set; }
    }
}
