namespace Services.Interfaces
{
    using Data.Common.Models;
    using System.Collections.Generic;

    public interface ILotsService<T> : IBaseService<T>
        where T : LotModel
    {
        IDictionary<string, T> GetEntitiesAsDictionary();
    }
}