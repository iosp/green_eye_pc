using System;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

/// <summary>
/// Summary description for Utils
/// </summary>
namespace GreenEyeAPI.Core.IFS
{
    public class CUtils
    {
        public CUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string Bool2Str(bool pVal)
        {
            return pVal == true ? "1" : "0";
        }

        public static string Str2SQLStr(string pStr)
        {
            return "N'" + pStr.Replace("'", "''") + "'";
        }

        public static string GetBaseURL(HttpRequest pReq)
        {
            string Srvr = pReq.ServerVariables["SERVER_NAME"];
            string Url = pReq.ServerVariables["URL"];
            string QueryString = pReq.ServerVariables["QUERY_STRING"];
            string port = pReq.ServerVariables["SERVER_PORT"];
            if (port != "80")
                port = ":" + port;
            else
                port = string.Empty;
            return "http://" + Srvr + port;// +"/" + GetCurrentLanguage();
        }

        public static string GetCurrentLanguage()
        {
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }

        public static int Bool2Int (bool pBool)
        {
            if (pBool == true)
                return 1;
            else
                return 0;
        }

        public static string ClearHTMLTags(string strHTML, int intWorkFlow)
        {
            //Variables used in the function
            Regex regEx;
            string strTagLess;
		
            strTagLess = strHTML;

            // regEx.Global = True //Global applicability
    
            //Phase I
            //	"bye bye html tags"
            if (intWorkFlow != 1)
            {
                //---------------------------------------
                regEx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
                //this pattern mathces any html tag
                strTagLess = regEx.Replace(strTagLess, "");
                //all html tags are stripped
                //---------------------------------------
            }
		
            //Phase II
            //	"bye bye rouge leftovers"
            //	"or, I want to render the source"
            //	"as html."

            //---------------------------------------
            //We *might* still have rouge < and > 
            //let's be positive that those that remain
            //are changed into html characters
            //---------------------------------------	

            if (intWorkFlow > 0 && intWorkFlow < 3)
            {
                regEx = new Regex("[<]", RegexOptions.IgnoreCase);
                strTagLess = regEx.Replace(strTagLess, "<");

                regEx = new Regex("[>]", RegexOptions.IgnoreCase);
                strTagLess = regEx.Replace(strTagLess, ">");
            }
		
            //Clean up
            //---------------------------------------
            regEx = null;
            //Destroys the regExp object
            //---------------------------------------	
		
            //---------------------------------------
            return strTagLess;
            //The results are passed back
            //---------------------------------------
        }

        static public string ClearURL(string pURL)
        {
            if (pURL == string.Empty)
                return string.Empty;

            string RetVal = pURL.Replace(".", " ").Replace(":", " ").Replace("?", " ").Replace("/", "-").Replace("#", "").Replace("\"", " ").Replace("&", " ").Replace(";", " ").Replace("*", "").Replace("'", "").Trim();
            while (RetVal.IndexOf("  ") != -1)
                RetVal = RetVal.Replace("  ", " ");

            return RetVal;
        }

        static public bool IsNumber(string pStr)
        {
            try
            {
                int x = int.Parse(pStr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static public string PrepareStringForDB(string pStr)
        {
            return PrepareStringForDB(pStr, -1);
        }

        static public string PrepareStringForDB(string pStr, int pMaxLen)
        {
            string NewStr = pStr.Replace("'", "''");

            if (NewStr.Length > pMaxLen && pMaxLen > 0)
                NewStr = NewStr.Substring(0, pMaxLen);

            return NewStr;
        }

        static public string GetStringField(DataRow pRow, string pFieldName, string pDefaultValue)
        {
            if (pRow[pFieldName] != DBNull.Value) return (string)pRow[pFieldName]; else return pDefaultValue;
        }

        static public double GetDoubleField(DataRow pRow, string pFieldName, double pDefaultValue)
        {
            if (pRow[pFieldName] != DBNull.Value) return (double)pRow[pFieldName]; else return pDefaultValue;
        }

        static public int GetIntField(DataRow pRow, string pFieldName, int pDefaultValue)
        {
            if (pRow[pFieldName] != DBNull.Value) return (int)pRow[pFieldName]; else return pDefaultValue;
        }

        static public DateTime GetDateTimeField(DataRow pRow, string pFieldName, DateTime pDefaultValue)
        {
            if (pRow[pFieldName] != DBNull.Value) return (DateTime)pRow[pFieldName]; else return pDefaultValue;
        }

        static public bool GetBoolField(DataRow pRow, string pFieldName, bool pDefaultValue)
        {
            if (pRow[pFieldName] != DBNull.Value)
                return (bool)pRow[pFieldName];
            else
                return pDefaultValue;
        }


        static public int[] GetIntArrayParam(HttpRequest pRequest, string pParamName)
        {
            int[] RetVal;

            if (pParamName == null)
                return null;

            try
            {
                string[] Elements = pRequest[pParamName].Split(',');
                RetVal = new int[Elements.Length];
                for (int i = 0; i < Elements.Length; i++)
                    RetVal[i] = int.Parse(Elements[i]);

                return RetVal;
            }
            catch
            {
                return null;
            }
        }

        public static string GetStringParam(HttpRequest pRequest, string pParamName)
        {
            return GetStringParam(pRequest, pParamName, string.Empty);
        }

        public static string GetStringParam(HttpRequest pRequest, string pParamName, string pDefaultValue)
        {
            if (pParamName == null || pRequest == null)
                return pDefaultValue;

            if (pRequest[pParamName] != null)
                return pRequest[pParamName];
            else
                return pDefaultValue;
        }

        public static string GetStringParam(HttpListenerRequest pRequest, string pParamName)
        {
            return GetStringParam(pRequest, pParamName, string.Empty);
        }

        public static string GetStringParam(HttpListenerRequest pRequest, string pParamName, string pDefaultValue)
        {
            if (pParamName == null || pRequest == null)
                return pDefaultValue;

            if (pRequest.QueryString[pParamName] != null)
                return pRequest.QueryString[pParamName];
            else
                return pDefaultValue;
        }

        public static int GetIntParam(HttpRequest pRequest, string pParamName)
        {
            return GetIntParam(pRequest, pParamName, -1);
        }

        public static int GetIntParam(HttpRequest pRequest, string pParamName, int pDefaultValue)
        {
            try
            {
                if (pParamName == null || pRequest == null)
                    return pDefaultValue;

                if (pRequest[pParamName] != null)
                    return int.Parse(pRequest[pParamName]);
                else
                    return pDefaultValue;
            }
            catch
            {
                return pDefaultValue;
            }
        }

        public static double GetDoubleParam(HttpRequest pRequest, string pParamName)
        {
            return GetDoubleParam(pRequest, pParamName, -1);
        }

        public static double GetDoubleParam(HttpRequest pRequest, string pParamName, int pDefaultValue)
        {
            try
            {
                if (pParamName == null || pRequest == null)
                    return pDefaultValue;

                if (pRequest[pParamName] != null)
                    return double.Parse(pRequest[pParamName]);
                else
                    return pDefaultValue;
            }
            catch
            {
                return pDefaultValue;
            }
        }

        public static long GetLongParam(HttpRequest pRequest, string pParamName)
        {
            return GetLongParam(pRequest, pParamName, -1);
        }

        public static long GetLongParam(HttpRequest pRequest, string pParamName, long pDefaultValue)
        {
            try
            {
                if (pParamName == null || pRequest == null)
                    return pDefaultValue;

                if (pRequest[pParamName] != null)
                    return long.Parse(pRequest[pParamName]);
                else
                    return pDefaultValue;
            }
            catch
            {
                return pDefaultValue;
            }
        }

        public static bool GetBoolParam(HttpRequest pRequest, string pParamName)
        {
            return GetBoolParam(pRequest, pParamName, false);
        }

        public static bool GetBoolParam(HttpRequest pRequest, string pParamName, bool pDefaultValue)
        {
            try
            {
                if (pParamName == null || pRequest == null)
                    return pDefaultValue;

                if (pRequest[pParamName] != null)
                    return bool.Parse(pRequest[pParamName]);
                else
                    return pDefaultValue;
            }
            catch
            {
                return pDefaultValue;
            }
        }

        static public int GetIntFormParam(NameValueCollection pForm, string pParamName, int pDefaultValue)
        {
            if (pParamName == null)
                return pDefaultValue;

            if (pForm[pParamName] == null)
                return pDefaultValue;

            try
            {
                return int.Parse(pForm[pParamName]);
            }
            catch
            {
                return pDefaultValue;
            }
        }

        static public int[] GetIntArrayFormParam(NameValueCollection pForm, string pParamName)
        {
            int[] RetVal;

            if (pParamName == null)
                return null;

            try
            {
                string[] Elements = pForm[pParamName].Split(',');
                RetVal = new int[Elements.Length];
                for (int i = 0; i < Elements.Length; i++)
                    RetVal[i] = int.Parse(Elements[i]);

                return RetVal;
            }
            catch
            {
                return null;
            }
        }

        static public string GetStringFormParam(NameValueCollection pForm, string pParamName, string pDefaultValue)
        {
            if (pParamName == null)
                return pDefaultValue;

            if (pForm[pParamName] == null)
                return pDefaultValue;

            try
            {
                return pForm[pParamName];
            }
            catch
            {
                return pDefaultValue;
            }
        }

    }
}