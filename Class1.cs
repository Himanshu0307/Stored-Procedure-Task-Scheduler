using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskScheduleSQL
{
    internal class VariableClassScheduler
    {
        private Timer timer;

        public Action CallBack;


        public VariableClassScheduler() { }

        public void ChangeTime()
        {
            timer.Dispose();
          
        }

        //private TimerCallback GetTimerCallback()
        //{

        //}

    }
}
