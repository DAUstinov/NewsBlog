using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string TagName { get; set; }
        public virtual ICollection<BlogItem> BlogItems { get; set; }
        public Tag()
        { 
            BlogItems = new List<BlogItem>();
        }
    }
}