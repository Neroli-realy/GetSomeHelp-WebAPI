using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GetSomeHelp.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System;

namespace GetSomeHelp.Controllers
{
    
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : Controller
    {

        private GSHContext dbContext;

        public TasksController(){
            var connectionString = "";
            this.dbContext = GSHContextFactory.Create(connectionString);
        }

        [HttpGet("all")]
        public ActionResult GetTasks(){
            if(!this.ModelState.IsValid)
                return BadRequest();
            return Ok(this.dbContext.Task.ToArray());
        }

        [HttpGet("filter")]
        public ActionResult GetTasks([FromQuery]bool? finished, [FromQuery]string location){
            if(!this.ModelState.IsValid)
                return BadRequest();
            if(String.IsNullOrEmpty(location) || String.IsNullOrWhiteSpace(location))
                location = "";
            if(finished == null)
                return Ok(this.dbContext.Task.ToArray().Where(x => x.Location.Contains(location)));

            return Ok(this.dbContext.Task.ToArray().Where(x => x.Finished == finished && x.Location.Contains(location)));
        }

        [HttpPost("addTask")]
        public ActionResult addTask([FromBody]Task task, [FromHeader(Name = "Authorization")]string AuthToken){
            if (!this.ModelState.IsValid) {
                return BadRequest();
            }

            task.Asker = getNameFromToken(AuthToken);
            this.dbContext.Add(task);
            this.dbContext.SaveChanges();
            return Created($"api/tasks/{task.ID}",task);
        }

        private string getNameFromToken(string AuthToken){
            AuthToken = AuthToken.Split(' ')[1].ToString();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(AuthToken);
            var asker = token.Claims.SingleOrDefault(c => c.Type.Equals("Name"));
            return asker.Value.ToString();
        }

        [HttpGet("AcceptTask/{id}")]
        public ActionResult acceptTask(int id, [FromHeader(Name = "Authorization")]string AuthToken){
            if (!this.ModelState.IsValid) {
                return BadRequest();
            }

            Task task = this.dbContext.Task.SingleOrDefault(t => t.ID == id);
            task.Accepter = getNameFromToken(AuthToken);
            if(task != null){
                this.dbContext.Entry(task).CurrentValues.SetValues(task);
                this.dbContext.SaveChanges();
                return Ok();
            } else{
                return NotFound();
            }
        }

        [HttpGet("MarkFinished/{id}")]
        public ActionResult markFinished(int id, [FromHeader(Name = "Authorization")]string AuthToken){
            if (!this.ModelState.IsValid) {
                    return BadRequest();
                }

                Task task = this.dbContext.Task.SingleOrDefault(t => t.ID == id);
                if(task != null){
                    if(task.Asker.Equals(getNameFromToken(AuthToken)))
                    {
                        task.Finished = true;
                        this.dbContext.Entry(task).CurrentValues.SetValues(task);
                        this.dbContext.SaveChanges();
                        return Ok();
                    }   
                    else
                        return Unauthorized(); 
                }
                return NotFound();
        }

      
    }
}