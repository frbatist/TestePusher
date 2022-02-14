using Microsoft.AspNetCore.Mvc;
using PusherServer;
using System.Threading.Tasks;

namespace TestePusher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestePusherController : ControllerBase
    {        
        private const string Cluster = "your-cluster";
        private const string ChannelName = "your-channel";
        private const string EventName = "your-event";
        private const string AppId = "your-app-id";
        private const string AppKey = "your-app-key";
        private const string AppSecret = "your-app-secret";

        [HttpPost]
        public async Task<ActionResult> HelloWorld()
        {
            var options = new PusherOptions
            {
                Cluster = Cluster,
                Encrypted = true
            };

            var pusher = new Pusher(
                  AppId,
                  AppKey,
                  AppSecret,
                  options);

            var result = await pusher.TriggerAsync(
              ChannelName,
              EventName,
              new { message = "hello world" });

            return Ok(result);
        }
    }
}
