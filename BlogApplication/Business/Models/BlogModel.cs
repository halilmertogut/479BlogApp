using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class BlogModel : RecordBase
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name must be maximum {1} characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must be maximum {1} characters.")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<PostModel> Posts { get; set; } = new List<PostModel>();
    }
}
