using EmlakPortal2.Models;

namespace EmlakPortal2.Repositories.Abstract
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Özel bir metod gerekirse buraya eklenir
        void Save();
    }
}