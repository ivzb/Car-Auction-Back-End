namespace WebService.Controllers.Base
{
    using System.Web.Http;
    using System.Web.Http.OData;
    using WebService.Data;

    public class BaseController : ODataController
    {
        public BaseController()
        {
            this.Context = new CopartEntities();
        }

        protected CopartEntities Context { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}