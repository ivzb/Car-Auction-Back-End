namespace Services.Interfaces
{
    using Data.Common.Models;

    public interface IDefaultService<T> : IBaseService<T, int>
        where T : BaseModel<int>
    {
    }
}