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
            var blogID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);
            //get the directory of blog posts JSON file
            var mappath = Server.MapPath("~/App_Data/Blog-Posts.json");
            //json file as string
            var myJsonString = System.IO.File.ReadAllText(mappath);
            //string to object
            JObject blogPosts = JObject.Parse(myJsonString);
         
            foreach (JObject post in blogPosts["blogPosts"])
            {
                //convert JValue to integer
                int postId = post["id"].ToObject<int>();

                if(postId == blogID)
                {
                    System.Diagnostics.Debug.WriteLine("blog title: " + post["title"]);
                }
            }

            

            JArray jsonArray = (JArray)blogPosts["blogPosts"];

            


            //foreach (JObject post in jsonArray)
            //{
            //    //System.Diagnostics.Debug.WriteLine("blog:");

            //    foreach (var details in post.Properties())
            //    {
            //        System.Diagnostics.Debug.WriteLine(details["id"]);
            //        //if (details == blogID)
            //        //{
            //        //    System.Diagnostics.Debug.WriteLine(details.Name + ":" + details.Value);
            //        //}

            //    }
            //}

            //System.Diagnostics.Debug.WriteLine("blog id: " + details);

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
