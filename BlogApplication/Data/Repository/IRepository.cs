using BlogApplication.Models;
using BlogApplication.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int? PostId);
        List<Post> GetAllPosts();
        void CreatePost(Post post);
        void RemovePost(Post post);
        void UpdatePost(Post post);

        void AddSubComment(SubComment comment);

        Task<bool> SaveChangesAsync();

    }
}
