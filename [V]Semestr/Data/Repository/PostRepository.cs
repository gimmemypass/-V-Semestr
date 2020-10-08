using _V_Semestr.Models;
using _V_Semestr.Models.Comments;
using _V_Semestr.ViewModel;
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

        public IndexViewModel GetAllPosts(int pageNumber)
        {
            int pageSize = 5;
            int skip = pageSize * (pageNumber - 1);
            return new IndexViewModel
            {
                NextPage =  _ctx.Posts.Count() > skip + pageSize,
                Posts = _ctx.Posts
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToList(),
                PageNumber = pageNumber
            };

        }
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.Category)
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
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

        public IndexViewModel GetAllPosts(int pageNumber, string category)
        {
            int pageSize = 5;
            int skip = pageSize * (pageNumber - 1);
            var query = _ctx.Posts.Include(p => p.Category).AsQueryable();
            var test = _ctx.Posts.Include(p => p.Category).AsQueryable().ToList();
            if (!String.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name.ToLower().Equals(category.ToLower()));
            }
            var posts = query
                        .Skip(pageSize * (pageNumber - 1))
                        .Take(pageSize)
                        .ToList();
            var postCount = query.Count();
            return new IndexViewModel
            {
                NextPage = postCount > skip + pageSize,
                PageCount = (int) Math.Ceiling((double)postCount / pageSize), 
                Posts = posts,
                PageNumber = pageNumber,
                Category = category
            };
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
