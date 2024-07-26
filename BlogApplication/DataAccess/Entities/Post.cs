using DataAccess.Entities;
using DataAccess.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities { 
public class Post : RecordBase
{
    [Required, StringLength(200)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public string UserId { get; set; } // Ensure this is string
    public int BlogId { get; set; }

    public ApplicationUser User { get; set; } // Ensure this references ApplicationUser
    public Blog Blog { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
}
