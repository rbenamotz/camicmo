#include <avr/io.h>
#include <util/delay.h>
#include "hardware.h"
#include <avr/interrupt.h>
#include "common.h"

struct cRGB led[NUMPIXELS];

void apply()
{
    ws2812_setleds(led, NUMPIXELS);
}

void setupPixels()
{
    led[0].b = 255;
    led[1].b = 255;
}
void loopPixels()
{
    apply();
}
