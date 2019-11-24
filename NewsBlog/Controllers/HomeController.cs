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

          private readonly BlogContext _db = new BlogContext();
          private readonly BlogService _blogService;
        
          public HomeController(DbContext dbContext)
          {
              _blogService = new BlogService(dbContext);
          }
        
          public ActionResult Index(string tagString ,string searchString , int page = 1)
          {
              const int pageSize = 6;
              var articlesPerPAge = _db.BlogItems.ToList().Skip(
                  (page - 1) * pageSize).Take(pageSize);
              var pageInfo = new PageInfo 
                  { PageNumber = page, PageSize = pageSize, TotalItems = _db.BlogItems.ToList().Count };
              if (!string.IsNullOrEmpty(searchString))
              {
                  articlesPerPAge = articlesPerPAge.Where(s => s.Category.Contains(searchString));
              }
              var tagFilter = from s in _db.Tags select s;
              if (!string.IsNullOrEmpty(tagString))
              {
                  tagFilter = tagFilter.Where(s => s.TagName.Contains(tagString));
              }
              var pvm = new PageViewModel()
              {
                  PageInfo = pageInfo,
                  BlogItems = articlesPerPAge,
                  Tags = tagFilter
              };
              return View(pvm);
          }
        
          public ActionResult Admin(string tagString, string searchString, int page = 1)
          {
              const int pageSize = 6;
              var articlesPerPAge = _db.BlogItems.ToList().Skip(
                  (page - 1) * pageSize).Take(pageSize);
              var pageInfo = new PageInfo
                  { PageNumber = page, PageSize = pageSize, TotalItems = _db.BlogItems.ToList().Count };
              if (!string.IsNullOrEmpty(searchString))
              {
                  articlesPerPAge = articlesPerPAge.Where(s => s.Category.Contains(searchString));
              }
              var tagFilter = from s in _db.Tags select s;
              if (!string.IsNullOrEmpty(tagString))
              {
                  tagFilter = tagFilter.Where(s => s.TagName.Contains(tagString));
              }
              var pvm = new PageViewModel()
              {
                  PageInfo = pageInfo,
                  BlogItems = articlesPerPAge,
                  Tags = tagFilter
              };
              return View(pvm);
          }
        
          public ActionResult CreateNews(BlogItem blog)
          {
              ViewBag.Tags = _db.Tags.ToList();
              return View(blog);
          }
        
          [HttpPost]
          public ActionResult CreateNews(BlogItem blog , HttpPostedFileBase uploadImage , int[] selectedTag)
          {
               if (selectedTag != null)
               {
                   foreach (var c in _db.Tags.Where(co => selectedTag.Contains(co.Id)))
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
               return View(blog);
          }
        
          public ActionResult Article(int id)
          {
               var article = _blogService.GetArticle(id);
         
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
              return View(blog);
          }
        
          [HttpPost, ActionName("UpdateArticle")]
          public ActionResult Update(BlogItem item , HttpPostedFileBase uploadImage , int[] selectedTag)
          {
             var newItem = _db.BlogItems.Find(item.NewsId);
             if (newItem != null)
             {
                 newItem.Category = item.Category;
                 newItem.Description = item.Description;
                 newItem.Image = item.Image;
                 newItem.ShortDescription = item.ShortDescription;
                 newItem.Name = item.Name;
                 newItem.Tags = item.Tags;

                 newItem.Tags.Clear();
                 if (selectedTag != null)
                 {
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

             return View(item);
          }

     }
}