using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities
{    
    /// <summary>
    /// Mission
    /// </summary>
    public class CMission
    {
        public string Id;
        public int Type;
        public Area MissionArea;
        public List<CRecoverySite> RecoverySites;
        public List<CLauncher> Launchers;
        public MissionConfig Config;

        

        public CMission()
        {
            MissionArea = new Area();
            RecoverySites = new List<CRecoverySite>();
            Launchers = new List<CLauncher>();
            Config = new MissionConfig();
           

        }

    };
}
