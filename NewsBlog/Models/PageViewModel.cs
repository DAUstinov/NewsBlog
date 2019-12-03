using System.Collections.Generic;
using System.Web.Mvc;

namespace NewsBlog.Models
{
    public class PageViewModel
    {
        public IEnumerable<BlogItem> BlogItems { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public SelectList Category { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}