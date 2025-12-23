using System.Linq.Expressions;

namespace EmlakPortal2.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        // Tüm veriyi getir (filtre verilebilir)
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        // ID'ye göre tek veri getir
        T GetById(int id);

        // Ekleme, Güncelleme, Silme
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}