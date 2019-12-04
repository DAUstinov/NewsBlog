using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsBlog.Migrations;
using NewsBlog.Models;
using NewsBlog.Services;

namespace NewsBlog.Controllers
{
    public class HomeController : Controller
    {

        public PageViewModel Test(string startDate, string endDate, string tagString, string searchString, int page = 1)
        {
            const int pageSize = 6;
            var articlesPerPAge = _db.BlogItems.ToList().Skip(
                (page - 1) * pageSize).Take(pageSize);
            var pageInfo = new PageInfo
            { PageNumber = page, PageSize = pageSize, TotalItems = _db.BlogItems.ToList().Count };
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                articlesPerPAge = articlesPerPAge.Where(d => d.Date <= Convert.ToDateTime(endDate)
                                                             && d.Date >= Convert.ToDateTime(startDate)).ToList();
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                articlesPerPAge = articlesPerPAge.Where(s => s.CategoryId == Convert.ToInt32(searchString)).ToList();
            }
            if (!string.IsNullOrEmpty(tagString))
            {
                articlesPerPAge = articlesPerPAge.Where(s => s.Tags
                    .Any(t => t.TagName == tagString)).ToList();
            }
            if (!string.IsNullOrEmpty(tagString) && !string.IsNullOrEmpty(searchString))
            {
                articlesPerPAge = articlesPerPAge.Where(s => s.CategoryId == Convert.ToInt32(searchString)
                                                             && s.Tags.Any(t => t.TagName == tagString)).ToList();
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(tagString))
            {
                articlesPerPAge = articlesPerPAge.ToList()
                    .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                && d.Date >= Convert.ToDateTime(startDate)
                                && d.Tags.Any(t => t.TagName == tagString));
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(searchString))
            {
                articlesPerPAge = articlesPerPAge.ToList()
                    .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                && d.Date >= Convert.ToDateTime(startDate)
                                && d.CategoryId == Convert.ToInt32(searchString));
            }
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(tagString) && !string.IsNullOrEmpty(searchString))
            {
                articlesPerPAge = articlesPerPAge.ToList()
                    .Where(d => d.Date <= Convert.ToDateTime(endDate)
                                && d.Date >= Convert.ToDateTime(startDate)
                                && d.CategoryId == Convert.ToInt32(searchString)
                                && d.Tags.Any(t => t.TagName == tagString));
            }
            var pvm = new PageViewModel()
            {
                PageInfo = pageInfo,
                BlogItems = articlesPerPAge,
                Category = new SelectList(_db.Categories, "Id", "Name")
            };

            return pvm;
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

        public ActionResult CreateNews(BlogItem blog)
        {
            ViewBag.Tags = _db.Tags.ToList();
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            return View(blog);
        }

        [HttpPost]
        public ActionResult CreateNews(BlogItem blog, HttpPostedFileBase uploadImage, int[] selectedTag)
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
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }

                blog.Image = imageData;

                _db.BlogItems.Add(blog);
                _db.SaveChanges();

                return RedirectToAction("Admin");
            }
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            ViewBag.Tags = _db.Tags.ToList();
            return View(blog);
        }

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
        public ActionResult Delete(int id)
        {
            _blogService.DeleteArticle(id);
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public ActionResult UpdateArticle(int id)
        {
            var blog = _blogService.GetArticle(id);
            ViewBag.Tags = _db.Tags.ToList();
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            return View(blog);
        }

        [HttpPost, ActionName("UpdateArticle")]
        public ActionResult Update(BlogItem item, HttpPostedFileBase uploadImage, int[] selectedTag)
        {
            var newItem = _db.BlogItems.Find(item.NewsId);
            if (newItem != null)
            {
                newItem.Description = item.Description;
                newItem.Image = item.Image;
                newItem.ShortDescription = item.ShortDescription;
                newItem.Name = item.Name;
                newItem.Tags = item.Tags;
                newItem.Date = DateTime.UtcNow;
                newItem.CategoryId = item.CategoryId;

                if (selectedTag != null)
                {
                    newItem.Tags.Clear();
                    foreach (var c in _db.Tags.Where(co => selectedTag.Contains(co.Id)))
                    {
                        newItem.Tags.Add(c);
                    }
                }

                if (ModelState.IsValid && uploadImage != null)
                {
                    byte[] imageData;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }

                    newItem.Image = imageData;
                    _db.Entry(newItem).State = EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("Admin");
                }
            }
            var categories = new SelectList(_db.Categories, "Id", "Name");
            ViewBag.Categories = categories;
            ViewBag.Tags = _db.Tags.ToList();

            return View(item);
        }

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
        public ActionResult CreateNewCategory(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public ActionResult EditCategory(Category category)
        {
            return View(category);
        }

        [HttpPost , ActionName("EditCategory")]
        public ActionResult EditThatCategory(Category category)
        {
            _db.Entry(category).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        public ActionResult DeleteCategory(int id)
        {
            var category = _db.Categories.Find(id);
            return View(category);
        }

        [HttpPost , ActionName("DeleteCategory")]
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
        public ActionResult CreateThatTag(Tag tag)
        {
            _db.Tags.Add(tag);
            _db.SaveChanges();
            return RedirectToAction("TagList");
        }

        public ActionResult DeleteTag(Tag tag)
        {
            return View(tag);
        }

        [HttpPost]
        public ActionResult DeleteTag(int id)
        {
            var tag = _db.Tags.Find(id);
            _db.Tags.Remove(tag ?? throw new InvalidOperationException());
            _db.SaveChanges();
            return RedirectToAction("CategoryList");
        }

    }
}