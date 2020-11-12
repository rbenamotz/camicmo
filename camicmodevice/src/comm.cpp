#include "pa/picoUART.h"
#include "pa/pu_print.h"
#include "common.h"
#define CMD_TYPE_HANDSHAKE 0xC8
#define CMD_TYPE_SET_LED_COLORS 0xC9
#define RESULT_OK 0x64
#define RESULT_FAIL 0x65

struct msg
{
    uint8_t cmd;
    uint8_t data[6];
} activeMessage;

void handleCommand()
{
    if (activeMessage.cmd == CMD_TYPE_HANDSHAKE)
    {
        putx(RESULT_OK);
    }
    if (activeMessage.cmd == CMD_TYPE_SET_LED_COLORS)
    {
        led[0].r = activeMessage.data[0];
        led[0].g = activeMessage.data[1];
        led[0].b = activeMessage.data[2];
        led[1].r = activeMessage.data[3];
        led[1].g = activeMessage.data[4];
        led[1].b = activeMessage.data[5];
        putx(RESULT_OK);
    }
}
void setupComm()
{
}
void loopComm()
{
    static uint8_t datapos = 0;
    uint8_t c = purx();
    if (c >= CMD_TYPE_HANDSHAKE && c <= CMD_TYPE_SET_LED_COLORS)
    {
        activeMessage.cmd = c;
        datapos = 0;
        return;
    }
    activeMessage.data[datapos] = c;
    datapos++;
    if (datapos == 6)
    {
        handleCommand();
        datapos = 0;
    }
}
