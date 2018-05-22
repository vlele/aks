namespace TaskAPI.Models
{
    /// <summary>
    /// Request to create a task list
    /// </summary>
    public class CreateTaskListRequest
    {
        /// <summary>
        /// User Id for whom the task list is being created
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Title / Description of the task list
        /// </summary>
        public string TaskListTitle { get; set; }
    }

}
