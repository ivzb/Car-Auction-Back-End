namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;
    using System.Collections.Generic;
    using System.Linq;

    public class ServicesDispatcher : IServicesDispatcher
    {
        private readonly IDictionary<string, object> services; // IDefaultService<BaseModel>
        private readonly IDictionary<string, object> dictionaries; // IDictionary<string, BaseModel>

        public ServicesDispatcher()
        {
            this.services = new Dictionary<string, object>();
            this.dictionaries = new Dictionary<string, object>();
        }

        public void AddService<T>(IDefaultService<T> service)
            where T : BaseModel, new()
        {
            string key = typeof(T).FullName;

            this.services.Add(key, service);
            IDictionary<string, T> currentEntities = service.Get().ToDictionary(x => x.Value);
            this.dictionaries.Add(key, currentEntities);
        }

        public T GetEntity<T>(string key)
            where T : BaseModel, new()
        {
            string dictionaryKey = typeof(T).FullName;
            IDictionary<string, T> entities = (IDictionary<string, T>)this.dictionaries[dictionaryKey];
            T model;
            bool entityFound = ((IDictionary<string, T>)entities).TryGetValue(key, out model);

            if (!entityFound)
            {
                model = new T
                {
                    Value = key
                };

                ((IDefaultService<T>)this.services[dictionaryKey]).Add(model);
                ((IDictionary<string, T>)this.dictionaries[dictionaryKey]).Add(key, model);
            }

            return model;
        }
    }
}