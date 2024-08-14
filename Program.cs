﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using TaskScheduleSQL.Models;

namespace TaskScheduleSQL
{
    internal static class Program
    {

        static void Installer(string[] args)
        {
            const string ServiceName = "FinnauxTaskScheduler";

            if (args.Length == 1)
            {
                try
                {
                    string executablePath =
                        Path.Combine(AppContext.BaseDirectory, "TaskScheduleSQL.exe");

                    if (args[0] is "/Install")
                    {

                            Process install = new Process();
                        try
                        {
                            install.StartInfo.FileName = "cmd.exe";
                            install.StartInfo.CreateNoWindow = true;
                            install.Start();

                            install.StandardInput.Write($"sc create {ServiceName} binPath={executablePath} start=auto");
                            // install.StandardInput.Write($"sc start {ServiceName}");

                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            install.StandardInput.Flush();
                            install.StandardInput.Close();
                            install.WaitForExit();
                            install.Dispose();
                        }

                    }
                    else if (args[0] is "/Uninstall")
                    {

                            Process install = new Process();
                        try
                        {
                            install.StartInfo.FileName = "cmd.exe";
                            install.StartInfo.CreateNoWindow = true;
                            install.Start();

                            install.StandardInput.Write($"sc stop {ServiceName}");
                            install.StandardInput.Write($"sc delete {ServiceName}");
                          
                        }
                        catch (Exception )
                        {

                        }
                        finally
                        {
                            install.StandardInput.Flush();
                            install.StandardInput.Close();
                            install.WaitForExit();
                            install.Dispose();

                        }


                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }


            }
        }





        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }





    }
}
