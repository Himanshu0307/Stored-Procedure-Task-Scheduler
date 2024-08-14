using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskScheduleSQL.Models
{
    public class Database
    {
       public String DatabaseName="";

       

        public String CompanyName{get;set;}="";

        public List<Scheduler> Schedule { get; set; } = new List<Scheduler>();
         public Database(string databaseName, string companyName,List<Scheduler> Sc)
        {
            DatabaseName = databaseName;
            CompanyName = companyName;
        }

     
    }
}