using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class Mission
    {
        [DataMember(Name = "id")]
        public string id { get; set; }

        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "description")]
        public string description { get; set; }

        [DataMember(Name = "createTs")]
        public long createTs { get; set; }

        [DataMember(Name = "modTs")]
        public long modTs { get; set; }

        [DataMember(Name = "recoveryPoints")]
        public RecoveryPoint[] recoveryPoints { get; set; }

        [DataMember(Name = "launchers")]
        public Launcher[] launchers { get; set; }

        [DataMember(Name = "config")]
        public MissionConfig config { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Mission));

        public static Mission Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static Mission Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (Mission)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
