namespace Services.Interfaces
{
    using Data.Common.Models;
    using System.Collections.Generic;

    public interface ITitlesService<T> : IBaseService<T>
        where T : TitleModel
    {
    }
}