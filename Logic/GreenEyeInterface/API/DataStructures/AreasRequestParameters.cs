using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class AreasRequestParameters
    {
        [DataMember(Name = "polygon")]
        public PoligonGeoJson polygon { get; set; }

        [DataMember(Name = "missionConfig")]
        public MissionConfig missionConfig { get; set; }

        [DataMember(Name = "platforms")]
        public Platform[] platforms { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(AreasRequestParameters));

        public static AreasRequestParameters Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static AreasRequestParameters Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (AreasRequestParameters)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
