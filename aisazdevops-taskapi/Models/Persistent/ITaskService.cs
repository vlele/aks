using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAPI.Models;

namespace TaskAPI.AspNetCore.Web.Models.Persistent
{
    public interface ITaskService
    {
        List<User> Users
        {

            get;

            set;
        }

        List<TaskList> TaskLists
        {

            get;

            set;
        }

        List<TaskAPI.Models.Task> Tasks
        {

            get;

            set;
        }


        bool AddUser(User newUser);

        bool RemoveUser(User oldUser);

        bool AddTaskList(TaskList newTaskList);

        bool RemoveTaskList(TaskList oldTaskList);

        bool AddTask(TaskAPI.Models.Task newTask);

        bool RemoveTask(TaskAPI.Models.Task oldTask);
        bool UpdateTask(TaskAPI.Models.Task existingTask);
    }
}
