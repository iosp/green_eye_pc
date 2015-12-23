using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core.DataStructures;

namespace GreenEyeAPI.Core.BLInterface
{
    public class LogicServerBLInterface
    {
        private static LogicServerBLInterface mInstance = null;
        
        public delegate Cell[] GetAreasHandler(PoligonGeoJson polygon, MissionConfig missionConfig, Platform[] platforms);
        public delegate MissionPlan StartMissionHandler(Mission mission);
        public delegate MissionPlan StopMissionHandler(string missionId);
        public delegate void AbortMissionHandler(string missionId);
        public delegate MissionPlan UpdateMissionWorldHandler(string missionId, World world);

        public GetAreasHandler OnGetAreas;
        public StartMissionHandler OnStartMission;
        public StopMissionHandler OnStopMission;
        public AbortMissionHandler OnAbortMission;
        public UpdateMissionWorldHandler OnUpdateMissionWorld;

        protected LogicServerBLInterface()
        {
        }

        public static LogicServerBLInterface Instance()
        {
            if (mInstance == null)
                mInstance = new LogicServerBLInterface();

            return mInstance;
        }

        public Cell[] GetAreas(PoligonGeoJson polygon, MissionConfig missionConfig, Platform[] platforms)
        {
            if (OnGetAreas != null)
                return OnGetAreas(polygon, missionConfig, platforms);
            else
                throw new Exception("No GetAreas command handler");
        }

        public MissionPlan StartMission(Mission mission)
        {
            if (OnStartMission != null)
                return OnStartMission(mission);
            else
                throw new Exception("No StartMission command handler");
        }

        public MissionPlan StopMission(string missionId)
        {
            if (OnStopMission != null)
                return OnStopMission(missionId);
            else
                throw new Exception("No StopMission command handler");
        }

        public void AbortMission(string missionId)
        {
            if (OnAbortMission != null)
                OnAbortMission(missionId);
            else
                throw new Exception("No AbortMission command handler");
        }

        public MissionPlan UpdateMissionWorld(string missionId, World world)
        {
            if (OnUpdateMissionWorld != null)
                return OnUpdateMissionWorld(missionId, world);
            else
                throw new Exception("No UpdateMissionWorld command handler");
        }
    }
}
