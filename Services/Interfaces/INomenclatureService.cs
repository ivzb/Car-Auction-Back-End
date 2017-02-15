namespace Services.Interfaces
{
    using Data.Common.Models;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface INomenclatureService
    {
        IQueryable<T> GetAll<T>()
            where T : BaseModel<int>;
        IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate)
            where T : BaseModel<int>;
    }
}