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

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
        }

        public Post GetPost(int? PostId)
        {
            return _context.Posts
                .Include(m => m.MainComments)
                .ThenInclude(m => m.SubComments)
                .FirstOrDefault(m => m.PostId == PostId);
        }

        public void RemovePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public void AddSubComment(SubComment comment)
        {
            _context.SubComments.Add(comment);

        }
    }
}
