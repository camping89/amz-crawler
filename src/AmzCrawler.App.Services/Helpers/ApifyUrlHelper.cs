namespace AmzCrawler.App.Services.Helpers
{
    public class ApifyUrlHelper
    {
        private const string BaseApifyUrl = "https://api.apify.com/v2";

        /// <summary>
        /// This url can be used to run task, get list runs of task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CreateRunTaskUrl(string taskId, string token)
        {
            return $"{BaseApifyUrl}/actor-tasks/{taskId}/runs?token={token}";
        }

        /// <summary>
        /// This url can be used to run task synchronously
        /// Method might be used with both Post and Get
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CreateRunTaskSynchronouslyUrl(string taskId, string token)
        {
            return $"{BaseApifyUrl}/actor-tasks/{taskId}/run-sync?token={token}";
        }


        /// <summary>
        /// This url can be used to run task synchronously and get data
        /// Method might be used with both Post and Get
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CreateRunTaskAndGetDataUrl(string taskId, string token)
        {
            return $"{BaseApifyUrl}/actor-tasks/{taskId}/run-sync-get-dataset-items?token={token}";
        }


        /// <summary>
        /// This url can be used to get task info, update task settings, or delete task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CreateTaskUrl(string taskId, string token)
        {
            return $"{BaseApifyUrl}/actor-tasks/{taskId}?token={token}";
        }

        /// <summary>
        /// This url can be used to get task input, update task input
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CreateTaskInputUrl(string taskId, string token)
        {
            return $"{BaseApifyUrl}/actor-tasks/{taskId}/input?token={token}";
        }

        /// <summary>
        /// This url can be used to get last run info
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        /// <param name="successedOnly">To only get the last successful run of the actor task</param>
        /// <returns></returns>
        public static string CreateLastRunUrl(string taskId, string token, bool successedOnly)
        {
            var url = $"{BaseApifyUrl}/actor-tasks/{taskId}/runs/last?token={token}";
            if (successedOnly)
            {
                url += "&status=SUCCEEDED";
            }

            return url;
        }

        /// <summary>
        /// This url can be used to get last run dataset
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="token"></param>
        ///  /// <param name="successedOnly">To only get dataset of the last successful run of the actor task. Default is true</param>
        /// <returns></returns>
        public static string CreateLastRunDataUrl(string taskId, string token, bool successedOnly = true)
        {
            var url = $"{BaseApifyUrl}/actor-tasks/{taskId}/runs/last/dataset/items?token={token}";
            if (successedOnly)
            {
                url += "&status=SUCCEEDED";
            }

            return url;
        }
    }
}
