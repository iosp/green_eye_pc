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
    public class Sensor
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "active")]
        public bool active { get; set; }

        [DataMember(Name = "hfov")]
        public double hfov { get; set; }

        [DataMember(Name = "vfov")]
        public double vfov { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Sensor));

        public static Sensor Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static Sensor Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (Sensor)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
