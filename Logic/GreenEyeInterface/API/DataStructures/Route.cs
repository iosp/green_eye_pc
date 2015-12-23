using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "Route", ItemName = "Waypoint")]
    public class Route: List<Waypoint>
    {
    }
}
