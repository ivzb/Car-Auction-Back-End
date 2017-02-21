namespace WebService.Controllers.Base
{
    using Data.Common.Models;
    using Data.ViewModels.Base;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using WebService.Infrastructure.Automapper;

    public abstract class GenericController<T, K> : BaseController<T>
        where T : GenericModel<int>
        where K : BaseViewModel
    {
        public GenericController(IBaseService<T> service)
            : base(service)
        {
        }

        protected HttpResponseMessage GetValue(int id)
        {
            try
            {
                T val = service.Get().FirstOrDefault(x => x.Id == id);
                K result = Mapper.Map<T, K>(val);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        protected HttpResponseMessage GetValues(Expression<Func<T, bool>> selector = null)
        {
            try
            {
                IQueryable<T> result = service.Get();
                if (selector != null)
                {
                    result = result.Where(selector);
                }

                IList<K> responseResult = result.To<K>().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, responseResult);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        protected HttpResponseMessage AddValue(K model)
        {
            try
            {
                if (model == null)
                {
                    this.ModelState.AddModelError("model", "The model is empty");
                }

                if (!this.ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                T entity = Mapper.Map<T>(model);
                this.service.Add(entity);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        protected HttpResponseMessage UpdateValue(K model)
        {
            try
            {
                if (model == null)
                {
                    this.ModelState.AddModelError("model", "The model is empty");
                }

                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
                }

                T entity = this.service.Get(model.Id);
                this.Mapper.Map<K, T>(model, entity);
                this.service.Update(entity);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}