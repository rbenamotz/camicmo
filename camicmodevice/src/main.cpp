// #include <avr/io.h>
#include <util/delay.h>
#include "pixels.h"
#include "comm.h"
#include "common.h"

void loop()
{
  loopComm();
  loopPixels();
}

__attribute((weak)) int main(void)
{
  setupComm();
  setupPixels();
  while (true)
  {
    loop();
  }
}
