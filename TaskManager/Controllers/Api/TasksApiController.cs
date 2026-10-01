using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TasksApiController(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: api/TasksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppTask>>> GetTasks()
        {
            return await _context.AppTasks.Include(t => t.Project).ToListAsync();
        }

        // GET: api/TasksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppTask>> GetTask(int id)
        {
            var task = await _context.AppTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // GET: api/TasksApi/priority/3
        [HttpGet("priority/{priority}")]
        public async Task<ActionResult<IEnumerable<AppTask>>> GetTasksByPriority(int priority)
        {
            return await _context.AppTasks
                .Include(t => t.Project)
                .Where(t => t.Priority == priority)
                .ToListAsync();
        }

        // GET: api/TasksApi/project/5
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<AppTask>>> GetTasksByProject(int projectId)
        {
            return await _context.AppTasks
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        // POST: api/TasksApi
        [HttpPost]
        public async Task<ActionResult<AppTask>> CreateTask(AppTask task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AppTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/TasksApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, AppTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PATCH: api/TasksApi/5/toggle
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleTaskCompletion(int id)
        {
            var task = await _context.AppTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();

            return Ok(new { id = task.Id, isCompleted = task.IsCompleted });
        }

        // DELETE: api/TasksApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.AppTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.AppTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.AppTasks.Any(e => e.Id == id);
        }
    }
}
