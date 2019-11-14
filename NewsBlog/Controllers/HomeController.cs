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
         private readonly BlogService _blogService = new BlogService();

         
         public ActionResult Index()
         {
             return View(_db.BlogItems.ToList());
         }

         public ActionResult Admin()
         {
             
             return View(_db.BlogItems.ToList());
         }

         public ActionResult Create()
         {
             return View();
         }

         [HttpPost]
         public ActionResult Create(Image pic, HttpPostedFileBase uploadImage)
         {
             if (ModelState.IsValid && uploadImage != null)
             {
                 byte[] imageData = null;
                 // считываем переданный файл в массив байтов
                 using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                 {
                     imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                 }
                 // установка массива байтов
                 pic.Picture = imageData;

                 _db.Images.Add(pic);
                 _db.SaveChanges();

                 return RedirectToAction("Admin");
             }
             return View(pic);
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
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                blog.Image = imageData;

                _blogService.AddItem(blog);


                return RedirectToAction("Admin");
            }
            return View(blog);
         }

         [HttpGet]
         public ActionResult Article(int id)
         {
             ViewBag.NewsId = id;
        
             return View();
         }
        
         [HttpPost , ActionName("Article")]
         public ActionResult Article(int id)
         {
             _blogService.Article();
             return RedirectResult();
         }
    }
}