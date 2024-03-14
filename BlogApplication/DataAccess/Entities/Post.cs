using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

#nullable disable

namespace DataAccess.Entities
{
    public class Post : RecordBase
    {
        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int UserId { get; set; }
        public int BlogId { get; set; }

        public User User { get; set; }
        public Blog Blog { get; set; }
        public List<Comment> Comments { get; set; }
    }

}
