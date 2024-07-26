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
    public class PostModel : RecordBase
    {
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(200, ErrorMessage = "Title must be maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }

        public string UserId { get; set; } // Changed to string
        public int BlogId { get; set; }

        [Required(ErrorMessage = "BlogName is required")]
        public string BlogName { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }



}
