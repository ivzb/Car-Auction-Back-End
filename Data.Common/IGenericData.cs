namespace Data.Common
{
    using Data.Common.Models;

    public interface IGenericData
    {
        IDbRepository<T> GetRepository<T>() where T : GenericModel<int>;
        int SaveChanges();
    }
}