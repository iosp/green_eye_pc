using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class MissionArea
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "start")]
        public PoligonGeoJson[] polygon { get; set; }

        [DataMember(Name = "inGateways")]
        public Gateway[] inGateways { get; set; }

        [DataMember(Name = "outGateways")]
        public Gateway[] outGateways { get; set; }

        [DataMember(Name = "emergencyGateways")]
        public Gateway[] emergencyGateways { get; set; }
    }
}
