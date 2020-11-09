#include <windows.h>

/* ... */


// Open serial port
HANDLE serialHandle;

serialHandle = CreateFile("\\\\.\\COM1", GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);

// Do some basic settings
DCB serialParams = { 0 };
serialParams.DCBlength = sizeof(serialParams);

GetCommState(serialHandle, &serialParams);
serialParams.BaudRate = baudrate;
serialParams.ByteSize = byteSize;
serialParams.StopBits = stopBits;
serialParams.Parity = parity;
SetCommState(serialHandle, &serialParams);

// Set timeouts
COMMTIMEOUTS timeout = { 0 };
timeout.ReadIntervalTimeout = 50;
timeout.ReadTotalTimeoutConstant = 50;
timeout.ReadTotalTimeoutMultiplier = 50;
timeout.WriteTotalTimeoutConstant = 50;
timeout.WriteTotalTimeoutMultiplier = 10;

SetCommTimeouts(serialHandle, &timeout);