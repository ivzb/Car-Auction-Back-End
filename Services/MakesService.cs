namespace Services
{
    using Data;
    using Data.Common;
    using Services.Interfaces;

    public class MakesService : DefaultService<Make>, IMakesService
    {
        public MakesService(IDbRepository<Make> repository)
            : base(repository)
        {
        }
    }
}