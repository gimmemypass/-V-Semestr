using _V_Semestr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Data.Repository
{
    public interface ICategoryRepository
    {
        Category GetCategory(int id);
        List<Category> GetAllCategories();
        void AddCategory(Category cat);
        void UpdateCategory(Category cat);
        void RemoveCategory(int id);

        Task<bool> SaveChangesAsync();

    }
}
