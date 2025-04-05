from picosdk.ps2000 import ps2000

device = None  # Initialize device outside the try block

try:
    device = ps2000.open_unit()
    if device is None:
        print("Failed to open the PicoScope device.")
    else:
        print('Device info: {}'.format(device.info))
        print("TESTPASS")

except Exception as e:
    print(f"An unexpected error occurred: {e}")

finally:
    if device is not None:
        try:
            device.close()
        except Exception as e:
            print(f"An unexpected error occurred while closing: {e}")