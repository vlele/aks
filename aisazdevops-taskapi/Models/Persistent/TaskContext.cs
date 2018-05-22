using Microsoft.EntityFrameworkCore;

namespace TaskAPI.Models
{
    public class TaskContext: DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }

    }
}
