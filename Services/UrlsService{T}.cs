namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class UrlsService<T> : BaseService<T>, IUrlsService<T>
        where T : UrlModel
    {
        public UrlsService(IDbRepository<T> repository)
            : base(repository)
        {
        }

        public override IDictionary<string, T> GetEntitiesAsDictionary()
        {
            IDictionary<string, T> entities = (IDictionary<string, T>)this.GetAll().ToDictionary(x => x.Url);
            return entities;
        }
    }
}