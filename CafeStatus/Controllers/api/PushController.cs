using CafeStatus.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CafeStatus.Controllers.api
{
    [RoutePrefix("api/Push")]
    public class PushController : ApiController
    {
        [HttpPost]
        [Route("Register")]
        public async Task Register([FromBody]PushRegisterModel endpoint)
        {
            var registerId = endpoint?.Endpoint?.Replace("https://android.googleapis.com/gcm/send/", "");
            if (registerId != null)
            {
                using (var contexto = new CafeDBContext())
                {
                    var pushDevice = contexto.PushDevices.Where(a => a.Endpoint == registerId).FirstOrDefault();
                    if (pushDevice == null)
                    {
                        contexto.PushDevices.Add(new PushDevice()
                        {
                            Endpoint = registerId
                        });
                        await contexto.SaveChangesAsync();
                    }
                }
            }
        }

        [HttpPost]
        [Route("Unregister")]
        public async Task Unregister([FromBody]PushRegisterModel endpoint)
        {
            var registerId = endpoint?.Endpoint?.Replace("https://android.googleapis.com/gcm/send/", "");
            if (registerId != null)
            {
                using (var contexto = new CafeDBContext())
                {
                    var pushDevice = contexto.PushDevices.Where(a => a.Endpoint == registerId).FirstOrDefault();
                    if (pushDevice != null)
                    {
                        contexto.PushDevices.Remove(pushDevice);
                        await contexto.SaveChangesAsync();
                    }
                }
            }
        }
    }

    public class PushRegisterModel
    {
        public string Endpoint { get; set; }
    }
}
