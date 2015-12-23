using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "Polygon", ItemName = "LinearRing")]
    public class Polygon: List<LinearRing>
    {
    }
}
