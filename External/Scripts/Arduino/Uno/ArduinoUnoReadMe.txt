1. During the test development, we found that the serial communication DTR issue below:
The setup() function in Arduino code is designed to run only once when the Arduino board powers up or is reset. It should not be executing repeatedly when you send commands via Python over the serial port.

The issue you're likely experiencing is that your Python script is causing the Arduino to reset when it establishes the serial connection. This is a common behavior on many Arduino boards (like the Uno) due to the way the serial port is implemented and how the host computer interacts with it.

Here's a breakdown of why this happens and how to potentially mitigate it:

Why the Reset Occurs:

DTR (Data Terminal Ready) Signal: When a serial connection is established with an Arduino Uno (and many similar boards), the host computer often toggles the DTR (Data Terminal Ready) control line.
Connection to Reset Pin: On the Arduino Uno, the DTR signal is connected (through a capacitor) to the reset pin of the microcontroller (ATmega328P).
Capacitor Effect: When the DTR line is asserted (typically goes low when the serial port is opened), the capacitor creates a brief pulse on the reset pin, causing the microcontroller to reset.
setup() Execution: After the reset, the Arduino naturally starts executing the code from the beginning, which includes running the setup() function.
Why You See setup() Behavior:

The actions you perform in setup() (initializing the LCD, setting the serial baud rate, printing "Serial Control") become visible because the board is indeed resetting when your Python script opens the serial port.

How to Prevent or Mitigate the Reset:

There are a few ways to address this, with varying degrees of effectiveness:

a. Modify the Arduino Hardware (Adding a Capacitor):

You can add a capacitor (typically around 0.1uF) between the reset pin and +5V on the Arduino. This can help to filter out the short reset pulse caused by the DTR signal.
Caution: This involves hardware modification and should be done carefully. Incorrect wiring can damage your board. Search online for "Arduino DTR reset disable capacitor" for detailed instructions and diagrams.

b. Cutting the "RESET-EN" trace on your Arduino Uno is the most direct and permanent hardware method to disable the automatic reset that occurs when the serial port is opened.

The RTS (Request To Send) signal, or any other manipulation of the serial control lines by your Python script (including DTR), should no longer trigger an automatic reset of your Arduino.

Why you might still perceive a reset:

Even with the "RESET-EN" trace cut, there are a few less likely but possible scenarios where you might still see behavior that seems like a reset when you open a new Python serial instance:

Power Cycle: When you start a new Python instance and it tries to open the serial port, the underlying USB connection might briefly be interrupted or re-initialized by your operating system or the USB drivers. In some very rare cases, this could cause a power fluctuation that might lead to the Arduino restarting, although this is not the typical behavior.

Your Arduino Code: Double-check your setup() function in your Arduino code. If it contains any initialization routines that take a significant amount of time or produce visible effects (like rapidly flashing LEDs or initializing external hardware), you might be simply observing the setup() running when you power up the Arduino (which happens when you plug in the USB or potentially when the OS re-initializes the USB connection for a new serial port access).

Python Script Behavior: Review your Python script to ensure it's not explicitly sending a reset command (which is unlikely with standard pyserial usage unless you've added such functionality).

Other Hardware Issues: Although less probable, a faulty USB cable or intermittent connection could potentially cause the Arduino to lose power and restart.

2. After cutting the "RESET-EN" trace on your Arduino Uno, in order to upload sketch program from PC to arduino:
a. Quick answer:
Prepare your sketch in the Arduino IDE and click the "Upload" button.
Watch the Arduino IDE's status window closely. It will go through the following stages:
"Compiling sketch..."
"Uploading..." (This is the crucial moment)
Just as the IDE's status changes from "Compiling sketch..." to "Uploading...", quickly press and release the reset button on your Arduino Uno.
Then, you will see "Uploading" appear only. quickly press and release the reset button on your Arduino Uno again.

b. Detail Explanation:
After you've cut the "RESET-EN" trace on your Arduino Uno, the automatic reset triggered by the serial port's DTR signal is disabled. This means the Arduino will no longer automatically enter the bootloader when a serial connection is opened.

Therefore, the standard method of simply clicking "Upload" in the Arduino IDE will likely fail. The IDE will attempt to initiate the upload process by opening the serial port and expecting the bootloader to be active and listening for the new sketch. Since the bootloader isn't automatically started, the upload will time out or give an error indicating it can't communicate with the board.

Here's how you can upload a new sketch after cutting the "RESET-EN" trace:

You need to manually trigger the bootloader mode on the Arduino Uno right before the Arduino IDE starts sending the new sketch. This involves pressing the reset button on the Arduino board at a very specific moment during the upload process.

The "Timing Trick":

Prepare your sketch in the Arduino IDE and click the "Upload" button.
Watch the Arduino IDE's status window closely. It will go through the following stages:
"Compiling sketch..."
"Uploading..." (This is the crucial moment)
Just as the IDE's status changes from "Compiling sketch..." to "Uploading...", quickly press and release the reset button on your Arduino Uno.
The Goal:

The idea is to press the reset button so that the bootloader starts running just as the Arduino IDE begins transmitting the new sketch data over the serial port. The bootloader has a short window of a few seconds where it listens for upload commands after a reset.

Tips and Troubleshooting:

Timing is Critical: This method requires some practice and good timing. You might need to try it a few times to get the hang of it. Pressing the reset button too early or too late will result in an upload failure.
Watch the IDE Output: The Arduino IDE's output window can give you clues about whether you're getting the timing right. If you see messages indicating a timeout or inability to sync with the board, your timing is likely off.
Try Different Timing: Experiment with pressing the reset button slightly before, exactly at, or slightly after the "Uploading..." message appears.
Ensure Serial Port is Selected: Double-check that the correct serial port for your Arduino Uno is selected in the Arduino IDE under the "Tools" > "Port" menu.
No Other Serial Connections: Make sure no other programs (like a serial monitor) are currently connected to your Arduino's serial port.
Alternative (More Involved) Methods:

If the timing trick proves too frustrating, you have a couple of more involved alternatives:

Using an External Programmer (ISP): You can use an In-System Programmer (ISP) device to directly flash the new sketch onto the Arduino's microcontroller, bypassing the bootloader entirely. This requires additional hardware and a different upload method in the Arduino IDE (selecting "Upload Using Programmer"). You would also need to connect the ISP to the ICSP header on your Arduino Uno.

Re-enabling Auto-Reset (If Desired Later): If you decide you want the automatic reset functionality back, you would need to carefully solder the "RESET-EN" trace that you previously cut. This requires fine soldering skills. You might consider using a thin wire to bridge the gap.

In summary, after cutting the "RESET-EN" trace, you'll need to manually trigger the bootloader by pressing the reset button on your Arduino Uno at the precise moment the Arduino IDE starts uploading the new sketch. It might take a few tries to get the timing right.