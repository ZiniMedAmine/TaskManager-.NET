using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class AppTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Range(1, 5, ErrorMessage = "Priority must be between 1 (Low) and 5 (High).")]
        public int Priority { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Foreign Key
        public int ProjectId { get; set; }

        // Navigation property
        [ForeignKey("ProjectId")]
        public virtual Project? Project { get; set; }
    }
}