namespace Services.Interfaces
{
    using Data.Common.Models;
    using System.Collections.Generic;

    public interface IBaseService<T> : IBaseService<T, int>
        where T : GenericModel<int>
    {
        IDictionary<string, T> GetEntitiesAsDictionary();
    }
}