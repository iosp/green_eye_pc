using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;

namespace GreenEyeAPI.BL
{
    public class SampleLogicBLImplementation
    {

        public SampleLogicBLImplementation()
        {
            LogicServerBLInterface.Instance().OnStartMission += this.StartMissionHandler;
            LogicServerBLInterface.Instance().OnStopMission += this.StopMissionHandler;
            LogicServerBLInterface.Instance().OnAbortMission += this.AbortMissionHandler;
            LogicServerBLInterface.Instance().OnGetAreas += this.GetAreasHandler;
            LogicServerBLInterface.Instance().OnUpdateMissionWorld += this.UpdateMissionWorldHandler;
        }

        public Cell[] GetAreasHandler(PoligonGeoJson polygon, MissionConfig missionConfig, Platform[] platforms)
        {
            Cell[] RetVal = new Cell[2];
            Position Pos = null;

            for (int i = 0; i < 2; i++)
            {
                RetVal[i] = new Cell();
                Pos = new Position();
                Pos.Add(32.123123123 - i);
                Pos.Add(25.123123123 - i);
                RetVal[i].Add(Pos);
                Pos = new Position();
                Pos.Add(32.23234234 + i);
                Pos.Add(24.123123123 + i);
                RetVal[i].Add(Pos);
            }

            return RetVal;
        }

        public MissionPlan StartMissionHandler(Mission mission)
        {
            Random rnd = new Random();
            MissionPlan MP = new MissionPlan() { missionId = (rnd.Next(1000)).ToString() };
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
            WP.alt = rnd.Next (30000);
            WP.id = rnd.Next (10000);
            WP.position = new Position();
            WP.position.Add(32.12312312);
            WP.position.Add(32.12312312);
            MP.platforms[0].route.Add(WP);

            return MP;
        }

        public MissionPlan StopMissionHandler(string missionId)
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

        public void AbortMissionHandler(string missionId)
        {
        }

        public MissionPlan UpdateMissionWorldHandler(string missionId, World world)
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
