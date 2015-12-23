using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using GreenEyeAPI.BL;
using GreenEyeAPI.Core.BLInterface;

namespace GreenEyeAPI
{
    class Program : ServiceBase
    {
        private static bool mIsService;
        private static bool mIsLogEnabled;
        private static SampleLogicBLImplementation LogicBL = new SampleLogicBLImplementation();
        private static SampleWorldBLImplementation WorldBL = new SampleWorldBLImplementation();

        static void Main(string[] args)
        {
            mIsService = true;
            mIsLogEnabled = false;

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].ToLower() == "-log" && mIsLogEnabled == false)
                        mIsLogEnabled = true;
                    if (args[i].ToLower() == "-console" && mIsService == true)
                    {
                        mIsService = false;
                        Program Svc = new Program();
                        Svc.OnStart(args);
                    }
                }
            }

            if (mIsService == true)
                ServiceBase.Run(new Program());
        }

        public Program()
        {
        }

        protected override void OnStart(string[] args)
        {
            if (mIsService == true)
                base.OnStart(args);

            WebServer.Start(!mIsService, mIsLogEnabled);
        }

        protected override void OnStop()
        {
            if (mIsService == true)
                base.OnStop();

            WebServer.Stop();
        }
    
    }
}
