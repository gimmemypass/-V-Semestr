using _V_Semestr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private AppDbContext _ctx;
        public CategoryRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public void AddCategory(Category cat)
        {
            _ctx.Categories.Add(cat);
        }

        public List<Category> GetAllCategories()
        {
            var list = _ctx.Categories.ToList();
            //            return _ctx.Categories.ToList();
            return list;
        }

        public Category GetCategory(int id)
        {
            var cat = _ctx.Categories
                .Include(c => c.Posts)
                .FirstOrDefault(c => c.Id == id);
            int count = _ctx.ChangeTracker.Entries().Count();
            System.Diagnostics.Debug.WriteLine($"{count}");
            return cat;
        }

        public void RemoveCategory(int id)
        {
            _ctx.Categories.Remove(GetCategory(id));
        }

        public async Task<bool> SaveChangesAsync()
        {
            if(await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void UpdateCategory(Category cat)
        {
            _ctx.Categories.Update(cat);
        }
    }
}
