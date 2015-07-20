using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Helpers
{
    public class BatchManager
    {
        public BatchManager()
        {
        }
        public void WriteInstall(string path)
        {
            using (StreamWriter sw = new StreamWriter("Install.bat"))
            {
                sw.WriteLine("cd " + path);
                sw.WriteLine("hyperbase.live.client /i");
                sw.WriteLine("pause");

            }
        }
        public void WriteStart(string serviceName)
        {
            using (StreamWriter sw = new StreamWriter("Start.bat"))
            {
                sw.WriteLine("sc start \"" + serviceName + "\"");
                sw.WriteLine("pause");
            }
        }
        public void WriteStop(string serviceName)
        {
            using (StreamWriter sw = new StreamWriter("Stop.bat"))
            {
                sw.WriteLine("sc stop \"" + serviceName + "\"");
                sw.WriteLine("pause");
            }
        }
        public void LaunchInstall()
        {
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = @"Install.bat";
            proc1.Verb = "runas";
            Process.Start(proc1);
        }

        public void LaunchStart()
        {
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = @"Start.bat";
            proc1.Verb = "runas";
            Process.Start(proc1);
        }
        public void LaunchStop()
        {
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.FileName = @"Stop.bat";
            proc1.Verb = "runas";
            Process.Start(proc1);
        }
    }
}
