using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskScheduleSQL.Models
{
    public class Server
    {
      
        public String DatabaseServerName = "";
        public String DatabaseUserName = "";
        // public List<String> ScheduleTime = new List<String>();

        public String DatabasePassword = "";


        public String BaseDir { get; set; }
        public List<Database> Databases{get;set;}=new List<Database>();

        public Server(string databaseServerName, string databaseUserName,  string databasePassword, string baseDir, List<Database> databases)
        {
            DatabaseServerName = databaseServerName;
            DatabaseUserName = databaseUserName;
            DatabasePassword = databasePassword;
            BaseDir = baseDir;
            Databases = databases;
        }

       

        public override string ToString()
        {
            return $"DatabaseServerName:{DatabaseServerName} \n DatabaseUserName:{DatabaseUserName}";
        }
    }
}