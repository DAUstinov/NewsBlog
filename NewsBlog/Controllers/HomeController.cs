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
        
          public HomeController(BlogContext blogContext)
          {
              _blogService = new BlogService(blogContext);
          }
        
          public ActionResult Index(int page = 1)
          {
              //string tagCollection, string categoryCollection,
             var categoryCollection = new SelectList(_db.BlogItems.Select(
                 row => new SelectListItem 
                     { Text = row.Category, Value = row.Category }).Distinct().ToList());
              var tagCollection = new SelectList(_db.Tags.Select(
                  row => new SelectListItem 
                      { Text = row.TagName, Value = row.TagName }).ToList());
              const int pageSize = 6;
              var articlesPerPAge = _db.BlogItems.ToList().Skip(
                  (page - 1) * pageSize).Take(pageSize);
              var pageInfo = new PageInfo 
                  { PageNumber = page, PageSize = pageSize, TotalItems = _db.BlogItems.ToList().Count };
              var pvm = new PageViewModel()
              {
                  PageInfo = pageInfo,
                  BlogItems = articlesPerPAge,
                  Category = categoryCollection,
                  TagName = tagCollection
        
              };
              return View(pvm);
          }
        
          public ActionResult Admin(int page = 1)
          {
             var categoryCollection = new SelectList(_db.BlogItems.Select(
                 row => new SelectListItem
                     { Text = row.Category, Value = row.Category }).Distinct().ToList());
             var tagCollection = new SelectList(_db.Tags.Select(
                 row => new SelectListItem
                     { Text = row.TagName, Value = row.TagName }).ToList());
             const int pageSize = 6;
             var articlesPerPAge = _db.BlogItems.ToList().Skip(
                 (page - 1) * pageSize).Take(pageSize);
             var pageInfo = new PageInfo
                 { PageNumber = page, PageSize = pageSize, TotalItems = _db.BlogItems.ToList().Count };
             var pvm = new PageViewModel()
             {
                 PageInfo = pageInfo,
                 BlogItems = articlesPerPAge,
                 Category = categoryCollection,
                 TagName = tagCollection
        
             };
             return View(pvm);
          }
        
          public ActionResult CreateNews(BlogItem blog)
          {
              return View(blog);
          }
        
          [HttpPost]
          public ActionResult CreateNews(BlogItem blog , HttpPostedFileBase uploadImage)
          {
              
              if (ModelState.IsValid && uploadImage != null)
              {
                  byte[] imageData;
                  using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                  {
                      imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                  }
                  blog.Image = imageData;
        
                  _blogService.AddItem(blog);
        
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