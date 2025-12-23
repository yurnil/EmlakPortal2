using EmlakPortal2.Data;
using EmlakPortal2.Repositories.Abstract;

namespace EmlakPortal2.Repositories.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository Category { get; private set; }
        public IPropertyRepository Property { get; private set; }
        public IPropertyImageRepository PropertyImage { get; private set; } 

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Property = new PropertyRepository(_context);
            PropertyImage = new PropertyImageRepository(_context); 
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}