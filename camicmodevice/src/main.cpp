// #include <avr/io.h>
#include <util/delay.h>
#include "pixels.h"
#include "comm.h"
#include "common.h"
struct cRGB led[NUMPIXELS];


void loop()
{
  loopComm();
  loopPixels();
}

int main(void)
{
  setupComm();
  setupPixels();
  while (true)
  {
    loop();
  }
}
