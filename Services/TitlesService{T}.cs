namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class TitlesService<T> : BaseService<T>, ITitlesService<T>
        where T : TitleModel
    {
        public TitlesService(IDbRepository<T> repository)
            : base(repository)
        {
        }

        public override IDictionary<string, T> GetEntitiesAsDictionary()
        {
            IDictionary<string, T> entities = (IDictionary<string, T>)this.Get().ToDictionary(x => x.Title);
            return entities;
        }
    }
}