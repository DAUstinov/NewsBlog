using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsBlog.Models;
using NewsBlog.Services;

namespace NewsBlog.Controllers
{
    public class HomeController : Controller
    {
        [HandleError(ExceptionType = typeof(FormatException), View = "FormatError"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError"),
         HandleError(ExceptionType = typeof(ArgumentException), View = "Error")]
        public PageViewModel Test(string startDate, string endDate, string tagString, string searchString, int page = 1)
        {

            const int pageSize = 6;
            var articles = _db.BlogItems.ToList();
            try
            {
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    articles = articles.Where(d => d.Date <= Convert.ToDateTime(endDate)
                                                   && d.Date >= Convert.ToDateTime(startDate)).ToList();
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    articles = articles.Where(s => s.CategoryId
                                                   == Convert.ToInt32(searchString)).ToList();
                }

                if (!string.IsNullOrEmpty(tagString))
                {
                    articles = articles.Where(s => s.Tags
                        .Any(t => t.TagName == tagString)).ToList();
                }

                if (!string.IsNullOrEmpty(tagString) && !string.IsNullOrEmpty(searchString))
                {
                    articles = articles.Where(s => s.CategoryId == Convert.ToInt32(searchString)
                                                   && s.Tags.Any(t => t.TagName == tagString)).ToList();
                }

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) &&
                    !string.IsNullOrEmpty(tagString))
                {
                    articles = articles
                        .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                    && d.Date >= Convert.ToDateTime(startDate)
                                    && d.Tags.Any(t => t.TagName == tagString)).ToList();
                }

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) &&
                    !string.IsNullOrEmpty(searchString))
                {
                    articles = articles
                        .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                    && d.Date >= Convert.ToDateTime(startDate)
                                    && d.CategoryId == Convert.ToInt32(searchString)).ToList();
                }

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) &&
                    !string.IsNullOrEmpty(tagString) && !string.IsNullOrEmpty(searchString))
                {
                    articles = articles
                        .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                    && d.Date >= Convert.ToDateTime(startDate)
                                    && d.CategoryId == Convert.ToInt32(searchString)
                                    && d.Tags.Any(t => t.TagName == tagString)).ToList();
                }

                var articlesPerPAge = articles.Skip(
                    (page - 1) * pageSize).Take(pageSize);
                var pageInfo = new PageInfo
                {
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = _db.BlogItems.ToList().Count
                };
                var pvm = new PageViewModel()
                {
                    PageInfo = pageInfo,
                    BlogItems = articlesPerPAge,
                    Category = new SelectList(_db.Categories, "Id", "Name")
                };

                return pvm;
            }
            catch (FormatException)
            {
                throw;
            }
        }

        private readonly BlogContext _db = new BlogContext();
        private readonly BlogService _blogService;

        public HomeController(DbContext dbContext)
        {
            _blogService = new BlogService(dbContext);
        }

        public ActionResult Index(string startDate, string endDate, string tagString, string searchString, int page = 1)
        {
            var dryView = Test(startDate, endDate, tagString, searchString, page);
            ViewBag.Categories = _db.Categories.ToList();
            return View(dryView);
        }

        public ActionResult Admin(string startDate, string endDate, string tagString, string searchString, int page = 1)
        {
            var dryView = Test(startDate, endDate, tagString, searchString, page);
            ViewBag.Categories = _db.Categories.ToList();
            return View(dryView);
        }

        [HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult CreateNews(BlogItem blog)
        {
            ViewBag.Tags = _db.Tags.ToList();
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            return View(blog);
        }

        [HttpPost]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentOutOfRangeException), View = "OutOfRangeError"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError"),
         HandleError(ExceptionType = typeof(NotImplementedException), View = "ImplementationError"),
         HandleError(ExceptionType = typeof(NotSupportedException), View = "SupportError")]
        public ActionResult CreateNews(BlogItem blog, HttpPostedFileBase uploadImage, int[] selectedTag)
        {
            if (ModelState.IsValid)
            {
                blog.Date = DateTime.UtcNow;
                if (selectedTag != null)
                {
                    foreach (var c in _db.Tags.Where(co => selectedTag
                        .Contains(co.Id)))
                    {
                        blog.Tags.Add(c);
                    }
                }

                if (uploadImage != null)
                {
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }

                    blog.Image = imageData;
                }

                _db.BlogItems.Add(blog);
                _db.SaveChanges();

                ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
                ViewBag.Tags = _db.Tags.ToList();

                return RedirectToAction("Admin");
            }
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Tags = _db.Tags.ToList();
            return View(blog);
        }

        [HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult Article(int id)
        {
            var article = _blogService.GetArticle(id);
            ViewBag.Categories = _db.Categories.ToList();
            return View(article);
        }

        [HttpGet]
        public ActionResult DeleteArticle(int id)
        {
            var item = _blogService.GetArticle(id);
            return View(item);
        }

        [HttpPost, ActionName("DeleteArticle")]
        [HandleError(ExceptionType = typeof(ArgumentException),View = "Error")]
        public ActionResult Delete(int id)
        {
            _blogService.DeleteArticle(id);
            return RedirectToAction("Admin");
        }

        [HttpGet]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult UpdateArticle(int id)
        {
            var blog = _blogService.GetArticle(id);
            ViewBag.Tags = _db.Tags.ToList();
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            return View(blog);
        }

        [HttpPost, ActionName("UpdateArticle")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentOutOfRangeException), View = "OutOfRangeError"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError"),
         HandleError(ExceptionType = typeof(NotImplementedException), View = "ImplementationError"),
         HandleError(ExceptionType = typeof(NotSupportedException),View = "SupportError"),
         HandleError(ExceptionType = typeof(InvalidOperationException) , View = "OperationError" )]
        public ActionResult Update(BlogItem item, HttpPostedFileBase uploadImage, int[] selectedTag)
        {
            var newItem = _db.BlogItems.Find(item.NewsId);
            if (ModelState.IsValid && newItem != null)
            {
                newItem.Description = item.Description;
                newItem.Image = item.Image;
                newItem.ShortDescription = item.ShortDescription;
                newItem.Name = item.Name;
                newItem.Tags = item.Tags;
                newItem.Date = DateTime.UtcNow;
                newItem.CategoryId = item.CategoryId;

                newItem.Tags.Clear();
                if (selectedTag != null)
                {
                    foreach (var c in _db.Tags.Where(co => selectedTag.Contains(co.Id)))
                    {
                        newItem.Tags.Add(c);
                    }
                }
                if (uploadImage != null)
                {
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }

                    newItem.Image = imageData;
                }
                _db.Entry(newItem).State = EntityState.Modified;
                _db.SaveChanges();

                ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
                ViewBag.Tags = _db.Tags.ToList();

                return RedirectToAction("Admin");
            }
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Tags = _db.Tags.ToList();

            return View(item);
        }

        [HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult CategoryList()
        {
            var category = _db.Categories.ToList();
            return View(category);
        }

        public ActionResult CreateCategory(Category category)
        {
            return View(category);
        }

        [HttpPost , ActionName("CreateCategory")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult CreateNewCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _blogService.AddCategory(category);
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

        public ActionResult EditCategory(Category category)
        {
            return View(category);
        }

        [HttpPost , ActionName("EditCategory")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult EditThatCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _blogService.UpdateCategory(category);
                return RedirectToAction("CategoryList");
            }

            return View(category);
        }

        public ActionResult DeleteCategory(int id)
        {
            var category = _blogService.GetCategory(id);
            return View(category);
        }

        [HttpPost , ActionName("DeleteCategory")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error") , 
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult DeleteThatCategory(int id)
        {
            var cascade = _db.Categories.Include(c => c.BlogItems)
                                        .FirstOrDefault(c => c.Id == id);
            _db.Categories.Remove(cascade ?? throw new InvalidOperationException());
            _db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public ActionResult TagList()
        {
            var tags = _db.Tags.ToList();
            return View(tags);
        }

        public ActionResult CreateTag(Tag tag)
        {
            return View(tag);
        }

        [HttpPost , ActionName("CreateTag")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult CreateThatTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                _blogService.AddTags(tag);
                return RedirectToAction("TagList");
            }
            return View(tag);
        }

        public ActionResult DeleteTag(int id)
        {
            var tag = _blogService.GetTag(id);
            return View(tag);
        }

        [HttpPost , ActionName("DeleteTag")]
        [HandleError(ExceptionType = typeof(ArgumentException), View = "Error"),
         HandleError(ExceptionType = typeof(ArgumentNullException), View = "NullError")]
        public ActionResult DeleteThatTag(int id)
        {
            _blogService.DeleteTag(id);
            return RedirectToAction("TagList");
        }

    }
}