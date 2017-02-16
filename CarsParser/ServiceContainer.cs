namespace CarsParser
{
    using Data.Common.Models;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceContainer
        
    {
        private IDictionary<string, IDefaultService<BaseModel<int>>> services;
        private IDictionary<string, IDictionary<string, BaseModel<int>>> dictionaries;

        public ServiceContainer()
        {
            this.services = new Dictionary<string, IDefaultService<BaseModel<int>>>();
            this.dictionaries = new Dictionary<string, IDictionary<string, BaseModel<int>>>();
        }

        public void AddService<T>(IDefaultService<T> service)
            where T : BaseModel<int>, new()
        {
            string key = typeof(T).GetType().ToString();
            //IDefaultService<BaseModel<int>> cast = (IDefaultService<BaseModel<int>>)service;
            this.services.Add(key, cast);
            this.dictionaries.Add(key, cast.Get().ToDictionary(x => x.Value));
        }

        //private IDefaultService<T> Service { get; set; }
        //private Dictionary<string, T> Dictionary<T> { get; set; }

        public T GetValue<T>(string key)
            where T : BaseModel<int>, new()
        {
            BaseModel<int> model;
            string dictionaryKey = typeof(T).GetType().ToString();

            bool keyFound = dictionaries[dictionaryKey].TryGetValue(key, out model);

            if (!keyFound)
            {
                model = new T
                {
                    Value = key
                };

                this.services[dictionaryKey].Add(model);
                this.dictionaries[dictionaryKey].Add(key, model);
            }

            return (T)model;
        }
    }
}