using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using WebService.Data;

namespace WebService.Controllers
{
    public class MakesController : ODataController
    {
        private CopartEntities db = new CopartEntities();

        // GET: odata/Makes
        [EnableQuery]
        public IQueryable<Make> GetMakes()
        {
            return db.Makes;
        }

        // GET: odata/Makes(5)
        [EnableQuery]
        public SingleResult<Make> GetMake([FromODataUri] int key)
        {
            return SingleResult.Create(db.Makes.Where(make => make.Id == key));
        }

        // GET: odata/Makes(5)/Cars
        [EnableQuery]
        public IQueryable<Car> GetCars([FromODataUri] int key)
        {
            return db.Makes.Where(m => m.Id == key).SelectMany(m => m.Cars);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MakeExists(int key)
        {
            return db.Makes.Count(e => e.Id == key) > 0;
        }
    }
}
