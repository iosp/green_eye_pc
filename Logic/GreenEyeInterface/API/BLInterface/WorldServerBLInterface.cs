using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenEyeAPI.Core.DataStructures;

namespace GreenEyeAPI.Core.BLInterface
{
    public class WorldServerBLInterface
    {
        private static WorldServerBLInterface mInstance = null;
        
        public delegate MissionPlan StartMissionHandler(MissionPlan missionPlan);
        public delegate void UpdateMissionHandler(string missionId, MissionPlan missionPlan);
        public delegate void AbortMissionHandler(string missionId);
        public delegate MissionPlan SendCommandHandler(string missionId, int platformId, Command command);

        public StartMissionHandler OnStartMission;
        public UpdateMissionHandler OnUpdateMission;
        public AbortMissionHandler OnAbortMission;
        public SendCommandHandler OnSendCommand;

        protected WorldServerBLInterface()
        {
        }

        public static WorldServerBLInterface Instance()
        {
            if (mInstance == null)
                mInstance = new WorldServerBLInterface();

            return mInstance;
        }

        public MissionPlan StartMission(MissionPlan missionPlan)
        {
            if (OnStartMission != null)
                return OnStartMission(missionPlan);
            else
                throw new Exception("No StartMission command handler");
        }

        public void UpdateMission(string missionId, MissionPlan missionPlan)
        {
            if (OnUpdateMission != null)
                OnUpdateMission(missionId, missionPlan);
            else
                throw new Exception("No UpdateMission command handler");
        }

        public void AbortMission(string missionId)
        {
            if (OnAbortMission != null)
                OnAbortMission(missionId);
            else
                throw new Exception("No AbortMission command handler");
        }

        public MissionPlan SendCommand(string missionId, int platformId, Command command)
        {
            if (OnSendCommand != null)
                return OnSendCommand(missionId, platformId, command);
            else
                throw new Exception("No SendCommand command handler");
        }
    }
}
