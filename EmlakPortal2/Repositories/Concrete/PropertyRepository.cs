using EmlakPortal2.Data;
using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;

namespace EmlakPortal2.Repositories.Concrete
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}