using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class AppTasksController : Controller
    {
        private readonly TaskManagerContext _context;

        public AppTasksController(TaskManagerContext context)
        {
            _context = context;
        }

        // GET: AppTasks
        public async Task<IActionResult> Index()
        {
            var tasks = _context.AppTasks.Include(a => a.Project);
            return View(await tasks.ToListAsync());
        }

        // Search Action
        public async Task<IActionResult> SearchByPriority(int? priority)
        {
            var tasks = _context.AppTasks.Include(a => a.Project).AsQueryable();
            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == priority.Value);
            }
            return View("Index", await tasks.ToListAsync());
        }

        // Toggle Task Completion
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var appTask = await _context.AppTasks.FindAsync(id);
            if (appTask == null) return NotFound();

            appTask.IsCompleted = !appTask.IsCompleted;
            _context.Update(appTask);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: AppTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appTask = await _context.AppTasks
                .Include(a => a.Project)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appTask == null) return NotFound();

            return View(appTask);
        }

        // GET: AppTasks/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            return View();
        }

        // POST: AppTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Priority,DueDate,ProjectId,IsCompleted")] AppTask appTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", appTask.ProjectId);
            return View(appTask);
        }

        // GET: AppTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appTask = await _context.AppTasks.FindAsync(id);
            if (appTask == null) return NotFound();

            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", appTask.ProjectId);
            return View(appTask);
        }

        // POST: AppTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Priority,DueDate,ProjectId,IsCompleted")] AppTask appTask)
        {
            if (id != appTask.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppTaskExists(appTask.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", appTask.ProjectId);
            return View(appTask);
        }

        // GET: AppTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appTask = await _context.AppTasks
                .Include(a => a.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appTask == null) return NotFound();

            return View(appTask);
        }

        // POST: AppTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appTask = await _context.AppTasks.FindAsync(id);
            if (appTask != null)
            {
                _context.AppTasks.Remove(appTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppTaskExists(int id)
        {
            return _context.AppTasks.Any(e => e.Id == id);
        }
    }
}