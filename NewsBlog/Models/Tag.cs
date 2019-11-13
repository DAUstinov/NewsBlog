using System.Collections.Generic;

namespace NewsBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<BlogItem> BlogItems { get; set; }
        public Tag()
        {
            BlogItems = new List<BlogItem>();
        }
    }
}