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
            int blogIdToAddComment = Int32.Parse(commentData["blogId"]);

            try
            {
                //get the directory of blog posts JSON file
                var mapPath = Server.MapPath("~/App_Data/Blog-Posts.json");
                //json file as string
                var JsonString = System.IO.File.ReadAllText(mapPath);
                //json string --> JObject
                JObject jsonObj = JObject.Parse(JsonString);
                //loop through blog posts
                foreach (JObject blogPost in jsonObj["blogPosts"])
                {
                    //get blog post id
                    int blogPostId = blogPost["id"].ToObject<int>();
                    //if blogId in json and blog post id are equal
                    if (blogPostId == blogIdToAddComment)
                    {
                        if ((JArray)blogPost["comments"] == null)
                        {
                            System.Diagnostics.Debug.WriteLine("no comments yet");
                            //create new comment object
                            Comment newComment = new Comment(name, date, emailAddress, message);
                            //create array with the new comment as an element
                            Comment[] comments = new Comment[] { newComment };
                            System.Diagnostics.Debug.WriteLine("new comment: " + newComment.message);
                            //add newComment to the comment array
                            blogPost.Property("htmlContent").AddAfterSelf(new JProperty("comments", JToken.FromObject(comments)));
                            System.Diagnostics.Debug.WriteLine(blogPost.ToString());
                        }
                        else
                        {
                            //get comments array from json for that blog post
                            JArray comments = (JArray)blogPost["comments"];
                            //create new comment object
                            Comment newComment = new Comment(name, date, emailAddress, message);
                            System.Diagnostics.Debug.WriteLine("new comment: " + newComment.message);
                            //add newComment to the comment array
                            comments.Add(JToken.FromObject(newComment));

                            System.Diagnostics.Debug.WriteLine("comments: " + comments.ToString());
                        }

                    }

                }

                System.IO.File.WriteAllText(mapPath, jsonObj.ToString());

                //redirect to the currently viewed blog post
                return RedirectToAction("BlogContent", new { id = commentData["blogId"] });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("error occured" + e);
            }
            return View();
        }

        internal class Comment
        {
            public String name;
            public DateTime date;
            public String emailAddress;
            public String message;

            public Comment(String usersName, DateTime currentDate, String usersEmailAddress, String usersMessage)
            {
                name = usersName;
                date = currentDate;
                emailAddress = usersEmailAddress;
                message = usersMessage;
            }
        }
    }
}