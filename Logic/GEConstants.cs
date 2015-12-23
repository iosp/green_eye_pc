using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class GEConstants
    {
        private const bool IsLabMode = true;
      
        public const int RX_PORT = 4000;
        public const int TX_PORT = 4001;


        public const int MAX_ROUTE = 15;
        public const int GE_START_BYTES	=		0x5050;
        public const int GE_FLEET_ADDRESS = -1;
        public const int GE_LOGIC_ADDRESS = 0;

        public const int KEEP_LAST_VALUE = -999;

        public static string GetLocalIP()
        {
            if (IsLabMode)
                return "172.23.1.138";
            else
                return "169.254.49.135";            
        }

        public static string GetHostIp()
        {
            if (IsLabMode)
                return "172.23.1.138";
            else
                return "169.254.49.135";            

        }
     
    }
}
