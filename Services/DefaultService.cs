namespace Services
{
    using Data.Common;
    using Data.Common.Models;
    using Services.Interfaces;

    public class DefaultService<T> : BaseService<T, int>, IDefaultService<T>
        where T : BaseModel<int>
    {
        public DefaultService(IDbRepository<T> repository)
            : base(repository)
        {
        }
    }
}