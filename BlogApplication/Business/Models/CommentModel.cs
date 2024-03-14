using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CommentModel : RecordBase
    {
        #region Entity Properties
        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        #endregion
    }
}
