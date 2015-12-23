using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace GreenEyeAPI.Core.IFS
{
    public class ApplicationDataPaths
    {
        public static string[] EntityUpdateRecipients()
        {
            string Recipients = ConfigurationManager.AppSettings["EntityUpdateRecipients"];

            return Recipients.Split(';');
        }

        public static int GetServerPort()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["UdpServerPort"]);
            }
            catch
            {
                return 8009;
            }
        }

        public static int GetEntityUpdateInterval()
        {
            try
            {
                return int.Parse(ConfigurationManager.AppSettings["EntityUpdateInterval"]);
            }
            catch
            {
                return 1000;
            }
        }

        public static string GetServerAddress()
        {
            try
            {
                return ConfigurationManager.AppSettings["ServerAddr"];
            }
            catch
            {
                return "http://+:8008/";
            }
        }

    }
}
