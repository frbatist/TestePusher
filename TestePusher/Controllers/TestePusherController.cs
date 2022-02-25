using Microsoft.AspNetCore.Mvc;
using PusherServer;
using System.Threading.Tasks;

namespace TestePusher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestePusherController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> HelloWorld()
        {
            var options = new PusherOptions
            {
                Cluster = PusherSettings.Cluster,
                Encrypted = true
            };

            var pusher = new Pusher(
                  PusherSettings.AppId,
                  PusherSettings.AppKey,
                  PusherSettings.AppSecret,
                  options);

            var result = await pusher.TriggerAsync(
              PusherSettings.ChannelName,
              PusherSettings.EventName,
              new { message = "hello world" });

            return Ok(result);
        }
    }
}
