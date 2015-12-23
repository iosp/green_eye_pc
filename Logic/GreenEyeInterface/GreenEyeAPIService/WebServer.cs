using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using GreenEyeAPI.Core.IFS;

namespace GreenEyeAPI
{
    public class WebServer
    {
        // To enable this so that it can be run in a non-administrator account:
        // Open an Administrator command prompt.
        //  netsh http add urlacl urul=http://+:8008/ user=Everyone listen=yes
        private static string Prefix = ApplicationDataPaths.GetServerAddress();
        //private static string WEBSITE_ROOT_DIR = ApplicationDataPaths.GetWWWRootPath();
        //private static string APP_ROOT_DIR = ApplicationDataPaths.GetRootDataPath();

        static HttpListener Listener = null;
        static int RequestNumber = 0;
        static readonly DateTime StartupDate = DateTime.UtcNow;
        static bool mIsConsoleMode = false;
        static bool mIsRunning = false;
        static bool mIsLogEnabled = false;
        static Hashtable mServedFiles = new Hashtable();

        public static void WriteLine(string pText)
        {
            try
            {
                if (mIsConsoleMode == false)
                {
                    //if (mIsLogEnabled == true)
                        //System.IO.File.AppendAllText(APP_ROOT_DIR + "\\" + "service.log", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + pText + "\n");
                }
                else
                    Console.WriteLine(pText);
            }
            catch
            {
            }
        }

        static public void Start(bool pIsConsoleMode, bool pIsLogEnabled)
        {
            if (!HttpListener.IsSupported || mIsRunning == true)
                return;

            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            Directory.SetCurrentDirectory(path);

            mIsRunning = true;
            mIsConsoleMode = pIsConsoleMode;
            mIsLogEnabled = pIsLogEnabled;
            Listener = new HttpListener();

            Listener.Prefixes.Add(Prefix);
            Listener.Start();
            // Begin waiting for requests.
            Listener.BeginGetContext(GetContextCallback, null);
            if (mIsConsoleMode)
            {
                WriteLine("Listening.  Press Enter to stop.");
                Console.ReadLine();
                Listener.Stop();
            }
        }

        static public void Stop()
        {
            if (mIsRunning == false)
                return;
            mIsRunning = false;

            if (Listener != null)
                Listener.Stop();
        }

        static protected void GetContextCallback(IAsyncResult ar)
        {
            try
            {
                int req = ++RequestNumber;
                // Get the context
                var context = Listener.EndGetContext(ar);
                // listen for the next request
                Listener.BeginGetContext(GetContextCallback, null);
                // get the request
                var NowTime = DateTime.UtcNow;

                WriteLine(NowTime.ToString("R") + ":" + context.Request.RawUrl);

                // Create Response
                if (GreenEyeAPI.Core.API.API.Instance().ProcessURLRequest(context.Request, context.Response, context.Request.RawUrl) == false)
                {
                    context.Response.ContentType = "text/html";
                    string responseString = string.Format("<html><head></head><body>File not found (404)</body></html>");
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.StatusDescription = "File Not Found (404)";
                    context.Response.OutputStream.Close();
                    WriteLine("File Not Found!");

                    try
                    {
                        context.Response.Close();
                    }
                    catch { }
                }
                WriteLine("Request Ended " + context.Request.RawUrl);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
