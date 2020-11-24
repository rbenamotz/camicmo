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
    for (uint8_t u = 0; u < 3; u++)
    {
        for (uint8_t i = 0; i < 50; i++)
        {
            led[0].r = i;
            led[0].g = i;
            led[0].b = i;
            led[1].r = i;
            led[1].g = i;
            led[1].b = i;
            _delay_ms(5);
            apply();
        }
        for (uint8_t i = 50; i >= 10; i--)
        {
            led[0].r = i;
            led[0].g = i;
            led[0].b = i;
            led[1].r = i;
            led[1].g = i;
            led[1].b = i;
            _delay_ms(5);
            apply();
        }
    }
    // led[0].r = 50;
    // led[0].b = 30;
    // led[1].r = 50;
    // led[1].b = 30;
    apply();
}
void loopPixels()
{
    apply();
}
