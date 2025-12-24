using EmlakPortal2.Data;
using EmlakPortal2.Models;
using EmlakPortal2.Repositories.Abstract;

namespace EmlakPortal2.Repositories.Concrete
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        private ApplicationDbContext _db;

        public PropertyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        // Update metodu GenericRepository'den geliyor, buraya tekrar yazmıyoruz.
        // Böylece çakışma olmuyor.
    }
}