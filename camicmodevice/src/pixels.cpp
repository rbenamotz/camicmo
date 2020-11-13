#include <avr/io.h>
#include <util/delay.h>
#include "hardware.h"
#include <avr/interrupt.h>
#include "common.h"


void apply()
{
    ws2812_setleds(led, NUMPIXELS);
}

void setupPixels()
{
    led[0].r = 50;
    led[0].b = 50;
    led[1].r = 50;
    led[1].b = 50;
    apply();
}
void loopPixels()
{
    apply();
}
