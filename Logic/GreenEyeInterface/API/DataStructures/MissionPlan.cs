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
    public class MissionPlan
    {
        [DataMember(Name = "missionId")]
        public string missionId { get; set; }

        [DataMember(Name = "cells")]
        public Cell[] cells { get; set; }

        [DataMember(Name = "platforms")]
        public Platform[] platforms { get; set; }

        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(MissionPlan));

        public static MissionPlan Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static MissionPlan Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (MissionPlan)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
