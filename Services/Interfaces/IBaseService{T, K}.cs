namespace Services.Interfaces
{
    using System.Linq;

    /// <typeparam name="T">Type of the Model</typeparam>
    /// <typeparam name="K">Type of the Key</typeparam>
   public interface IBaseService<T, K>
       where K : struct
    {
       IQueryable<T> Get();
       T Get(K id);
       void Add(T entity);
       void Update(T entity);
       void Delete(T entity);
    }
}