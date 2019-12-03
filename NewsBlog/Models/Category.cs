using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlogItem> BlogItems { get; set; }
        public Category()
        {
            BlogItems = new List<BlogItem>();
        }
    }
}