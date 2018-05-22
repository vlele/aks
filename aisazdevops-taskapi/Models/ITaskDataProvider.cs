using System.Collections.Generic;

namespace TaskAPI.Models
{
    public interface ITaskDataProvider
    {
        IEnumerable<User> Users { get; set; }
        IEnumerable<Task> Tasks { get; set; }
        IEnumerable<TaskList> TaskLists { get; set; }
    }
}
