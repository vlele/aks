using MongoDB.Bson;
using System;

namespace TaskAPI.Models
{
    public class Task
    {
        /// <summary>
        /// Identity
        /// </summary>
        //public int Id { get; set; }
        public ObjectId Id { get; set; }

        /// <summary>
        /// Task List Unique Id
        /// </summary>
        public string TaskListId { get; set; }

        /// <summary>
        /// Task Unique Id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Task due date
        /// </summary>
        public DateTime? DueOnUtc { get; set; }

        /// <summary>
        /// Task completed on date
        /// </summary>
        public DateTime? CompletedOnUtc { get; set; }

        /// <summary>
        /// True if task is completed 
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Task created on date
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Task updated on date
        /// </summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>
        /// True if task is deleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// True if task is active
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
