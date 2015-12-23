using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Logic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool IsFullMode = false;
            if (args.Length > 0)
                IsFullMode = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(IsFullMode));

        }
    }
}
