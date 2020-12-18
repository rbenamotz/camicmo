# camicmo
Camera and Microphone Monitor

Camicmo will run in the background (taking almost zero CPU and memory) and will monitor when applications actively use Microphone or Camera.

Once that happens, camicmo will send a notification to a physical device (soon....) and/or to MQTT server.

This allows for easy integration with home automation systems.

## Sample usage

* Light the room better once a camera in use
* Add a warning light while a camera is on
* Illuminate "quiet" sign outside the room while a microphone is in use

## Privacy
Everything is local. The system does not collect any personal information, does not store usage logs, and does not "call home".


## Thanks
Based on idea (and butiful execution) by Tovi Levy. Check his project at https://youtu.be/FTn1oHqCWCM
