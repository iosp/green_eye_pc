using System.Configuration;
using System.Text.RegularExpressions;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;
using System;

namespace GreenEyeAPI.Core.API.APICommands
{
    public class APILogicMissions : IAPICommand
    {
        private string[] mPaths = { "/api/logic/missions" };

        public override string[] URLPaths()
        {
            return mPaths;
        }

        protected override string ExecuteDeleteCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Delete command is not supported!", string.Empty, null);
        }

        protected override string ExecuteGetCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Get command is not supported!", string.Empty, null);
        }

        protected override string ExecutePostCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            string MissionJson = GetStringParam(pParameters, "CommandObjects");
            if (MissionJson == string.Empty)
                return CreateJSONResponse(ERR_MISSING_PARAMETERS, "Command body is missing", null);

            try
            {
                Mission NewMission = Mission.Deserialize(MissionJson);

                MissionPlan MP = LogicServerBLInterface.Instance().StartMission(NewMission);

                return CreateJSONResponse(0, "OK", MP);
            }
            catch (Exception ex)
            {
                return CreateJSONResponse(ERR_GENERAL_ERROR, ex.Message, null);
            }
        }

        protected override string ExecutePutCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Put command is not supported!", string.Empty, null);
        }

        protected override string ExecuteDebugCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Debug command is not supported!", string.Empty, null);
        }
    }
}
