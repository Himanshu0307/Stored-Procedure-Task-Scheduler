using System;
using System.Collections.Generic;
using System.Globalization;
using TaskScheduleSQL.Models;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Linq;

namespace TaskScheduleSQL
{
    public class TaskActionService
    {
        private Database DatabaseInfo;
        private Server ServerInfo;

        private LogService logService;

        private String ConnectionString;


        private List<TaskScheduler> totalSchedule = new List<TaskScheduler>();
 
        public TaskActionService(Database database, ref Server server, ref LogService logService)
        {
            this.DatabaseInfo = database;
            this.ServerInfo = server;
            this.logService = logService;
            ConnectionString = $"Data Source={ServerInfo.DatabaseServerName};Initial Catalog={DatabaseInfo.DatabaseName};";
            if (!string.IsNullOrEmpty(ServerInfo.DatabaseUserName))
            {
                ConnectionString.Concat($"UID={ServerInfo.DatabaseUserName};");
            }
            if (!string.IsNullOrEmpty(ServerInfo.DatabasePassword))
            {
                ConnectionString.Concat($"Password={ServerInfo.DatabasePassword};");
            }
            logService.LogInformation(ConnectionString);
        }

        // object sender
        public void ExecuteSP(string Command)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.CommandTimeout = 7200;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = $"{Command}";
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    logService.LogInformation("Successfully Executed " + Command);

                }

            }
            catch (Exception e)
            {
                logService.LogError($"Error Executing {Command}:" + e.ToString());
            }
        }



        public void ChangeDatabase(Database db)
        {
            this.DatabaseInfo = db;
            ConnectionString = $"Data Source={ServerInfo.DatabaseServerName}; UID={ServerInfo.DatabaseUserName}; Password={ServerInfo.DatabasePassword};Database={DatabaseInfo.DatabaseName};TrustServerCertificate=True;";


        }



        public void ScheduleAllDatabaseSPByTask()
        {

            foreach (Database db in this.ServerInfo.Databases)
            {
                ChangeDatabase(db);

                foreach (Scheduler schedule in DatabaseInfo.Schedule)
                {


                    this.ScheduleBasedOnType(schedule);
                }


            }
        }

        public void ScheduleBasedOnType(Scheduler schedule)
        {
            try
            {
                if (schedule.Type == "Daily")
                {
                    int hour = int.Parse(schedule.Time.Substring(0, 2));
                    int minute = int.Parse(schedule.Time.Substring(3, 2));
                    TaskScheduler task = new TaskScheduler(ref logService);
                    DateTime now = DateTime.Now;
                    DateTime scheduledTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 00);
                    if (now > scheduledTime)
                    {
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                    Int64 interval = (Int64)((scheduledTime - now).TotalMilliseconds);

                    task.ScheduleTaskAndStart(offset: interval, 86400000, new TimerCallback(_ => ExecuteSP(schedule.CommandText)));
                    AddTaskInList(task);

                }
                else if (schedule.Type == "Days")
                {


                    TaskScheduler task = new TaskScheduler(ref logService);
                    DateTime now = DateTime.Now;
                    DateTime scheduledTime = DateTime.Parse(schedule.Time);
                    while (now > scheduledTime)
                    {
                        if (schedule.Days == null) schedule.Days = 1;
                        scheduledTime = scheduledTime.AddDays((double)schedule.Days);
                    }
                    Int64 interval = (Int64)((scheduledTime - now).TotalMilliseconds);
                    Int64 period = (Int64)(schedule.Days*24*60*60*1000);
               

                    task.ScheduleTaskAndStart( interval, period, new TimerCallback(_ => ExecuteSP(schedule.CommandText)));
                    AddTaskInList(task);
                }
                else if (schedule.Type == "Monthly")
                {


                    TaskScheduler task = new TaskScheduler(ref logService);
                    DateTime now = DateTime.Now;
                    DateTime scheduledTime = DateTime.Parse(schedule.Time.ToString());
                    while (now > scheduledTime)
                    {

                        scheduledTime = scheduledTime.AddMonths(1);
                    }
                    Int64 interval = (Int64)((scheduledTime - now).TotalMilliseconds);


                    task.ScheduleTaskAndStart(offset: interval, Timeout.Infinite, new TimerCallback(obj =>
                    {
                        ExecuteSP(schedule.CommandText);
                        Timer currentObject = (Timer)obj;
                        DateTime nextTargetDate = DateTime.Today.AddMonths(1);
                        TimeSpan timeUntilNextTarget = nextTargetDate - DateTime.Now;
                        currentObject.Change((Int64)timeUntilNextTarget.TotalMilliseconds, Timeout.Infinite);

                    }));
                    AddTaskInList(task);
                }
                else
                {
                    throw new Exception("Invalid Schedule Type Provided. Select either Daily or Days or Monthly ");
                }
            }
            catch (Exception ex)
            {
                logService.LogError(ex.ToString());
            }

        }


        public void AddTaskInList(TaskScheduler taskScheduler)
        {
            totalSchedule.Add(taskScheduler);

        }


        public void RemoveTaskInList(TaskScheduler taskScheduler)
        {
            totalSchedule.Remove(taskScheduler);

        }

        public void UnScheduleAllTask()
        {
            foreach (TaskScheduler taskScheduler in totalSchedule)
            {
                taskScheduler.StopTimer();
            }

            this.totalSchedule = new List<TaskScheduler>();
        }




    }
}
