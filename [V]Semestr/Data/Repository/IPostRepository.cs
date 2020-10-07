using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _V_Semestr.Models;
using _V_Semestr.Models.Comments;

namespace _V_Semestr.Data.Repository
{
    public interface IPostRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(int pageNumber);
        List<Post> GetAllPosts(string category);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);
        void AddSubComment(SubComment comment);

        Task<bool> SaveChangesAsync();
    }
}
