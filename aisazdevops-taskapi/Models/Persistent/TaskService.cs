using System;
using System.Collections.Generic;
using System.Linq;
using TaskAPI.Models;

using MongoDB.Bson;
using MongoDB.Driver.Core;
using MongoDB.Driver;

using Microsoft.Extensions.Configuration;

//using System.Threading.Tasks;

namespace TaskAPI.AspNetCore.Web.Models.Persistent
{
    public class TaskService : ITaskService
    {
        private List<User> _lstUsers;

        private List<TaskList> _lstTaskLists;
        private List<Task> _lstTasks;

        private IMongoClient _client;
        private IMongoDatabase _database;
        private string _ConnectionString;

        private const string TASK_DATABASE_NAME = "Tasks";
        private const string USER_COLLECTION_NAME = "Users";
        private const string TASKLIST_COLLECTION_NAME = "UserTaskLists";
        private const string TASK_COLLECTION_NAME = "UserTasks";

        public TaskService(string connectionString)
        {
            //var connStr = ConfigurationExtensions.GetConnectionString("TaskDBConn");   

            _ConnectionString = connectionString;
            _client = new MongoClient(_ConnectionString);
            _database = _client.GetDatabase(TASK_DATABASE_NAME);
        }

        public List<User> Users
        {
            get { return GetAllUsers(); }

            set { _lstUsers = value; }

        }
        public List<Task> Tasks
        {
            get { return GetAllTasks(); }

            set { _lstTasks = value; }
        }
        public List<TaskList> TaskLists
        {
            get { return GetAllTaskLists(); }
            set { _lstTaskLists = value; }

        }


        private List<User> GetAllUsers()
        {
            var lstUsers = new List<User>();

            lstUsers = _database.GetCollection<User>(USER_COLLECTION_NAME).Find(o => o.UserId != null).ToList();
            return lstUsers;
        }



        public bool AddUser(User newUser)
        {
            try
            {
                _database.GetCollection<User>(USER_COLLECTION_NAME).InsertOne(newUser);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUser(User oldUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq("userId", oldUser.UserId);
                var ud = Builders<User>.Update.Set("IsDeleted", true);
                _database.GetCollection<User>(USER_COLLECTION_NAME).UpdateOne(filter, ud);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }


        private List<TaskList> GetAllTaskLists()
        {
            var lstTaskList = new List<TaskList>();

            lstTaskList = _database.GetCollection<TaskList>(TASKLIST_COLLECTION_NAME).Find(o => o.TaskListId != null).ToList();
            return lstTaskList;
        }


        public bool AddTaskList(TaskList newTaskList)
        {
            try
            {
                _database.GetCollection<TaskList>(TASKLIST_COLLECTION_NAME).InsertOne(newTaskList);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool RemoveTaskList(TaskList oldTaskList)
        {
            try
            {
                var filter = Builders<TaskList>.Filter.Eq("TaskListId", oldTaskList.TaskListId);
                var ud = Builders<TaskList>.Update.Set("IsDeleted", true);
                _database.GetCollection<TaskList>(TASKLIST_COLLECTION_NAME).UpdateOne(filter, ud);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }


        private List<Task> GetAllTasks()
        {
            var lstTasks = new List<Task>();

            lstTasks = _database.GetCollection<Task>(TASK_COLLECTION_NAME).Find(o => o.TaskId != null).ToList();
            return lstTasks;
        }


        public bool AddTask(Task newTask)
        {
            try
            {
                _database.GetCollection<Task>(TASK_COLLECTION_NAME).InsertOne(newTask);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveTask(Task oldTask)
        {
            try
            {
                var filter = Builders<Task>.Filter.Eq("TaskId", oldTask.TaskId);
                var ud = Builders<Task>.Update.Set("IsDeleted", true);

                _database.GetCollection<Task>(TASK_COLLECTION_NAME).UpdateOne(filter, ud);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public bool UpdateTask(Task oldTask)
        {
            try
            {
                var filter = Builders<Task>.Filter.Eq("Id", oldTask.Id);
                var ud = Builders<Task>.Update.Set("IsCompleted", oldTask.IsCompleted)
                                              .Set("CompletedOnUtc", oldTask.CompletedOnUtc)
                                              .Set("DueOnUtc", oldTask.DueOnUtc)
                                              .Set("IsActive", oldTask.IsActive)
                                              .Set("Title", oldTask.Title)
                    ;
                _database.GetCollection<Task>(TASK_COLLECTION_NAME).UpdateOne(filter, ud);
                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

    }
}
