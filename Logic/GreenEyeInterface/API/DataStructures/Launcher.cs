using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class Launcher
    {
        [DataMember(Name = "id")]
        public int id { get; set; }

        [DataMember(Name = "position")]
        public Position position { get; set; }

        [DataMember(Name = "alt")]
        public double alt { get; set; }

        [DataMember(Name = "cleanupTime")]
        public double cleanupTime { get; set; }

        [DataMember(Name = "platforms")]
        public Platform[] platforms { get; set; }

    }
}
