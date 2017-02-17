namespace Data.Common
{
    using Models;
    using System.Linq;

    public interface IDbRepository<T> : IDbRepository<T, int>
         where T : BaseModel
    {
    }

    public interface IDbRepository<T, in TKey>
        where T : BaseModel
    {
        IQueryable<T> All();
        T GetById(TKey id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Attach(T entity);
        void AttachModel<M>(M entity) where M : class;
        int Save();
    }
}