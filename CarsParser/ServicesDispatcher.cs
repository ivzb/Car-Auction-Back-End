namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;
    using System;
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

        public IServicesDispatcher InjectService<T>(IBaseService<T> service)
            where T : GenericModel<int>, new()
        {
            string key = this.GetClassName<T>();
            Console.WriteLine("Loading {0}", key);

            this.services.Add(key, service);
            this.entities.Add(key, service.GetEntitiesAsDictionary());

            return this;
        }

        public T GetEntity<T>(string key)
            where T : GenericModel<int>, new()
        {
            string serviceKey = this.GetClassName<T>();
            IDictionary<string, T> serviceEntities = (IDictionary<string, T>)this.entities[serviceKey];
            T entity;
            bool entityExists = serviceEntities.TryGetValue(key, out entity);

            if (!entityExists)
            {
                entity = (T)Activator.CreateInstance(typeof(T), key);
                this.AddEntity<T>(entity, key);
            }

            return entity;
        }

        public int GetEntityId<T>(string key)
            where T : GenericModel<int>, new()
        {
            string serviceKey = this.GetClassName<T>();
            IDictionary<string, T> serviceEntities = (IDictionary<string, T>)this.entities[serviceKey];
            T entity;
            bool entityExists = serviceEntities.TryGetValue(key, out entity);

            if (entityExists)
            {
                return entity.Id;
            }

            return 0;
        }

        public bool EntityExists<T>(string key)
            where T : GenericModel<int>, new()
        {
            string serviceKey = this.GetClassName<T>();
            IDictionary<string, T> serviceEntities = (IDictionary<string, T>)this.entities[serviceKey];
            T entity;
            bool entityExists = serviceEntities.TryGetValue(key, out entity);

            return entityExists;
        }

        public T AddEntity<T>(T entity, string entityKey)
            where T : GenericModel<int>, new()
        {
            string key = this.GetClassName<T>();
            ((IBaseService<T>)this.services[key]).Add(entity);
            ((IDictionary<string, T>)this.entities[key]).Add(entityKey, entity);

            return entity;
        }

        private string GetClassName<T>()
            where T : class
        {
            return typeof(T).FullName;
        }
    }
}