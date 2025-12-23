using EmlakPortal2.Models;

namespace EmlakPortal2.Repositories.Abstract
{
    public interface IPropertyImageRepository : IGenericRepository<PropertyImage>
    {
        void Update(PropertyImage obj);
    }
}