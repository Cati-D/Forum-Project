using Anspeamiaincercareplusunu.Models;
using Microsoft.AspNet.Identity;
using System;
using Microsoft.Security.Application;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anspeamiaincercareplusunu.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private int _perPage = 3;

        // GET: Posts
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Index()
        {
            var posts = db.Posts.Include("Forum").Include("User").OrderBy(a => a.Created);
            var totalItems = posts.Count();
            var currentPage = Convert.ToInt32(Request.Params.Get("page"));

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * this._perPage;
            }

            var paginatedPosts = posts.Skip(offset).Take(this._perPage);

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            ViewBag.total = totalItems;
            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)this._perPage);
            ViewBag.Posts = paginatedPosts;

            return View();
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Post post = db.Posts.Find(id);

            ViewBag.afisareButoane = false;
            if (User.IsInRole("Editor") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = "User, Editor, Admin")]
        public ActionResult Show(PostReply postReply)
        {
            postReply.Created = DateTime.Now;
            postReply.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.PostReplies.Add(postReply);
                    db.SaveChanges();
                    return Redirect("/Posts/Show/" + postReply.Id);
                }
                else
                {
                    Post a = db.Posts.Find(postReply.Id);
                    SetAccessRights();
                    return View(a);
                }
            }
            catch (Exception e)
            {
                Post a = db.Posts.Find(postReply.Id);
                SetAccessRights();
                return View(a);
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        public ActionResult New()
        {
            Post post = new Post();

            post.Form = GetAllForum();

            post.UserId = User.Identity.GetUserId();

            return View(post);
        }


        [HttpPost]
        [Authorize(Roles = "Editor,Admin")]
        [ValidateInput(false)]
        public ActionResult New(Post post)
        {
            post.Created = DateTime.Now;
            post.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    post.Content = Sanitizer.GetSafeHtmlFragment(post.Content);

                    db.Posts.Add(post);
                    db.SaveChanges();
                    TempData["message"] = "Postarea a fost adaugata";
                    return RedirectToAction("Index");
                }
                else
                {
                    post.Form = GetAllForum();
                    return View(post);
                }
            } catch (Exception e)
            {
                post.Form = GetAllForum();
                return View(post);
            }
        }

        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Post post = db.Posts.Find(id);
            post.Form = GetAllForum();

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(post);
            }
            else
            {
                TempData["message"] = "Nu ai voie sa editezi daca nu este postarea ta.";
                return RedirectToAction("Index");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Editor,Admin")]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Post requestPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Post post = db.Posts.Find(id);
                    if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
                        if (TryUpdateModel(post))
                        {
                            post.Title = requestPost.Title;

                            requestPost.Content = Sanitizer.GetSafeHtmlFragment(requestPost.Content);

                            post.Content = requestPost.Content;
                            post.ForumId = requestPost.ForumId;
                            db.SaveChanges();
                            TempData["message"] = "Postarea a fost modificata!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei postari care nu va apartine";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestPost.Form = GetAllForum();
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                requestPost.Form = GetAllForum();
                return View(requestPost);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Post post = db.Posts.Find(id);

            if (post.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Posts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost stearsa";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o postare care nu va apartine!";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllForum()
        {
            var selectList = new List<SelectListItem>();

            var forums = from frm in db.Forums
                         select frm;

            foreach (var forum in forums)
            {
                selectList.Add(new SelectListItem
                {
                    Value = forum.Id.ToString(),
                    Text = forum.Title.ToString()
                });
            }
            return selectList;
        }
        private void SetAccessRights()
        {
           
        }
    }
}