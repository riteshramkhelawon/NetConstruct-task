using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetC.JuniorDeveloperExam.Web.App_Start
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult BlogContent()
        {
            System.Diagnostics.Debug.WriteLine("id val: " + Url.RequestContext.RouteData.Values["id"]);
            //get the blog id from the url (in 32-bit integer format)
            var urlBlogID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);
            //get the directory of blog posts JSON file
            var mapPath = Server.MapPath("~/App_Data/Blog-Posts.json");
            //json file as string
            var JsonString = System.IO.File.ReadAllText(mapPath);
            //string to object
            JObject blogPosts = JObject.Parse(JsonString);

            //loop through each object in the JSON Object
            foreach (JObject blog in blogPosts["blogPosts"])
            {
                //convert JValue to integer
                int blogId = blog["id"].ToObject<int>();

                //if id in the url matches the current blogId, get blog details
                if (blogId == urlBlogID)
                {
                    //get details from appropriate blog entry [MAKE THESE INTO A SINGLE OBJECT]
                    ViewBag.blogID = urlBlogID;
                    ViewBag.date = blog["date"];
                    ViewBag.title = blog["title"];
                    ViewBag.image = blog["image"];
                    ViewBag.htmlContent = blog["htmlContent"];
                    ViewBag.comments = blog["comments"];

                    break;
                }
            }
            
            return View();
        }

        //public ActionResult AddComment()
        //{
        //    System.Diagnostics.Debug.WriteLine("add comment action");


        //    return View("~/Views/BlogContent.cshtml");
        //}

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        public ActionResult AddComment(FormCollection commentData)
        {
            System.Diagnostics.Debug.WriteLine("add comment action");

            //get user inputs from the comment form
            String name = commentData["name"];
            DateTime date = DateTime.Now;
            String emailAddress = commentData["emailAddress"];
            String message = commentData["message"];

            try
            {
                System.Diagnostics.Debug.WriteLine("name: " + name);
                System.Diagnostics.Debug.WriteLine("date: " + date.ToString());
                System.Diagnostics.Debug.WriteLine("email: " + emailAddress);
                System.Diagnostics.Debug.WriteLine("msg: " + message);
                //redirect to the currently viewed blog post
                return RedirectToAction("BlogContent", new { id = commentData["blogId"] });


            }
            catch
            {
                return View();
            }
        }

        
        // GET: Blog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Blog/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Blog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Blog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
