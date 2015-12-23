using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Diagnostics;

namespace GreenEyeAPI
{
    [RunInstaller(true)]
    public class FireWallOpenerInstaller : Installer
    {
        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            var path = Path.GetDirectoryName(Context.Parameters["assemblypath"]);
            OpenFirewallForProgram(Path.Combine(path, "GreenEyeAPIService.exe"),
                                   "GreenEye API Local Server");
        }

        private static void OpenFirewallForProgram(string exeFileName, string displayName)
        {
            var proc = Process.Start(
                new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments =
                            string.Format(
                                "http add urlacl url=http://+:8008/ user=Everyone listen=yes",
                                exeFileName, displayName),
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
            proc.WaitForExit();
        }
    }
}