Camicmo server will listen to device events.
Once event occurs (for example, Zoom started using camera),
camicmo will notify all registered listeners. 
To customized listeners, create file called "listeners.json" in the same folder of the 
application and use the following example.
Omnce the file is created need to restart the server.


[
  {
    "type": "serial",
    "config": {
      "micOnColor": [ 0, 0, 255 ],
      "micOffColor": [ 10, 10, 10 ],
      "camOnColor": [ 0, 0, 255 ],
      "camOffColor": [ 10, 10, 10 ]
    }
  },
  {
    "type": "MqttBroker",
    "config": {
      "broker": "",
      "port": 1883,
      "clientId": "camicmo",
      "topicPrefix": "camicmo",
      "credentials": {
        "username": "",
        "password": ""
      }
    }
  }
]