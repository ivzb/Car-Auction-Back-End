namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;

    public class BaseService<T> : BaseService<T, int>, IBaseService<T>
        where T : GenericModel<int>
    {
        public BaseService(IDbRepository<T> repository)
            : base(repository)
        {
        }

        public virtual IDictionary<string, T> GetEntitiesAsDictionary()
        {
            throw new NotImplementedException();
        }
    }
}