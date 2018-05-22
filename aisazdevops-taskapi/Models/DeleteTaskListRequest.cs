namespace TaskAPI.Models
{

    public class DeleteTaskListRequest
    {
        public string UserId { get; set; }
        public string TaskListId { get; set; }
    }
}
