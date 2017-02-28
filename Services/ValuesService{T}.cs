namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class ValuesService<T> : BaseService<T>, IValuesService<T>
        where T : ValueModel
    {
        public ValuesService(IDbRepository<T> repository)
            : base(repository)
        {
        }

        public override IDictionary<string, T> GetEntitiesAsDictionary()
        {
            IDictionary<string, T> entities = (IDictionary<string, T>)this.GetAll().ToDictionary(x => x.Value);
            return entities;
        }
    }
}