using EmlakPortal2.Data;
using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;

namespace EmlakPortal2.Repositories.Concrete
{
    public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyImageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(PropertyImage obj)
        {
            _context.PropertyImages.Update(obj);
        }
    }
}