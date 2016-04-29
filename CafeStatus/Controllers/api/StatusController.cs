using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CafeStatus.Controllers.api
{
    [RoutePrefix("api/Status")]
    public class StatusController : ApiController
    {
        public string Get()
        {
            return "ok";
        }
    }
}
