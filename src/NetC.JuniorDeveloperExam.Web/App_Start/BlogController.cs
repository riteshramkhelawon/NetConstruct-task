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
                if(blogId == urlBlogID)
                {
                    //get details from appropriate blog entry [MAKE THESE INTO A SINGLE OBJECT]
                    ViewBag.date = blog["date"];
                    ViewBag.title = blog["title"];
                    ViewBag.image = blog["image"];
                    ViewBag.htmlContent = blog["htmlContent"];
                    ViewBag.comments = blog["comments"];
                    

                    System.Diagnostics.Debug.WriteLine("\nblog title: " + blog["title"] +
                   "\ndate: " + blog["date"] +
                   "\nimage: " + blog["image"] +
                   "\ncontent: " + blog["htmlContent"] + 
                   "\ncomments: " + blog["comments"]);

                    break;
                }
            }

            return View();
        }

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
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
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
