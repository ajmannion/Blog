using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jmannionBlog.Models;
using System.IO;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace jmannionBlog.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
       
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BlogPosts
        public ActionResult Index( int? page)
        {
            //int pageSize = 3; // the number of posts shown per page.
           // int pagenumber = (page?? 1); // if no posts set default page as page 1.

          //  var qpost = db.Posts.OrderByDescending(c => c.Id).ToPagedList(pagenumber, pageSize);
            return View(db.Posts.ToList());
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.FirstOrDefault(p=> p.Slug ==slug);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Updated,Title,Body,MediaURL")] BlogPost blogPost, HttpPostedFileBase image)
        {

            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var filePath = "/Uploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    blogPost.MediaURL = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }


                var Slug = StringUtil.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title.");
                        return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The Title must be unique");
                    return View(blogPost);
                }
                blogPost.Created = DateTime.Now;
                blogPost.Slug = Slug;
                db.Posts.Add(blogPost);
                db.SaveChanges();
                var currentSlug = db.Posts.Find(blogPost.Id).Slug;
                return RedirectToAction("Details", "BlogPosts",new { slug = currentSlug });
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Title,Body,MediaURL,Id")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                {
                    ModelState.AddModelError("image", "Invalid Format");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var filePath = "/Uploads/";
                    var absPath = Server.MapPath("~" + filePath);
                    blogPost.MediaURL = filePath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }
                if (!((StringUtil.URLFriendly(blogPost.Title) == blogPost.Slug)))
                {
                    blogPost.Slug = StringUtil.URLFriendly(blogPost.Title);
                }
                 blogPost.Updated = DateTime.Now;
                //db.Entry(blogPost).State = EntityState.Modified;
                db.Posts.Attach(blogPost);
                db.Entry(blogPost).Property("Title").IsModified = true;
                db.Entry(blogPost).Property("Body").IsModified = true;
                db.Entry(blogPost).Property("Slug").IsModified = true;
                db.Entry(blogPost).Property("MediaURL").IsModified = true;
                db.Entry(blogPost).Property("Updated").IsModified = true;
                db.Entry(blogPost).Property("Created").IsModified = false;
                //db.Entry(blogPost).Property("Id").IsModified = false;
                //db.Entry(blogPost).Property("Published").IsModified = false;
                db.SaveChanges();
                //var currentSlug = db.Posts.Find(blogPosts.Id).Slug;
                
                return RedirectToAction("Details", "BlogPosts", new { slug = blogPost.Slug });
                // return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
