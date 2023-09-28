using System.ComponentModel.DataAnnotations;

namespace EntityFramework.Entities
{
    public class Post
    {
   
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

 
        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
