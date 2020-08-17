using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace NetC.JuniorDeveloperExam.Web.App_Start
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult BlogContent()
        {
            //get the blog id from the url (in integer format)
            //check if id parameter in url is an integer
            //if so, convert to int
            //if not, use blog id 1 as default blog id
            var urlBlogID = (int.TryParse(Url.RequestContext.RouteData.Values["id"].ToString(), out int isUrlBlogIdNumeric) != false) ? isUrlBlogIdNumeric : 1;

            //get blog post details from JSON file as a JObject
            JObject blogPosts = getBlogPostJsonObject();

            //loop through each object in the blogPosts JObject
            foreach (JObject blog in blogPosts["blogPosts"])
            {
                //convert current blogId(JToken) to an integer
                int currentBlogId = convertIdToInteger(blog["id"]);

                //if the id in the url matches the current blogId in the loop
                if (urlBlogID == currentBlogId)
                {
                    //get details from appropriate blog entry and add these to the ViewBag for use in the view
                    ViewBag.blogID = urlBlogID;
                    ViewBag.date = blog["date"];
                    ViewBag.title = blog["title"];
                    ViewBag.image = blog["image"];
                    ViewBag.htmlContent = blog["htmlContent"];
                    ViewBag.comments = blog["comments"];
                    ViewBag.blogFound = true;
                    break;
                }
                else
                {
                    ViewBag.blogFound = false;
                }
            }
            return View();
        }

        // POST: Blog/AddComment
        [HttpPost]
        public ActionResult AddComment(FormCollection commentData)
        {
            //get user inputs from the comment form
            String name = commentData["name"];
            DateTime date = DateTime.Now;
            String emailAddress = commentData["emailAddress"];
            String message = commentData["message"];
            int blogIdToAddComment = Int32.Parse(commentData["blogId"]);

            try
            {
                //get blog post details from JSON file as a JObject
                JObject blogPosts = getBlogPostJsonObject();

                //loop through each object in the blogPosts JObject
                foreach (JObject blog in blogPosts["blogPosts"])
                {
                    //get the current blog post id(JToken) as an integer
                    int currentBlogPostId = convertIdToInteger(blog["id"]);

                    //if the blogId to add the comment to is equal to the current blogId in the loop
                    if (blogIdToAddComment == currentBlogPostId)
                    {
                        //get comments array for that blog post
                        JArray comments = (JArray)blog["comments"];

                        //generate new comment id
                        int newCommentId = comments != null ? comments.Count + 1 : 1;

                        //create new comment object with comment details
                        Comment newComment = new Comment(newCommentId, name, date, emailAddress, message);

                        //if there are already existing comments for this blog post
                        if (comments != null)
                        {
                            //add the new comment to the exiting comment array
                            comments.Add(JToken.FromObject(newComment));
                        }
                        //if there are no existing comments for this blog post
                        else
                        {
                            //create an array with the new comment as an element
                            Comment[] commentsArray = new Comment[] { newComment };

                            //add the new comment array as a property of the current blog(JObject)
                            blog.Property("htmlContent").AddAfterSelf(new JProperty("comments", JToken.FromObject(commentsArray)));
                        }

                        break;
                    }

                }

                //overwrite blog posts JSON file with the new JSON object
                saveJsonFile(blogPosts);

                //redirect to the currently viewed blog post
                return RedirectToAction("BlogContent", new { id = commentData["blogId"] });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("error occured" + e);
                return View();
            }
        }

        public ActionResult addCommentReply(FormCollection replyData)
        {
            //get user inputs from the reply form
            String name = replyData["replyName"];
            DateTime date = DateTime.Now;
            String emailAddress = replyData["replyEmailAddress"];
            String message = replyData["replyMessage"];
            int blogIdToAddReply = Int32.Parse(replyData["blogId"]);
            int commentId = Int32.Parse(replyData["commentId"]);

            //create new comment object for the reply
            Comment reply = new Comment(1, name, date, emailAddress, message);

            //get blog post details from JSON file as a JObject
            JObject blogPosts = getBlogPostJsonObject();

            //loop through blogPosts to find the appropriate blog
            foreach(JObject blog in blogPosts["blogPosts"])
            {
                //get the current blog post id(JToken) as an integer
                int currentBlogPostId = convertIdToInteger(blog["id"]);

                //if the blog id to add the reply to, is the same as the current blogId in the loop
                if (blogIdToAddReply == currentBlogPostId)
                {
                    //get comments array for that blog post
                    JArray comments = (JArray)blog["comments"];

                    //loop through each comment object in the comments array
                    foreach (JObject comment in comments)
                    {
                        //get the current blog post id(JToken) as an integer
                        int currentCommentId = convertIdToInteger(comment["id"]);
                        
                        //if the comment id to add the reply to is the same as the current comment id in the loop
                        if (commentId == currentCommentId)
                        {
                            //create new property 'reply' in the comment object and add the reply as it's value
                            comment.Property("message").AddAfterSelf(new JProperty("reply", JToken.FromObject(reply)));
                        }
                    }
                }
            }

            //overwrite blog posts JSON file with the new JSON object
            saveJsonFile(blogPosts);

            //redirect to the currently viewed blog post
            return RedirectToAction("BlogContent", new { id = replyData["blogId"] });
        }

        [HttpPost]
        public Boolean checkEmail()
        {
            //get email address input from the user
            var emailAddress = Request["emailAddress"];
            
            Boolean isValidEmail = true;

            //check the email address against the regex to see if it matches the correct format
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailAddress);

            if (!match.Success)
            {
                //if the email address is not in the correct format
                isValidEmail = false;
            }

            return isValidEmail;
        }

        public JObject getBlogPostJsonObject()
        {
            //get the path of blog posts JSON file
            var mapPath = Server.MapPath("~/App_Data/Blog-Posts.json");

            //get JSON file as a string
            var JsonString = System.IO.File.ReadAllText(mapPath);

            //Parse string as JObject
            JObject blogPosts = JObject.Parse(JsonString);

            return blogPosts;
        }

        public void saveJsonFile(JObject newJsonObject)
        {
            //get the path of blog posts JSON file
            var mapPath = Server.MapPath("~/App_Data/Blog-Posts.json");

            //Write new JSON object to the blogPosts Json file
            System.IO.File.WriteAllText(mapPath, newJsonObject.ToString());
        }

        public int convertIdToInteger(JToken id)
        {
            //convert JToken to integer
            int integerId = id.ToObject<int>();

            return integerId;
        }


        internal class Comment
        {
            public int id;
            public String name;
            public DateTime date;
            public String emailAddress;
            public String message;

            public Comment(int newId, String usersName, DateTime currentDate, String usersEmailAddress, String usersMessage)
            {
                id = newId;
                name = usersName;
                date = currentDate;
                emailAddress = usersEmailAddress;
                message = usersMessage;
            }
        }
    }
}