using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GreenEyeAPI.Core.DataStructures;
using GreenEyeAPI.Core.IFS;
using System.Web.Script.Serialization;

namespace GreenEyeAPI
{
    public class UdpServer
    {
        private static int UDP_SERVER_PORT = ApplicationDataPaths.GetServerPort();
        private static int RECIPIENT_UPDATE_INTERVAL = ApplicationDataPaths.GetEntityUpdateInterval();
        private static string[] RECIPIENTS = ApplicationDataPaths.EntityUpdateRecipients();

        private static UdpServer mInstance = null;

        private static bool mIsConsoleMode = false;
        private static bool mIsRunning = false;
        private static bool mIsLogEnabled = false;

        private Thread mThread = null;
        private Hashtable mEntities = new Hashtable();
      
        protected UdpServer()
        {
        }

        public static UdpServer Instance()
        {
            if (mInstance == null)
                mInstance = new UdpServer();

            return mInstance;
        }

        public static void WriteLine(string pText)
        {
            try
            {
                if (mIsConsoleMode == true)
                    Console.WriteLine(pText);
            }
            catch
            {
            }
        }

        public void Start(bool pIsConsoleMode, bool pIsLogEnabled)
        {
            if (mThread != null || mIsRunning == true)
                return;

            mIsRunning = true;
            mIsConsoleMode = pIsConsoleMode;
            mIsLogEnabled = pIsLogEnabled;
            mThread = new Thread(new ThreadStart(this.UdpListener));

            // Start the thread
            mThread.Start();

            // Spin for a while waiting for the started thread to become
            // alive:
            while (!mThread.IsAlive);

            // Put the Main thread to sleep for 1 millisecond to allow oThread
            // to do some work:
            Thread.Sleep(1);

            if (mIsConsoleMode)
            {
                WriteLine("Listening.  Press Enter to stop.");
                Console.ReadLine();
                Stop();
            }
        }

        public void Stop()
        {
            if (mIsRunning == false)
                return;
            mIsRunning = false;
      
            // Request that oThread be stopped
            mThread.Abort();
      
            // Wait until oThread finishes. Join also has overloads
            // that take a millisecond interval or a TimeSpan object.
            mThread.Join();
            mThread = null;
        }

        protected Platform ParseUdpMessage(byte[] pMessage)
        {
            Platform P = new Platform();

            return P;
        }

        protected void PublishPlatformsUpdate()
        {
            WebClient WC = new WebClient();
            WC.Headers["ContentType"] = "application/json";

            // Prepare the data
            World W = new World();
            foreach (Platform P in mEntities.Values)
            {
                W.Add (P);
            }

            // Prepare the Json
            JavaScriptSerializer oSer = new JavaScriptSerializer();
            string JsonW = oSer.Serialize(W);

            for (int i = 0; i < RECIPIENTS.Length; i++)
            {
                WC.UploadString(RECIPIENTS[i], "PUT", JsonW);
            }
        }

        protected void UdpListener()
        {
            UdpClient Listener = new UdpClient(UDP_SERVER_PORT);
            IPEndPoint GroupEP = new IPEndPoint(IPAddress.Any, UDP_SERVER_PORT);
            byte[] ReceivedData;
            DateTime LastPublishing = DateTime.MinValue;

            while (mIsRunning == true && mThread != null)
            {
                while (Listener.Available > 0)
                {
                    ReceivedData = Listener.Receive(ref GroupEP);
                    Platform P = ParseUdpMessage(ReceivedData);
                    if (mEntities.ContainsKey(P.id.ToString()) == false)
                        mEntities.Add(P.id.ToString(), P);
                    else
                        mEntities[P.id.ToString()] = P;
                }

                TimeSpan TS = DateTime.Now - LastPublishing;
                if (TS.TotalMilliseconds > RECIPIENT_UPDATE_INTERVAL)
                {
                    LastPublishing = DateTime.Now;
                    PublishPlatformsUpdate();
                }

                Thread.Sleep (1);
            }
        }
    }
}
