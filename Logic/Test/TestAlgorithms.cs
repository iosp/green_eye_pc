using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities;

namespace Logic.Test
{
    public class TestAlgorithms
    {
        public TestAlgorithms()
        {
        }

        public void TestReturnHomeAlgo()
        {
            Algorithm.ReturnHomeAlgo alg = new Algorithm.ReturnHomeAlgo();
            CMission mission = new CMission();
            CRecoverySite HomeSite = new CRecoverySite();
            HomeSite.ASL = 0;
            HomeSite.Position.East = 0;
            HomeSite.Position.North = 0;
            mission.RecoverySites.Add(HomeSite);
            List<IPlatform> platfotms = new List<IPlatform>();
            for (int i = 1; i <= 10; i++)
            {
                IPlatform platform = new IPlatform(i);
                platform.State.Position.East = 100 - i * 5;
                platform.State.Position.North = 100 - i * 5;
                platform.State.ASL = 10;


                platfotms.Add(platform);
            }

            alg.CreateWorkplan(platfotms, mission);
        }
    }
}
