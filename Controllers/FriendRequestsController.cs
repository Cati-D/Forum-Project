using Anspeamiaincercareplusunu.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Anspeamiaincercareplusunu.Controllers
{
    public class FriendRequestsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: FriendRequest
        public ActionResult Index()
        {
            var friendRequest = db.FriendRequests.ToList();
            ViewBag.FriendRequests = friendRequest;

            return View();
        }

        public ActionResult Show(int id)
        {
            FriendRequest friendRequest = db.FriendRequests.Find(id);
            if (friendRequest.ReceiverId == User.Identity.GetUserId())
            {
                return View(friendRequest);
            }
            return Redirect("/");
        }

        public ActionResult New()
        {
            ViewBag.FriendRequests = db.FriendRequests;
            return View();
        }
        [HttpGet]
        public ActionResult AddFriend()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFriend(FriendRequest t)
        {
            if (ModelState.IsValid)
            {
                var user = User.Identity.GetUserId();
                var frReq = db.FriendRequests.Where(x =>
                (
                (user == x.ReceiverId && t.ReceiverId == x.SenderId ) || (user == x.SenderId  && t.ReceiverId == x.ReceiverId)
                )).ToList();
                if(frReq != null)
                {
                    if(frReq.Count > 0)
                    {
                        if(frReq[0].FriendRequestStatus == FriendRequestStatus.Blocked || frReq[0].FriendRequestStatus == FriendRequestStatus.Declined || frReq[0].FriendRequestStatus == FriendRequestStatus.Accepted)
                        {
                            return View();
                        }
                        else if(frReq[0].FriendRequestStatus == FriendRequestStatus.Pending)
                        {
                            frReq[0].FriendRequestStatus = t.FriendRequestStatus;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        FriendRequest fr = new FriendRequest();
                        fr.ReceiverId = t.ReceiverId;
                        fr.SenderId = User.Identity.GetUserId();
                        fr.FriendRequestStatus = t.FriendRequestStatus;
                        db.FriendRequests.Add(fr);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                        //adaugam
                    }
                }
            }
            return View(t);
        }

    }
}