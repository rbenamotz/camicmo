import serial
ser = serial.Serial('COM10',9600)  # open serial port
# ser.baudrate = 115200
# ser.open()
hand_shake = bytearray([200, 0,0,0,0,0,0])
garabage = bytearray([98, 1,2,3,4,5,6,7])
change_colors = bytearray([201, 7,0,0,0,7,0])
out = hand_shake
print(','.join(format(x, '02') for x in out))
ser.write(out)

# output = ser.read(7)
output = ser.read()
print(','.join(format(x, '02') for x in output))
ser.close()