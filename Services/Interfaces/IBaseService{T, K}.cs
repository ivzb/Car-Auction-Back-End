namespace Services.Interfaces
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <typeparam name="T">Type of the Model</typeparam>
    /// <typeparam name="K">Type of the Key</typeparam>
   public interface IBaseService<T, K>
       where K : struct
    {
       T Get(K id);
       T Get(Expression<Func<T, bool>> selector);
       IQueryable<T> GetAll(Expression<Func<T, bool>> selector = null);
       void Add(T entity);
       void Update(T entity);
       void Delete(T entity);
    }
}