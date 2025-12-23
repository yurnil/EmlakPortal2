using EmlakPortal2.Models;

namespace EmlakPortal2.Repositories.Abstract
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        void Save();
    }
}