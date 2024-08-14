namespace TaskScheduleSQL{
    public class Scheduler{

        public string Time{get;set;}="00:00";

        public string CommandText{get;set;}="";
        public string Type { get; set; } = "Daily";

        public int? Days { get; set; } 


    }
}