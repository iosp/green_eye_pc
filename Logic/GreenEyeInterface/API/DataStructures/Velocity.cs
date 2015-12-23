using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "Velocity", ItemName = "double")]
    public class Velocity: List<double>
    {
    }
}
