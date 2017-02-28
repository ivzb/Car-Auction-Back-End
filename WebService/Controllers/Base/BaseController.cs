namespace WebService.Controllers.Base
{
    using AutoMapper;
    using Data.Common.Models;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.OData;
    using WebService.Infrastructure.Automapper;

#if DEBUG
    [EnableCors(origins: "*", headers: "*", methods: "*")]
#endif
    public class BaseController<T> : ODataController
        where T : GenericModel<int>
    {
        protected IBaseService<T> service;

        public BaseController(IBaseService<T> service)
        {
            this.service = service;
        }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected IQueryable<T> Get()
        {
            return this.service.GetAll();
        }

        protected SingleResult<T> Get(int key)
        {
            return SingleResult.Create(this.service.GetAll(x => x.Id == key));
        }

        protected SingleResult<K> Get<K>(int key, Expression<Func<T, K>> selector)
            where K : GenericModel<int>
        {
            return SingleResult.Create(this.service.GetAll(x => x.Id == key).Select(selector));
        }

        protected IQueryable<K> Get<K>(int key, Expression<Func<T, IEnumerable<K>>> selector)
            where K : GenericModel<int>
        {
            return this.service.GetAll(x => x.Id == key).SelectMany(selector);
        }
    }
}