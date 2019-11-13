using System.IO;
using System.Web;
using System.Web.Mvc;
using NewsBlog.Models;

namespace NewsBlog.Controllers
{
     public class HomeController : Controller
     {

         private readonly BlogContext _db = new BlogContext();
         public ActionResult Index()
         {
             return View(_db.BlogItems);
         }

         public ActionResult Admin()
         {
             return View(_db.BlogItems);
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

         public ActionResult CreateNews()
         {
             return View();
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

                _db.BlogItems.Add(blog);
                _db.SaveChanges();

                return RedirectToAction("Admin");
            }
            return View(blog);
         }
    }
}