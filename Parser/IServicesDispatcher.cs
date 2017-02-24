namespace Parser
{
    using Data.Common.Models;
    using Services.Interfaces;

    public interface IServicesDispatcher
    {
        IServicesDispatcher InjectService<T>(IBaseService<T> service)
            where T : GenericModel<int>, new();
        T GetEntity<T>(string key)
            where T : GenericModel<int>, new();
        int GetEntityId<T>(string key)
            where T : GenericModel<int>, new();
        bool EntityExists<T>(string key)
            where T : GenericModel<int>, new();
        T AddEntity<T>(T entity, string entityKey)
            where T : GenericModel<int>, new();
    }
}