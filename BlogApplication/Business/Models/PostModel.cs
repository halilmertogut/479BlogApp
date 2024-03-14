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
        #region Entity Properties
        [Required(ErrorMessage = "Title is required!")]
        [StringLength(200, ErrorMessage = "Title must be maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }

        public int UserId { get; set; }
        public int BlogId { get; set; }
        #endregion

    }
}
