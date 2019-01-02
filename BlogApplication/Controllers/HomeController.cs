using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogApplication.Models;
using BlogApplication.Data;
using BlogApplication.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using BlogApplication.Models.Comment;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        /// <summary>
        /// This is the constructor of repository 
        /// </summary>
        public HomeController(IRepository repository)
        {
            _repository = repository;
            var comment = new MainComment();
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// This action method uses the get all post from repository , and display all the post in view Post
        /// </summary>
        /// <returns>A view with List of posts</returns>        
        public IActionResult Post()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        /// <summary>
        /// This action method provide a view PanelTest to List all the post and giving a option to edit and remove
        /// Admin only 
        /// </summary>
        /// <returns>Post action method</returns>
        //[Authorize(Policy = "Admin")]
        public IActionResult PanelTest()
        {           
            return Post();
        }

        /// <summary>
        /// This action method check if the postid is valid and gives a view EditPost
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns>if the Post id is nothing then redirect the page to the Post page, otherwise pass the id</returns>
        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Post");
            }
            else
            {
                var post = _repository.GetPost(id);
                return View(post);
            }
        }

        /// <summary>
        /// This action method check if the postid match and update the post and save changes
        /// Admin only
        /// </summary>
        /// <param name="id">PostId</param>
        /// <param name="post">The post</param>
        /// <returns>if the the post id doesnt match redirect to post page , otherwise update post save changes and redirect to post page </returns>
        [HttpPost]
        //[Authorize(Policy = "Admin")]
        public async Task<IActionResult> EditPost(int id, Post post)
        {
            if (id != post.PostId)
            {
                return RedirectToAction("Post");
            }
            else
            {   
                //post.Author = User.Identity.Name;
                _repository.UpdatePost(post);
                await _repository.SaveChangesAsync();
                return RedirectToAction("Post");                       
            }
        }

        /// <summary>
        /// This action method gives a view CreatePost
        /// </summary>
        /// <returns>new post</returns>
        [HttpGet]
        public IActionResult CreatePost()
        {
           return View(new Post());
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel cvm)
        {
            if (ModelState.IsValid)
                return RedirectToAction("Post", new { id = cvm.PostId });

            var post = _repository.GetPost(cvm.PostId);

            if(cvm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = cvm.Message,
                    CreatedTime = DateTime.Now,

                });

                _repository.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = cvm.MainCommentId,
                    Message = cvm.Message,
                    CreatedTime = DateTime.Now,
                };

            }

            await _repository.SaveChangesAsync();


            return View();
        }

        /// <summary>
        /// This action method create a post also store the author which is username
        /// Admin only
        /// </summary>
        /// <param name="post">The post</param>
        /// <returns>Redirect to Post page</returns>
        [HttpPost]
        //[Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreatePost(Post post)
        {
            post.Author = User.Identity.Name;
            _repository.CreatePost(post);
            await _repository.SaveChangesAsync();
            return RedirectToAction("Post");
        }

        /// <summary>
        /// This action method remove the post
        /// Admin only
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="post">The post</param>
        /// <returns></returns>
        //[Authorize(Policy = "Admin")]
        public async Task<IActionResult> RemovePost(int? id , Post post)
        {
            post = _repository.GetPost(id);
            _repository.RemovePost(post);
            await _repository.SaveChangesAsync();
            return RedirectToAction("PanelTest");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
