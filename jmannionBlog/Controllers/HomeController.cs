using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;


namespace jmannionBlog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        [RequireHttps]
        public ActionResult index(int? page, string query)
        {

            var result = db.Posts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                //Search Function
                result = result.Where(p => p.Body.Contains(query))
               .Union(db.Posts.Where(p => p.Title.Contains(query)))
               .Union(db.Posts.Where(p => p.Comments.Any(c => c.Body.Contains(query)))
               .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.FirstName.Contains(query))))
               .Union(db.Posts.Where(p => p.Comments.Any(c => c.Author.DisplayName.Contains(query)))));
            }
            //page numbers
            int pageSize = 3; // the number of posts shown per page.
            int pagenumber = (page?? 1); // if no posts set default page as page 1.

            var qpost = result.OrderByDescending(c => c.Id).ToPagedList(pagenumber, pageSize);
            return View(qpost);


            //return View(db.Posts.OrderByDescending(c => c.Id).Take(5).ToList());
        }
        [RequireHttps]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [RequireHttps]
        public ActionResult post()
        {
            ViewBag.Message = "Your post description page.";

            return View();
        }
        [RequireHttps]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}