using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "LinearRing", ItemName = "Position")]
    public class LinearRing: List<Position>
    {
    }
}
