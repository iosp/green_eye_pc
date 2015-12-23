using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "Cell", ItemName = "Position")]
    public class Cell: List<Position>
    {
        private static DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Cell));

        public static Cell Deserialize(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException("json");
            }

            return Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(json)));
        }

        public static Cell Deserialize(Stream jsonStream)
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException("jsonStream");
            }

            return (Cell)jsonSerializer.ReadObject(jsonStream);
        }
    }
}
