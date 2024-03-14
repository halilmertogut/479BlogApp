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
    public class User : RecordBase
    {
        [Required, StringLength(100)]
        public string Username { get; set; }
        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }
        [Required, StringLength(255)]
        public string PasswordHash { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
    }

}
