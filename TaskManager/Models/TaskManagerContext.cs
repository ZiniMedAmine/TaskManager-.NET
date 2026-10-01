using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<AppTask> AppTasks { get; set; } = null!;
    }
}