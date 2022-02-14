using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PusherClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestePusher
{
    public class TesteListener : IHostedService, IDisposable
    {
        private const string Cluster = "your-cluster";
        private const string ChannelName = "your-channel";
        private const string EventName = "your-event";        
        private const string AppKey = "your-app-key";

        private readonly Pusher _pusher;
        private readonly ILogger<TesteListener> _logger;

        public TesteListener(ILogger<TesteListener> logger)
        {
            _pusher = new Pusher(AppKey, new PusherOptions
            {
                Cluster = Cluster
            });
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _pusher.ConnectAsync();
            await _pusher.SubscribeAsync(ChannelName);
            _pusher.Bind(EventName, (PusherEvent d) =>
            {
                Execute(d.Data);
            });
            _logger.LogInformation("Subscribed to channel.");
        }

        private void Execute(string data)
        {
            _logger.LogInformation(data);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _pusher.UnsubscribeAllAsync();
            await _pusher.DisconnectAsync();
        }

        public async void Dispose()
        {
            await _pusher.UnsubscribeAllAsync();
            await _pusher.DisconnectAsync();
        }
    }
}
