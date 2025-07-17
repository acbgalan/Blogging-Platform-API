using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required")]
        [StringLength(100, ErrorMessage = "The field cannot exceed {1} characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "The field is required")]
        [StringLength(2500, ErrorMessage = "The field cannot exceed {1} characters")]
        public required string Content { get; set; }

        [Required(ErrorMessage = "The field is required")]
        [StringLength(25, ErrorMessage = "The field cannot exceed {1} characters")]
        public required string Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

    }
}
