using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _V_Semestr.Models;
using _V_Semestr.ViewModel;

namespace _V_Semestr.Data.Repository
{
    public interface IPostRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        //IndexViewModel GetAllPosts(int pageNumber);
        IndexViewModel GetAllPosts(int pageNumber, string category, string search);
        //List<Post> GetAllPosts(string category);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void RemovePost(int id);
        void AddComment(Comment comment);
        List<Comment> GetCommentsByPostId(int postId);

        Task<bool> SaveChangesAsync();
    }
}
