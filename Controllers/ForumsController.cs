using Anspeamiaincercareplusunu.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anspeamiaincercareplusunu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ForumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Forums
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            } 

            var forums = db.Forums;
            ViewBag.Forums = forums;
            return View();
        }
        public ActionResult New()
        {
            Forum forum = new Forum();
            return View(forum);
        }
        [HttpPost]
        public ActionResult New(Forum forum)
        {
            forum.Created = DateTime.Now;
            try
            {
                if(ModelState.IsValid)
                {
                    var currentUserId = User.Identity.GetUserId();
                    forum.UserId = currentUserId;
                    
                    /*if (db.Forums.Find(forum.Title) != null)
                    {
                        TempData["message"] = "Forumul se gaseste deja, adauga postarea intr-unul din Forumurile existente!";
                        return Redirect("/Forums/New/");
                    }
                    else
                    {*/
                        db.Forums.Add(forum);
                        db.SaveChanges();
                        TempData["message"] = "Forumul a fost adaugat!";
                        return Redirect("/Forums/Index/");
                    //}

                  
                }
                {
                    return View(forum);
                }
            }
            catch(Exception e)
            {
                return View(forum);
            }
        }

        public ActionResult Show(int id)
        {
            Forum forum = db.Forums.Find(id);
            ViewBag.Forums = forum;
            return View(forum);
        }

        public ActionResult Edit(int id)
        {
            Forum forum = db.Forums.Find(id);
            return View(forum);
        }

        [HttpPut]
        public ActionResult Edit(int id, Forum requestForum)
        {
            try
            {
                Forum forum = db.Forums.Find(id);

                if (TryUpdateModel(forum))
                {
                    forum.Title = requestForum.Title;
                    db.SaveChanges();
                    TempData["message"] = "Forumul a fost modificat!";
                    return Redirect("/Forums/Index/");
                }
                return View(requestForum);
            }
            catch (Exception e)
            {
                return View(requestForum);
            }
        }

        /*[HttpDelete]
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Forum forum = db.Forums.Find(id);

            if (forum.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Forums.Remove(forum);
                TempData["message"] = "Forumul a fost sters!";
                db.SaveChanges();
                return Redirect("/Forums/Index/");
            }
            TempData["message"] = "Nu poti sterge un articol ca nu-i al tau!";
            return Redirect("/Forums/Index/");
        }*/

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Forum forum = db.Forums.Find(id);
            db.Forums.Remove(forum);
            TempData["message"] = "Forumul a fost sters!";
            db.SaveChanges();
            return Redirect("/Forums/Index/");

        }
    }
}