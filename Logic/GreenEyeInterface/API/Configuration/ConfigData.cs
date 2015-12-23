using System.Xml.Serialization;

namespace GreenEyeAPI.Core.Configuration
{
    [XmlType(TypeName = "Config")]
    public class ConfigData
    {
        public ConfigData()
        {
            CacheOnlyMusic = true;
            MaxQuotaInMB = 1024 * 10; // 10 GB
            MinFreeSpaceInMB = 1024 * 1; // 1 GB
            IsEnabled = true;
            MaxFileSizeInMB = 100;
        }

        public bool CacheOnlyMusic { get; set; }        // Whether to cache only music clips
        public long MaxQuotaInMB { get; set; }               // Maximum space to use for caching
        public long MinFreeSpaceInMB { get; set; }           // Minimum disk space to be left
        public bool IsEnabled { get; set; }             // Is cache option enabled
        public long MaxFileSizeInMB { get; set; }             // Is cache option enabled
    }
}
