using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenEyeAPI.Core.DataStructures
{
    [CollectionDataContract(Name = "Position", ItemName = "double")]
    public class Position: List<double>
    {
    }
}
