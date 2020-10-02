using _V_Semestr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Data.Repository
{
    public class PostRepository : IPostRepository
    {
        private AppDbContext _ctx;
        public PostRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post); 
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }
        public void UpdatePost(Post post)
        {
            int count = _ctx.ChangeTracker.Entries().Count();
            System.Diagnostics.Debug.WriteLine($"{count}");
            _ctx.Posts.Update(post);
        }
        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Post> GetAllPosts(string category)
        {
            Func<Post, bool> InCategory = (p) => { return p.Category.Name.ToLower().Equals(category.ToLower()); };

            return _ctx.Posts
                .Where(p => InCategory(p))
                .ToList();
        }
    }
}
