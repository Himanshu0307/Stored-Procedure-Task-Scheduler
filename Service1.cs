
using System.ServiceProcess;
using System.IO;
using TaskScheduleSQL.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;

namespace TaskScheduleSQL
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    public partial class Service1 : ServiceBase
    {

        public LogService log = new LogService();
        public Server server;
        public Thread t1;

        public TaskActionService TaskAction;
        public Service1()
        {

            InitializeComponent();

        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);


        private void SetConfig()
        {
            
            if (File.Exists($"{AppContext.BaseDirectory}\\SchedulerConfig.json"))
            {

                using (StreamReader streamReader = new StreamReader($"{AppContext.BaseDirectory}\\SchedulerConfig.json"))
                {
                    //Console.WriteLine(AppContext.BaseDirectory);
                    this.server = JsonConvert.DeserializeObject<Server>(streamReader.ReadToEnd());
                    
                    this.log = new LogService(this.server.BaseDir);
                }



            }
            else
            {
                log.LogError("Please Provide Config File");
            }
        }

        

        protected override void OnStart(string[] args)
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
             this.t1 = new Thread(StartService);
            this.t1.IsBackground = true;
            this.t1.Start();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
           
        }

        public void StartService()
        {
            try
            {
                this.SetConfig();
                TaskAction = new TaskActionService(this.server.Databases[0], ref server, ref log);
                TaskAction.ScheduleAllDatabaseSPByTask();

            }
            catch (System.Exception e)
            {

                log.LogError("Starting Service" + e.ToString());
            }

        }



        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            try
            {
                if (t1.IsAlive)
                    t1.Suspend();
                TaskAction.UnScheduleAllTask();
                TaskAction = null;
            }
            catch (System.Exception e)
            {
                log.LogError("Stopping Service" + e.ToString());
            }

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
    }
}
