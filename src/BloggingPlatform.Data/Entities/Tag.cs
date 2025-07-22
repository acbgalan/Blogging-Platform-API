using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The field is required")]
        [StringLength(20, ErrorMessage = "")]
        public required string Name { get; set; }

        [ForeignKey(nameof(Post))]
        public int PostId { get; set; }

        public Post? Post { get; set; }
    }
}
