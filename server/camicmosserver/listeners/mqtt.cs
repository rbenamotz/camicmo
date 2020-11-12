using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace camicmosserver.listeners
{
    public class MqttBroker : IListener
    {
        private readonly IMqttClient _client;
        private State _pendingState = null;

        public  MqttBroker() 
        {
            var f = new MqttFactory();
            _client = f.CreateMqttClient();

        }
        private void Publish(State state)
        {
            if (!_client.IsConnected)
            {
                _pendingState = state;
                return;
            }
            Publish(State.WEBCAM, state.IsCapbilityOn(State.WEBCAM));
            Publish(State.MIC, state.IsCapbilityOn(State.MIC));
            PublishProgs(state, State.WEBCAM);
            PublishProgs(state, State.MIC);
            _pendingState = null;
        }
        private async void Publish(string capability, bool val)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("camicmo-" + capability + "-state")
                .WithPayload(val ? "ON" : "OFF")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
            await _client.PublishAsync(message, CancellationToken.None);

        }
        private async void PublishProgs( State state, string capbility)
        {
            String payload = "";
            foreach (var p in state.ProgsForCapability(capbility))
            {
                if (payload.Length>0) { payload = payload + ","; }
                payload += p;
            }
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("camicmo-" + capbility + "-progs")
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
            await _client.PublishAsync(message, CancellationToken.None);

        }

        public void OnStateChanged(State state)
        {
            Publish(state);
        }

        public void Init(dynamic config)
        {
            String broker = config["broker"];
            if (String.IsNullOrEmpty(broker))
            {
                return;
            }
            var ob = new MqttClientOptionsBuilder();
            if (config["clientId"] != null)
            {
                ob.WithClientId((string) config["clientId"]);
            } else
            {
                ob.WithClientId("camicmo");
            }
            ob.WithTcpServer((string) config.broker, (int?) config.port);
            if (config["credentials"] != null)
            {
                ob.WithCredentials((string)config.credentials.username, (string)config.credentials.password);
            }
            var options = ob.Build();
            _client.UseConnectedHandler( e =>
            {
                Console.WriteLine("Connected to MQTT");
                if (_pendingState!=null)
                {
                    Publish(_pendingState);
                }
            });
            _client.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await _client.ConnectAsync(options, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });
            _client.ConnectAsync(options, CancellationToken.None);

        }
    }
}
