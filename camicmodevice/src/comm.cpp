#include "pa/picoUART.h"
#include "pa/pu_print.h"
#include "common.h"
#define CMD_TYPE_HANDSHAKE 200
#define CMD_TYPE_SET_LED_COLORS 201
#define RESULT_OK_V1 0x64
#define RESULT_OK_V2 0x66
#define RESULT_OK RESULT_OK_V2
#define RESULT_FAIL 0x65

volatile struct msg
{
    uint8_t cmd;
    uint8_t data[6];
} activeMessage;

void handleCommand()
{
    if (activeMessage.cmd == CMD_TYPE_HANDSHAKE)
    {
        putx(RESULT_OK);
        return;
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
        return;
    }
    putx(RESULT_FAIL);
    // putx(activeMessage.cmd);
    // for (uint8_t i = 0; i < 6; i++)
    // {
    //     putx(activeMessage.data[i]);
    // }
}
void setupComm()
{
}
void loopComm()
{
    static uint8_t buffer[7] = {0};
    static uint8_t position = 0;
    while (position<7) {
        uint8_t c = purx();
        if (c==CMD_TYPE_HANDSHAKE || c==CMD_TYPE_SET_LED_COLORS) {
            position = 0;
        }
        buffer[position] = c;
        position++;
    }
    position = 0;
    activeMessage.cmd = buffer[0];
    for (uint8_t i = 0; i < 6; i++)
    {
        activeMessage.data[i] = buffer[i+1] * 2 + buffer[i+1] /2;
    }
    handleCommand();
}

// void loopComm()
// {
//     uint8_t buffer[7] = {0};
//     for (uint8_t i = 0; i < 7; i++) {
//         buffer[i] = purx();
//     }
//     activeMessage.cmd = buffer[0];
//     for (uint8_t i = 0; i < 6; i++)
//     {
//         activeMessage.data[i] = buffer[i+1];
//     }
//     handleCommand();
// }
