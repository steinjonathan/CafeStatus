using CafeStatus.Models;
using System.Linq;
using System.Web.Http;

namespace CafeStatus.Controllers.api
{
    [RoutePrefix("api/Status"), AllowAnonymous]
    public class StatusController : ApiController
    {
        public bool Get()
        {
            using (CafeDBContext db = new CafeDBContext())
            {
                return db.Status.OrderByDescending(p => p.Data).Take(1).Select(a=>a.Pronto).FirstOrDefault();
            }
        }
    }
}
