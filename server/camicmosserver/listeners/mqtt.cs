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
        private Dictionary<string, bool> _lastDataSent =new Dictionary<string, bool>();
        public  MqttBroker() 
        {
            var f = new MqttFactory();
            _client = f.CreateMqttClient();

        }

        private async void Publish(string topic, bool val)
        {
            if (_lastDataSent.ContainsKey(topic) && _lastDataSent[topic] == val)
            {
                return;
            }
            if (!_client.IsConnected)
            {
                return;
            }
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("camicmo-" + topic)
                .WithPayload(val ? "ON" : "OFF")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
            await _client.PublishAsync(message, CancellationToken.None);
            _lastDataSent.Remove(topic);
            _lastDataSent.Add(topic, val);

        }

        public void OnStateChanged(State state)
        {
            if (!_client.IsConnected)
            {
                return;
            }
            Publish("cam", state.IsCamOn);
            Publish("mic", state.IsMicOn);
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
