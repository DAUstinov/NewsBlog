using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Models
{
    public class BlogItem
    {
        [Key]
        public int NewsId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public BlogItem()
        {
            Tags = new List<Tag>();
        }

    }
}