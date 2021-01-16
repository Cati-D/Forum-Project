using Anspeamiaincercareplusunu.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anspeamiaincercareplusunu.Controllers
{
    public class PostRepliesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PostReplies
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Delete(int id)
        {
            PostReply postReply = db.PostReplies.Find(id);
            if (postReply.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.PostReplies.Remove(postReply);
                db.SaveChanges();
                return Redirect("/Posts/Show/" + postReply.PostId);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Posts");
            }
        }

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            PostReply postReply = db.PostReplies.Find(id);

            if (postReply.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(postReply);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Posts");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, PostReply requestPostReply)
        {
            try
            {
                PostReply postReply = db.PostReplies.Find(id);

                if (postReply.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(postReply))
                    {
                        postReply.Content = requestPostReply.Content;
                        db.SaveChanges();
                    }
                    return Redirect("/Posts/Show/" + postReply.PostId);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                    return RedirectToAction("Index", "Posts");
                }

            }
            catch (Exception e)
            {
                return View(requestPostReply);
            }
        }


    }
}