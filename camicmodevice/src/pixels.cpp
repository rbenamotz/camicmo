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
    for (uint8_t g = 0; g < 255; g++)
    {
        led[0].g = g;
        led[1].g = g;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }
    for (uint8_t g = 255; g >0; g--)
    {
        led[0].g = g;
        led[1].g =  g;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }

    for (uint8_t b = 0; b < 255; b++)
    {
        led[0].b = b;
        led[1].b = b;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }
    for (uint8_t b = 255; b >0; b--)
    {
        led[0].b = b;
        led[1].b =  b;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }
        for (uint8_t r = 0; r < 255; r++)
    {
        led[0].r = r;
        led[1].r = r;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }
    for (uint8_t r = 255; r >0; r--)
    {
        led[0].r = r;
        led[1].r =  r;
        ws2812_setleds(led, NUMPIXELS);
        // _delay_ms(5);
    }


    apply();
}
void loopPixels()
{
    apply();
}
