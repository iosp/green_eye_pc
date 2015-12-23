using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GreenEyeAPI.Core.DataStructures
{
    [DataContract]
    public class Command
    {
        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Command));

        public static Command Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static Command Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (Command)jsonSerializer.ReadObject(jsonStream);
        }

    }
}
