using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaskAPI.AspNetCore.Web.Models.Persistent;
using TaskAPI.Models;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //private readonly Models.TaskContext _context;

        private readonly ITaskService _taskService;
        //public UserController(Models.TaskContext context)
        //{
        //    _context = context;
        //}

        public UserController(ITaskService taskservice)
        {
            _taskService = taskservice;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IEnumerable<Models.User>> GetAll()
        {
            //return await Task.FromResult<IEnumerable<Models.User>>(_context.Users.Where(p => p.IsDeleted != true).ToList<Models.User>());
            return _taskService.Users.Where(p => p.IsDeleted != true).ToList();
        }

        // GET api/user/sample@mail.com
        [HttpGet("{email}")]
        public User Get(string email)
        {
            var item = _taskService.Users.Find(obj => (string.Compare(email, obj.EmailAddress, true) == 0) && !obj.IsDeleted.GetValueOrDefault());
            if (item != null)
            {
                return item;
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return item;
            }
        }

        // POST api/user
        [HttpPost]
        public ActionResult Post([FromBody]CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            else
            {
                var itemExists = _taskService.Users.Any(i => (string.Compare(i.EmailAddress, request.EmailAddress, true) == 0));
                if (itemExists)
                {
                    return BadRequest();
                }
                User item = new User();
                item.UserId = Guid.NewGuid().ToString().Replace("-", "");
                item.CreatedOnUtc = DateTime.UtcNow;
                item.UpdatedOnUtc = DateTime.UtcNow;
                item.EmailAddress = request.EmailAddress;
                if (_taskService.AddUser(item))
                {
                    HttpContext.Response.StatusCode = 201;
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // DELETE api/user/3ab4fcbd993f49ce8a21103c713bf47a
        [HttpDelete]
        public IActionResult Delete([FromBody]DeleteUserRequest request)
        {
            var item = _taskService.Users.FirstOrDefault(x => x.UserId == request.UserId && x.IsDeleted != true);
            if (item == null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            item.UpdatedOnUtc = DateTime.UtcNow;
            _taskService.RemoveUser(item);

            return new StatusCodeResult(204); // 201 No Content
        }
    }



}
