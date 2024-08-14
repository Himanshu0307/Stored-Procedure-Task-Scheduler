using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace TaskScheduleSQL
{
    public class TaskScheduler
    {
        public Timer timer;
        public LogService log;


        public TaskScheduler(ref LogService log){
            this.log=log;
        }



        public void StopTimer(){
            timer.Change(Timeout.Infinite,Timeout.Infinite);
            timer.Dispose();
        }

        
        public void ScheduleTaskAndStart(Int64 offset,Int64 period,TimerCallback callback)
        {
           
           
            timer=new Timer(callback,this,offset,period);
          

             log.LogInformation("Total Time left to Run:"+offset+" Milliseconds");
        }


        
    }
}