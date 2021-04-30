namespace AmzCrawler.App.TaskEngine.Services
{
    public class TaskInputModel
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public string Command { get; set; }
        public string SpreadsheetId { get; set; }
        public int? SheetId { get; set; }
        public string SheetName { get; set; }
        public string ApifyTaskId { get; set; }
        public string ApifyToken { get; set; }
        public int? StartIndex { get; set; } = 2;
        public int? EndIndex { get; set; }
        public bool? FailedToSyncOnly { get; set; }
    }

    public class TaskInputModel2
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public string Command { get; set; }
        public string SpreadsheetId { get; set; }
        public int? SheetId { get; set; }
        public string SheetName { get; set; }
        public string ApifyTaskId { get; set; }
        public string ApifyToken { get; set; }
        public bool? FailedToSyncOnly { get; set; }
    }
}
