using System;
using System.Configuration;
using System.Web.Script.Serialization;

namespace GreenEyeAPI.Core.API.APICommands
{
    public abstract class IAPICommand
    {
        public const int ERR_OK = 0;
        public const int ERR_GENERAL_ERROR = -1;
        public const int ERR_FORMAT_NOT_SUPPORTED = -20;
        public const int ERR_COMMAND_NOT_SUPPORTED = -21;
        public const int ERR_MISSING_PARAMETERS = -22;
        public const int ERR_ELEMENT_NOT_FOUND = -23;
        public const int ERR_INVALID_PARAMETER = -24;
        public const int ERR_ACCESS_DENIED = -25;

        public const string CMD_GET = "GET";
        public const string CMD_PUT = "PUT";
        public const string CMD_POST = "POST";
        public const string CMD_DELETE = "DELETE";
        public const string CMD_DEBUG = "DEBUG";

        public abstract string[] URLPaths();
        protected abstract string ExecuteDeleteCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters);
        protected abstract string ExecutePostCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters);
        protected abstract string ExecutePutCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters);
        protected abstract string ExecuteDebugCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters);
        protected abstract string ExecuteGetCommand(string pURLPath, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters);

        protected string GetStringParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName, string pDefaultValue)
        {
            if (pParameters[pParamName] == null)
                return pDefaultValue;
            else
                return System.Web.HttpUtility.UrlDecode(pParameters[pParamName].ToString(), new System.Text.UTF8Encoding()); 
        }

        protected string GetStringParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName)
        {
            return GetStringParam(pParameters, pParamName, string.Empty);
        }

        protected int GetIntParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName, int pDefaultValue)
        {
            if (pParameters[pParamName] == null)
                return pDefaultValue;
            else
            {
                try
                {
                    return int.Parse (pParameters[pParamName].ToString());
                }
                catch
                {
                    return pDefaultValue;
                }
            }
        }

        protected int GetIntParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName)
        {
            return GetIntParam(pParameters, pParamName, -1);
        }


        protected bool GetBoolParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName, bool pDefaultValue)
        {
            if (pParameters[pParamName] == null)
                return pDefaultValue;
            else
            {
                try
                {
                    return bool.Parse(pParameters[pParamName].ToString());
                }
                catch
                {
                    return pDefaultValue;
                }
            }
        }

        protected bool GetBoolParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName)
        {
            return GetBoolParam(pParameters, pParamName, false);
        }

        protected DateTime GetDateTimeParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName, DateTime pDefaultValue)
        {
            if (pParameters[pParamName] == null)
                return pDefaultValue;
            else
            {
                try
                {
                    return DateTime.Parse(pParameters[pParamName].ToString());
                }
                catch
                {
                    return pDefaultValue;
                }
            }
        }

        protected DateTime GetDateTimeParam(System.Collections.Specialized.NameValueCollection pParameters, string pParamName)
        {
            return GetDateTimeParam(pParameters, pParamName, DateTime.Today);
        }

        protected object[] CreateObjectArray(params object[] pObjects)
        {
            return pObjects;
        }


        protected string CreateResponse(API.EnumAPIFormat pFormat, string pJsonpCallback, int pErrorCode, string pErrorDesc, object[] pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(pJsonpCallback, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateResponse(API.EnumAPIFormat pFormat, string pJsonpCallback, int pErrorCode, string pErrorDesc, object pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(pJsonpCallback, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateResponse(API.EnumAPIFormat pFormat, int pErrorCode, string pErrorDesc, object[] pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(string.Empty, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateResponse(API.EnumAPIFormat pFormat, int pErrorCode, string pErrorDesc, object pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(string.Empty, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateResponse(API.EnumAPIFormat pFormat, string pJsonpCallback, int pErrorCode, string pErrorDesc, string pElementID, object[] pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(pJsonpCallback, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateResponse(API.EnumAPIFormat pFormat, string pJsonpCallback, int pErrorCode, string pErrorDesc, string pElementID, object pRetData)
        {
            if (pFormat == API.EnumAPIFormat.JSON)
                return CreateJSONResponse(pJsonpCallback, pErrorCode, pErrorDesc, pRetData);
            else
                return CreateXMLResponse(pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateJSONResponse(int pErrorCode, string pErrorDesc, object[] pRetData)
        {
            return CreateJSONResponse(string.Empty, pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateJSONResponse(int pErrorCode, string pErrorDesc, object pRetData)
        {
            return CreateJSONResponse(string.Empty, pErrorCode, pErrorDesc, pRetData);
        }

        protected string CreateJSONResponse(string pJsonpCallback, int pErrorCode, string pErrorDesc, object[] pRetData)
        {
            JavaScriptSerializer oSer = new JavaScriptSerializer();
            string JsonResp = null;

            if (pErrorCode == 0)
            {
                if (pRetData != null)
                    JsonResp = oSer.Serialize(pRetData);
            }
            else
                JsonResp = oSer.Serialize(new { ErrorCode = pErrorCode, ErrorDesc = pErrorDesc });

            if (pJsonpCallback != string.Empty)
                JsonResp = pJsonpCallback + " (" + JsonResp + ")";

            return JsonResp;
        }

        protected string CreateJSONResponse(string pJsonpCallback, int pErrorCode, string pErrorDesc, object pRetData)
        {
            JavaScriptSerializer oSer = new JavaScriptSerializer();
            string JsonResp = null;

            if (pErrorCode == 0)
            {
                if (pRetData != null)
                    JsonResp = oSer.Serialize(pRetData);
            }
            else
                JsonResp = oSer.Serialize(new { ErrorCode = pErrorCode, ErrorDesc = pErrorDesc });

            if (pJsonpCallback != string.Empty)
                JsonResp = pJsonpCallback + " (" + JsonResp + ")";

            return JsonResp;
        }

        protected string CreateXMLResponse(int pErrorCode, string pErrorDesc, object[] pRetData)
        {
            return "<xml><Error Code=\"-20\" Message=\"XML is not currently supported!\"></Error></xml>";
        }

        protected string CreateXMLResponse(int pErrorCode, string pErrorDesc, object pRetData)
        {
            return "<xml><Error Code=\"-20\" Message=\"XML is not currently supported!\"></Error></xml>";
        }

        public string ExecuteCommand(string pURLPath, string pCmd, API.EnumAPIFormat pFormat, System.Collections.Specialized.NameValueCollection pParameters)
        {
            if (pCmd.ToUpper() == CMD_GET)
                return ExecuteGetCommand(pURLPath, pFormat, pParameters);

            if (pCmd.ToUpper() == CMD_DELETE)
                return ExecuteDeleteCommand(pURLPath, pFormat, pParameters);

            if (pCmd.ToUpper() == CMD_POST)
                return ExecutePostCommand(pURLPath, pFormat, pParameters);

            if (pCmd.ToUpper() == CMD_PUT)
                return ExecutePutCommand(pURLPath, pFormat, pParameters);

            if (pCmd.ToUpper() == CMD_DEBUG)
                return ExecuteDebugCommand(pURLPath, pFormat, pParameters);

            return CreateResponse(pFormat, GetStringParam(pParameters, "CallbackName"), ERR_COMMAND_NOT_SUPPORTED, "Command is not supported", null); 
        }
    }
}
