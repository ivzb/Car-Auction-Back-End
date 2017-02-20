namespace Services.Interfaces
{
    using Data.Common.Models;
    using System.Collections.Generic;

    public interface IUrlsService<T> : IBaseService<T>
        where T : UrlModel
    {
    }
}