pio run -e production
aws s3 cp .pio\build\production\firmware.hex s3://com.loox.dev.backend-firmwares/camicmo.hex 

