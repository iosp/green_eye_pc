using System;
using System.IO;
using System.Xml.Serialization;
using GreenEyeAPI.Core.IFS;

namespace GreenEyeAPI.Core.Configuration
{
    public class Config
    {
        private string CONFIG_FILE_NAME = /*ApplicationDataPaths.GetRootDataPath() + "\\" + */"config.xml";
        private static Config mInstance = null;
        private DateTime mConfigFileDateTime = DateTime.MinValue;
        private ConfigData mConfigData = new ConfigData();

        public T DeserializeObject<T>(string pSerializedData)
        {
            return (T)DeserializeObject(pSerializedData, typeof(T));
        }

        public object DeserializeObject(string pSerializedData, Type pDeserializeTo)
        {
            var serializer = new XmlSerializer(pDeserializeTo);
            object result;

            using (TextReader reader = new StringReader(pSerializedData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        protected Config()
        {
            LoadConfigFile();
        }

        public static Config Instance()
        {
            if (mInstance == null)
                mInstance = new Config();

            return mInstance;
        }

        public ConfigData Configuration
        {
            get 
            {
                try
                {
                    DateTime TmpDT = new FileInfo(CONFIG_FILE_NAME).LastWriteTime;
                    if (TmpDT != mConfigFileDateTime)
                        LoadConfigFile();
                }
                catch
                {
                }

                return mConfigData;
            }
            
            set
            {
                mConfigData = value;
                SaveConfigFile();
            }
        }

        protected void SaveConfigFile()
        {
            try
            {
                XmlSerializer oSer = new XmlSerializer(mConfigData.GetType());
                using (StringWriter writer = new StringWriter())
                {
                    oSer.Serialize(writer, mConfigData);
                    System.IO.File.WriteAllText(CONFIG_FILE_NAME, writer.ToString());
                }
            }
            catch
            {
            }
        }

        protected void LoadConfigFile()
        {
            try
            {
                string XML = File.ReadAllText(CONFIG_FILE_NAME);
                mConfigData = (ConfigData)DeserializeObject (XML, typeof (ConfigData));
                mConfigFileDateTime = new FileInfo(CONFIG_FILE_NAME).LastWriteTime;
            }
            catch
            {
                
            }
        }
    }
}
