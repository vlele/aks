namespace TaskAPI.Models
{
    public class DeleteTaskRequest
    {
        public string TaskListId { get; set; }
        public string TaskId { get; set; }
    }
}
