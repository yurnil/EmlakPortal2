namespace EmlakPortal2.Repositories.Abstract
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IPropertyRepository Property { get; }
        IPropertyImageRepository PropertyImage { get; }
        void Save();
    }
}