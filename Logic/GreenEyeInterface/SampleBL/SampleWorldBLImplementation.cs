using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;

namespace GreenEyeAPI.BL
{
    public class SampleWorldBLImplementation
    {
        public SampleWorldBLImplementation()
        {
            WorldServerBLInterface.Instance().OnAbortMission += this.AbortMissionHandler;
            WorldServerBLInterface.Instance().OnSendCommand += this.SendCommandHandler;
            WorldServerBLInterface.Instance().OnStartMission += this.StartMissionHandler;
            WorldServerBLInterface.Instance().OnUpdateMission += this.UpdateMissionHandler;
        }

        public MissionPlan StartMissionHandler(MissionPlan missionPlan)
        {
            return missionPlan;
        }

        public void UpdateMissionHandler(string missionId, MissionPlan missionPlan)
        {
        }

        public void AbortMissionHandler(string missionId)
        {
        }

        public MissionPlan SendCommandHandler(string missionId, int platformId, Command command)
        {
            Random rnd = new Random();
            MissionPlan MP = new MissionPlan() { missionId = missionId };
            Cell[] Cells = new Cell[2];
            Position Pos = null;

            for (int i = 0; i < 2; i++)
            {
                Cells[i] = new Cell();
                Pos = new Position();
                Pos.Add(32.123123123 - i);
                Pos.Add(25.123123123 - i);
                Cells[i].Add(Pos);
                Pos = new Position();
                Pos.Add(32.23234234 + i);
                Pos.Add(24.123123123 + i);
                Cells[i].Add(Pos);
            }

            MP.cells = Cells;

            MP.platforms = new Platform[1];
            MP.platforms[0] = new Platform() { id = rnd.Next(1000) };
            MP.platforms[0].route = new Route();
            Waypoint WP = new Waypoint();
            WP.actions = new GreenEyeAPI.Core.DataStructures.Action[1];
            WP.actions[0] = new Core.DataStructures.Action();
            WP.alt = rnd.Next(30000);
            WP.id = rnd.Next(10000);
            WP.position = new Position();
            WP.position.Add(32.12312312);
            WP.position.Add(32.12312312);
            MP.platforms[0].route.Add(WP);

            return MP;
        }
    }
}
