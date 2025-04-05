import serial
import time
import argparse

def send_command(ser, command):
    """Sends a command to the Arduino and waits for acknowledgment."""
    ser.write((command + "\n").encode())
    time.sleep(0.1)  # Give Arduino time to process
    response = ser.readline().decode().strip() # read response
    return response

def main():
    parser = argparse.ArgumentParser(description="Send commands to Arduino via serial.")
    parser.add_argument("--comport", "-p", default="COM9", help="Serial COM port (e.g., COM9)")
    parser.add_argument("--baudrate", "-b", type=int, default=9600, help="Serial baud rate (e.g., 9600)")
    parser.add_argument("--command", "-c", default="UpVolume", help="Serial Command (e.g., UpVolume, DownVolume)")

    args = parser.parse_args()

    try:
        #ser = serial.Serial(
        #    args.comport,  # Replace with your serial port (e.g., 'COM3' on Windows, '/dev/ttyACM0' on Linux)
        #    baudrate=args.baudrate,
        #    bytesize=serial.EIGHTBITS,
        #    parity=serial.PARITY_NONE,
        #    stopbits=serial.STOPBITS_ONE,
        #    timeout=1,
        #    rtscts=False,  # Disable RTS/CTS flow control
        #    dsrdtr=False   # Disable DSR/DTR flow control
        #)
        ser = serial.Serial(args.comport, args.baudrate, timeout=1)
        time.sleep(2)  # Allow time for serial connection to establish.

        response = send_command(ser, args.command)
        print(f"Arduino Response: {response}")
    
        if (response == args.command + "_ACK"):  
            print("TESTPASS")
        else:
            print("TESTFAIL")

    except serial.SerialException as e:
        print(f"Error: {e}")
        print("TESTFAIL")

    except KeyboardInterrupt:
        print("\nProgram interrupted by user.")
        print("TESTABORT")

    finally:
        try:
            if 'ser' in locals() and ser.is_open:
                ser.close()
                print("Serial port closed.")
        except:
            print("Error closing serial port, or port was never opened.")
            print("TESTFAIL")

if __name__ == "__main__":
    main()