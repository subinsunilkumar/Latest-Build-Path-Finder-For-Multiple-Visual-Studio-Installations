using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>();
            const string intel = "INTEL";
            const string amd = "AMD";
            var buildPath = string.Empty;
            var processor = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
            var type = processor.Contains("Intel") ? intel : amd;
            var fileListOlder =
                new DirectoryInfo(@"C:\Program Files (x86)\MSBuild").GetFiles("MSBuild.exe",
                    SearchOption.AllDirectories);
            if (fileListOlder.Length > 0)
            {
                switch (type)
                {
                    case amd:
                        foreach (var file in fileListOlder)
                        {
                            if (file.FullName.Contains("Bin\\amd64\\MSBuild"))
                            {
                                list.Add(file.FullName);
                            }
                        }

                        break;
                    case intel:
                        foreach (var file in fileListOlder)
                        {
                            if (file.FullName.Contains("Bin\\MSBuild"))
                            {
                                list.Add(file.FullName);
                            }
                        }

                        break;
                }

                goto BuildSolution;
            }

            var fileListNewer =
                new DirectoryInfo(@"C:\Program Files (x86)\Microsoft Visual Studio").GetFiles("MSBuild.exe",
                    SearchOption.AllDirectories);
            if (fileListNewer.Length > 0)
            {
                switch (type)
                {
                    case amd:
                        foreach (var file in fileListNewer)
                        {
                            if (file.FullName.Contains("Bin\\amd64\\MSBuild") &&
                                file.FullName.Contains("Professional"))
                            {
                                list.Add(file.FullName);
                            }
                        }

                        break;
                    case intel:
                        foreach (var file in fileListNewer)
                        {
                            if (file.FullName.Contains("Bin\\MSBuild") && file.FullName.Contains("Professional"))
                            {
                                list.Add(file.FullName);
                            }
                        }

                        break;
                }

            }
            else
            {
                Environment.Exit(2);
            }

            BuildSolution:
            buildPath = list[0];
            if (list.Count > 1)
            {
                foreach (var value in list)
                {
                    var subPath = value.Remove(value.IndexOf("Professional"));
                    var fileList = new DirectoryInfo(subPath).GetFiles("devenv.exe", SearchOption.AllDirectories);
                    if (fileList.Length > 0)
                    {
                        buildPath = value;
                    }
                }
            }

            if (buildPath.Equals(string.Empty))
            {
                Environment.Exit(2);
            }

            Console.WriteLine($"\"{buildPath}\"@");
        }

    }
}
        
