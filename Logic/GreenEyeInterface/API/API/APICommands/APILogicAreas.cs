using System.Configuration;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.BLInterface;
using System;

namespace GreenEyeAPI.Core.API.APICommands
{
    public class APILogicAreas : IAPICommand
    {
        private string[] mPaths = { "/api/logic/areas" };

        public override string[] URLPaths()
        {
            return mPaths;
        }

        protected override string ExecuteDeleteCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Delete command is not supported!", string.Empty, null);
        }

        protected override string ExecutePostCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            string AreasRequestParametersJson = GetStringParam(pParameters, "CommandObjects");
            if (AreasRequestParametersJson == string.Empty)
                return CreateJSONResponse(ERR_MISSING_PARAMETERS, "Command body is missing", null);

            try
            {
                AreasRequestParameters RequestParameters = AreasRequestParameters.Deserialize(AreasRequestParametersJson);

                Cell[] Cells = LogicServerBLInterface.Instance().GetAreas(RequestParameters.polygon, RequestParameters.missionConfig, RequestParameters.platforms);

                return CreateJSONResponse(0, "OK", Cells);
            }
            catch (Exception ex)
            {
                return CreateJSONResponse(ERR_GENERAL_ERROR, ex.Message, null);
            }
        }

        protected override string ExecuteGetCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Get command is not supported!", string.Empty, null);
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
