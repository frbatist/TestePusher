using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PusherClient;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestePusher
{
    public class TesteListener : IHostedService, IDisposable
    {
        private readonly Pusher _pusher;
        private readonly ILogger<TesteListener> _logger;

        public TesteListener(ILogger<TesteListener> logger)
        {
            _pusher = new Pusher(PusherSettings.AppKey, new PusherOptions
            {
                Cluster = PusherSettings.Cluster
            });
            _pusher.Disconnected += OnDisconnected;
            _pusher.Connected += OnConnected;
            _pusher.Error += ErrorHandler;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _pusher.ConnectAsync();
            await _pusher.SubscribeAsync(PusherSettings.ChannelName);
            _pusher.Bind(PusherSettings.EventName, (PusherEvent d) =>
            {
                Execute(d.Data);
            });
            _logger.LogInformation("Subscribed to channel.");
        }
        
        void OnDisconnected(object sender)
        {
            _logger.LogInformation("Disconnected: " + ((Pusher)sender).SocketID);
        }
        
        void OnConnected(object sender)
        {
            _logger.LogInformation("Connected: " + ((Pusher)sender).SocketID);
        }

        void ErrorHandler(object sender, PusherException error)
        {
            if ((int)error.PusherCode < 5000)
            {
                _logger.LogInformation("Error recevied from Pusher cluster, use PusherCode to filter.");
            }
            else
            {
                if (error is ChannelUnauthorizedException)
                {
                    _logger.LogInformation("Private and Presence channel failed authorization with Forbidden (403)");
                }
                else if (error is ChannelAuthorizationFailureException)
                {
                    _logger.LogInformation("Authorization endpoint returned an HTTP error other than Forbidden (403)");                    
                }
                else if (error is OperationTimeoutException)
                {
                    _logger.LogInformation("A client operation has timed-out. Governed by PusherOptions.ClientTimeout");                    
                }
                else if (error is ChannelDecryptionException)
                {
                    _logger.LogInformation("Failed to decrypt the data for a private encrypted channel");                    
                }
                else
                {
                    _logger.LogInformation($"Error on pusher message: {error.Message}");
                }
            }
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
