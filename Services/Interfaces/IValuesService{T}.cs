namespace Services.Interfaces
{
    using Data.Common.Models;
    using System.Collections.Generic;

    public interface IValuesService<T> : IBaseService<T>
        where T : ValueModel
    {
        IDictionary<string, T> GetEntitiesAsDictionary();
    }
}