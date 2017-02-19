namespace Services.Interfaces
{
    using Data.Common.Models;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IGenericService
    {
        IQueryable<T> GetAll<T>()
            where T : GenericModel<int>;
        IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate)
            where T : GenericModel<int>;
    }
}