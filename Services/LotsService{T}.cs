namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class LotsService<T> : BaseService<T>, ILotsService<T>
        where T : LotModel
    {
        public LotsService(IDbRepository<T> repository)
            : base(repository)
        {
        }

        public override IDictionary<string, T> GetEntitiesAsDictionary()
        {
            IDictionary<string, T> entities = (IDictionary<string, T>)this.GetAll().ToDictionary(x => x.Lot);
            return entities;
        }
    }
}