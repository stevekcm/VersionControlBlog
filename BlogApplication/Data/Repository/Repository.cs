using BlogApplication.Models;
using BlogApplication.Models.Comment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BlogApplication.Data.Repository
{
    public class Repository : IRepository
    {
        private ApplicationDbContext _context;

        /// <summary>
        /// the context constructor
        /// </summary>
        /// <param name="context"></param>
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method create the post in database
        /// </summary>
        /// <param name="post">Post</param>
        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
        }

        /// <summary>
        /// This method get the post by id, also loaded up the comments
        /// </summary>
        /// <param name="PostId"> Post Id</param>
        /// <returns>A post with information</returns>
        public Post GetPost(int? PostId)
        {
            return _context.Posts
                .Include(m => m.MainComments)
                .ThenInclude(m => m.SubComments)
                .FirstOrDefault(m => m.PostId == PostId);
        }

        /// <summary>
        /// This method remove a post from database
        /// </summary>
        /// <param name="post">Post</param>
        public void RemovePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        /// <summary>
        /// This method update the post from database
        /// </summary>
        /// <param name="post">Post</param>
        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }

        /// <summary>
        /// This method save it and update the database
        /// </summary>
        /// <returns>Update Database</returns>
        public async Task<bool> SaveChangesAsync()
        {
            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method get all the post from the list
        /// </summary>
        /// <returns>Posts</returns>
        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        /// <summary>
        /// This method add subcomment below follow by the main comment
        /// </summary>
        /// <param name="comment">Comment</param>
        public void AddSubComment(SubComment comment)
        {
            _context.SubComments.Add(comment);

        }
    }
}
