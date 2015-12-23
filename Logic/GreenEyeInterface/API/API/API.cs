using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using GreenEyeAPI.Core.API.APICommands;
using GreenEyeAPI.Core.IFS;
using System.IO;

namespace GreenEyeAPI.Core.API
{
    public class API
    {
        public enum EnumAPIFormat
        {
            JSON = 1,
            XML = 2
        }

        public enum EnumAPICommands
        {
            GET = 1,
            POST = 2,
            PUT = 3,
            DELETE = 4,
            DEBUG = 5
        }

        private static API mInstance = null;
        private Hashtable mAPICommands = new Hashtable();
        private static Hashtable mDebugInfo = new Hashtable();

        protected API()
        {
            AddCommand(new APICommands.APILogicMissionsStop());
            AddCommand(new APICommands.APILogicMissionsWorld());
            AddCommand(new APICommands.APILogicAreas());
            AddCommand(new APICommands.APILogicMissions());
            AddCommand(new APICommands.APILogicMissionsAbort());
            AddCommand(new APICommands.APIWorldMissions());
            AddCommand(new APICommands.APIWorldMissionsAbort());
            AddCommand(new APICommands.APIWorldMissionsPlatformCommand());
            AddCommand(new APICommands.APIWorldMissionsUpdate());
        }

        private void AddCommand(IAPICommand pCommand)
        {
            for (int i = 0; i < pCommand.URLPaths().Length; i++)
                if (mAPICommands.ContainsKey(pCommand.URLPaths()[i]) == false)
                    mAPICommands.Add(pCommand.URLPaths()[i], pCommand);
        }
        
        public static API Instance()
        {
            if (mInstance == null)
                mInstance = new API();

            return mInstance;
        }

        private IAPICommand GetCommandObject(string pURL, ref NameValueCollection pParams)
        {
            IAPICommand RetVal = (IAPICommand)mAPICommands[pURL];
            
            if (RetVal == null)
            {
                string[] RequestURLParts = pURL.Split('/');
                foreach (IAPICommand Cmd in mAPICommands.Values)
                {
                    string[] CmdPaths = Cmd.URLPaths();
                    for (int j = 0; j < CmdPaths.Length; j++)
                    {
                        string[] CommandURLParts = CmdPaths[j].Split('/');
                        bool Found = true;
                        pParams.Clear();

                        if (RequestURLParts.Length == CommandURLParts.Length)
                        {
                            for (int i = 0; i < RequestURLParts.Length; i++)
                            {
                                if (RequestURLParts[i] != CommandURLParts[i])
                                    if (CommandURLParts[i].Trim()[0] == '{')
                                    {
                                        pParams.Add(CommandURLParts[i].Replace("{", string.Empty).Replace("}", string.Empty), RequestURLParts[i]);
                                    }
                                    else
                                        Found = false;
                            }
                        }
                        else
                            Found = false;

                        if (Found)
                        {
                            return Cmd;
                            break;
                        }
                    }
                }
            }

            return RetVal;
        }

        public bool ProcessURLRequest(HttpListenerRequest pRequest, HttpListenerResponse pResponse, string pRawURL)
        {
            string URL = pRawURL;
            IAPICommand CommandObj;
            string FormatParam = string.Empty;
            string ElementIDParam = string.Empty;
            string CommandObjectsParam = string.Empty;
            EnumAPIFormat Format = EnumAPIFormat.JSON;
            NameValueCollection Params = new NameValueCollection();

            if (URL.IndexOf('?') != -1)
                URL = URL.Substring(0, URL.IndexOf('?'));

            CommandObj = GetCommandObject(URL, ref Params);
            if (CommandObj != null && pRequest != null)
            {
                // Collect the parameters
                FormatParam = CUtils.GetStringParam(pRequest, "Fmt", string.Empty).ToLower();

                Params.Add(pRequest.QueryString);
                if (pRequest.HttpMethod.ToLower() == "post" || pRequest.HttpMethod.ToLower() == "put")
                {
                    string jsonString = String.Empty;

                    //pRequest.InputStream.Position = 0;
                    using (var inputStream = new StreamReader(pRequest.InputStream))
                    {
                        jsonString = inputStream.ReadToEnd();
                        if (jsonString != string.Empty)
                            Params.Add("CommandObjects", jsonString);
                    }
                }

                // Process params
                if (FormatParam == "xml") Format = EnumAPIFormat.XML; // json is the default, therefore it is not checked

                string Res = CommandObj.ExecuteCommand(URL, pRequest.HttpMethod, Format, Params);

                if (Format == EnumAPIFormat.JSON)
                {
                    pResponse.ContentType = "application/json";
                    pResponse.AddHeader("Access-Control-Allow-Origin", "*");
                    if (Res != null)
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(Res);
                        Console.WriteLine(Res);
                        pResponse.ContentLength64 = buffer.Length;

                        if (Res.IndexOf("ErrorCode") != -1)
                            pResponse.StatusCode = 500;

                        pResponse.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    
                    pResponse.OutputStream.Close();
                    return true;
                }
                else if (Format == EnumAPIFormat.XML)
                {
                    pResponse.ContentType = "text/xml";
                    pResponse.AddHeader("Access-Control-Allow-Origin", "*");
                    Res = "<xml><Error Code=\"-20\" Message=\"XML is not currently supported!\"></Error></xml>";
                    byte[] buffer = Encoding.UTF8.GetBytes(Res);
                    pResponse.ContentLength64 = buffer.Length;
                    pResponse.StatusCode = 200;
                    pResponse.OutputStream.Write(buffer, 0, buffer.Length);
                    pResponse.OutputStream.Close();
                    return true;
                }
            }

            return false;
        }

        public bool ProcessURLRequest(HttpRequest pRequest, HttpResponse pResponse, string pRawURL)
        {
            string URL = pRawURL;
            IAPICommand CommandObj;
            string FormatParam = string.Empty;
            string CommandParam = string.Empty;
            string ElementIDParam = string.Empty;
            string CommandObjectsParam = string.Empty;
            EnumAPIFormat Format = EnumAPIFormat.JSON;

            if (URL.IndexOf('?') != -1)
                URL = URL.Substring(0, URL.IndexOf('?'));

            CommandObj = (IAPICommand)mAPICommands[URL];
            if (CommandObj != null && pRequest != null)
            {
                // Collect the parameters
                FormatParam = CUtils.GetStringParam(pRequest, "Fmt", string.Empty).ToLower();
                CommandParam = CUtils.GetStringParam(pRequest, "Cmd", string.Empty).ToLower();
                ElementIDParam = CUtils.GetStringParam(pRequest, "ElementID", string.Empty).ToLower();
                NameValueCollection Params = new NameValueCollection();
                Params.Add(pRequest.QueryString);
                if (pRequest.RequestType.ToLower() == "post" || pRequest.RequestType.ToLower() == "put")
                {
                    CommandObjectsParam = CUtils.GetStringFormParam(pRequest.Form, "CommandObjects", string.Empty);
                    if (CommandObjectsParam != string.Empty)
                        Params.Add("CommandObjects", CommandObjectsParam);
                    else
                    {
                        string jsonString = String.Empty;

                        pRequest.InputStream.Position = 0;
                        using (var inputStream = new StreamReader(pRequest.InputStream))
                        {
                            jsonString = inputStream.ReadToEnd();
                            if (jsonString != string.Empty)
                                Params.Add("CommandObjects", jsonString);
                        }
                    }
                }

                // Process params
                if (FormatParam == "xml") Format = EnumAPIFormat.XML; // json is the default, therefore it is not checked

                string Res = CommandObj.ExecuteCommand(URL, pRequest.RequestType, Format, Params);
                if (Format == EnumAPIFormat.JSON)
                {
                    pResponse.ContentType = "application/json";
                    pResponse.AddHeader("Access-Control-Allow-Origin", "*");
                    byte[] buffer = Encoding.UTF8.GetBytes(Res);
                    Console.WriteLine(Res);
                    if (Res.IndexOf("ErrorCode") != -1)
                        pResponse.StatusCode = 500;
                    else
                        pResponse.StatusCode = 200;
                    pResponse.OutputStream.Write(buffer, 0, buffer.Length);
                    pResponse.OutputStream.Close();
                    return true;
                }
                else if (Format == EnumAPIFormat.XML)
                {
                    pResponse.ContentType = "text/xml";
                    pResponse.Write("<xml><Error Code=\"-20\" Message=\"XML is not currently supported!\"></Error></xml>");
                    pResponse.End();
                    return true;
                }
            }

            return false;
        }

        public string ExecuteCommand(string pURLPath, string pCmd, EnumAPIFormat pFormat, NameValueCollection pParameters)
        {
            IAPICommand CommandObj = (IAPICommand)mAPICommands[pURLPath];
            if (CommandObj != null)
                return CommandObj.ExecuteCommand(pURLPath, pCmd, pFormat, pParameters);
            else
                return string.Empty;
        }
    }
}
