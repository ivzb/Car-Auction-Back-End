namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class ServicesDispatcher : IServicesDispatcher
    {
        private readonly IDictionary<string, object> services; // object == IDefaultService<BaseModel>
        private readonly IDictionary<string, object> entities; // object == IDictionary<string, BaseModel>

        public ServicesDispatcher()
        {
            this.services = new Dictionary<string, object>();
            this.entities = new Dictionary<string, object>();
        }

        public void InjectService<T>(IDefaultService<T> service)
            where T : BaseModel, new()
        {
            string key = this.GetEntityClassName<T>();

            this.services.Add(key, service);
            IDictionary<string, T> currentEntities = service.Get().ToDictionary(x => x.Value);
            this.entities.Add(key, currentEntities);
        }

        public T GetEntity<T>(string entityValue)
            where T : BaseModel, new()
        {
            string key = this.GetEntityClassName<T>();
            IDictionary<string, T> entitiesByKey = (IDictionary<string, T>)this.entities[key];
            T model;
            bool entityFound = ((IDictionary<string, T>)entitiesByKey).TryGetValue(entityValue, out model);

            if (!entityFound)
            {
                model = new T
                {
                    Value = entityValue
                };

                ((IDefaultService<T>)this.services[key]).Add(model);
                ((IDictionary<string, T>)this.entities[key]).Add(entityValue, model);
            }

            return model;
        }


        private string GetEntityClassName<T>()
            where T : class
        {
            return typeof(T).FullName;
        }
    }
}