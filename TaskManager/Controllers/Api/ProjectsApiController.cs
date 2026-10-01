using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public ProjectsApiController(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: api/ProjectsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.Include(p => p.Tasks).ToListAsync();
        }

        // GET: api/ProjectsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // POST: api/ProjectsApi
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        // PUT: api/ProjectsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.Id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // DELETE: api/ProjectsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
