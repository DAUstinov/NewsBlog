using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsBlog.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<BlogItem> BlogItems { get; set; }
        public Category()
        {
            BlogItems = new List<BlogItem>();
        }
    }
}