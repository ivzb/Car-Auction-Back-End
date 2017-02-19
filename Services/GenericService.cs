namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class GenericService : IGenericService
    {
        private readonly IGenericData data;

        public GenericService(IGenericData data)
        {
            this.data = data;
        }

        public IQueryable<T> GetAll<T>()
            where T : GenericModel<int>
        {
            IDbRepository<T> repo = this.data.GetRepository<T>();
            return repo.All();
        }
        public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> func)
            where T : GenericModel<int>
        {
            IDbRepository<T> repo = this.data.GetRepository<T>();
            return repo.All().Where(func);
        }
    }
}