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
    public class RecoveryPoint
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "position")]
        public Position position { get; set; }

        [DataMember(Name = "alt")]
        public double alt { get; set; }

        [DataMember(Name = "cleanupTime")]
        public double cleanupTime { get; set; }

        [DataMember(Name = "state")]
        public string state { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(RecoveryPoint));

        public static RecoveryPoint Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static RecoveryPoint Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (RecoveryPoint)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
