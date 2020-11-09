#include "pa/picoUART.h"
#include "pa/pu_print.h"
#include "common.h"
void setupComm()
{
}
void loopComm()
{
    uint8_t  c;
    c = purx();
    led1Status = c;
    c = purx();
    led2Status = c;
    prints("LED 1: ");
    putx(led1Status);
    prints(", LED 1: ");
    putx(led2Status);
    prints("\r\n");
}
