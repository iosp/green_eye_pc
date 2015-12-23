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
    public class PoligonGeoJson
    {
        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "geometry")]
        public PoligonGeometry geometry { get; set; }

        [DataMember(Name = "properties")]
        public object properties { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PoligonGeoJson));

        public static PoligonGeoJson Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static PoligonGeoJson Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (PoligonGeoJson)jsonSerializer.ReadObject(jsonStream);
        }

    }
}
