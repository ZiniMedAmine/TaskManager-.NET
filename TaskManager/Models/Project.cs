using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!; 

        public virtual ICollection<AppTask>? Tasks { get; set; }
    }
}