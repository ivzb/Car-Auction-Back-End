namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class ServicesDispatcher : IServicesDispatcher
    {
        private readonly IDictionary<string, object> services; // object == IDefaultService<BaseModel>
        private readonly IDictionary<string, object> dictionaries; // object == IDictionary<string, BaseModel>

        public ServicesDispatcher()
        {
            this.services = new Dictionary<string, object>();
            this.dictionaries = new Dictionary<string, object>();
        }

        public void InjectService<T>(IDefaultService<T> service)
            where T : BaseModel, new()
        {
            string key = typeof(T).FullName;

            this.services.Add(key, service);
            IDictionary<string, T> currentEntities = service.Get().ToDictionary(x => x.Value);
            this.dictionaries.Add(key, currentEntities);
        }

        public T GetEntity<T>(string entityValue)
            where T : BaseModel, new()
        {
            string key = typeof(T).FullName;
            IDictionary<string, T> entities = (IDictionary<string, T>)this.dictionaries[key];
            T model;
            bool entityFound = ((IDictionary<string, T>)entities).TryGetValue(entityValue, out model);

            if (!entityFound)
            {
                model = new T
                {
                    Value = entityValue
                };

                ((IDefaultService<T>)this.services[key]).Add(model);
                ((IDictionary<string, T>)this.dictionaries[key]).Add(entityValue, model);
            }

            return model;
        }
    }
}