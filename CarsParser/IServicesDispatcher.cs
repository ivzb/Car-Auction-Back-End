namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;

    public interface IServicesDispatcher
    {
        void AddService<T>(IDefaultService<T> service) where T : BaseModel, new();
        T GetEntity<T>(string key) where T : BaseModel, new();
    }
}