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
    [CollectionDataContract(Name = "World", ItemName = "Platform")]
    public class World: List<Platform>
    {
        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(World));

        public static World Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static World Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (World)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
