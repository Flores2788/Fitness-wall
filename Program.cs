using System.Device.Gpio;
using System;
using System.Threading;


namespace Blinky
{
    public class Program
    {
        private static GpioController s_GpioController;
        private static GpioPin[] leds = new GpioPin[2];
        private static GpioPin[] buttons = new GpioPin[2];
        private static int rounds = 0;
        private static bool isPlaying = true;


        public static void Main()
        {
            s_GpioController = new GpioController();


            // Define the GPIO pins for LEDs and buttons
            int[] ledPins = { 15, 4 }; // 1, 2
            int[] buttonPins = { 14, 25 }; // 1, 2

            // Initialize LED pins as outputs and button pins as inputs with pull-up resistors
            for (int i = 0; i < 2; i++)
            {
                leds[i] = s_GpioController.OpenPin(ledPins[i], PinMode.Output);
                buttons[i] = s_GpioController.OpenPin(buttonPins[i], PinMode.InputPullUp);

                // Turn off all LEDs initially
                leds[i].Write(PinValue.High);
            }

            // Register event handlers for button presses
            for (int i = 0; i < 2; i++)
            {
                int buttonIndex = i; // Capturing the button index in the loop
                buttons[i].ValueChanged += (sender, args) => Button_ValueChanged(leds[buttonIndex], args);
            }



            while (isPlaying)
            {
                if (rounds == 20)
                {
                    isPlaying = false;
                }
            }




        }

        private static void Button_ValueChanged(GpioPin led, PinValueChangedEventArgs e)
{
   if (e.ChangeType == PinEventTypes.Falling) // Button pressed
{
    // Turn off all LEDs
    foreach (var pin in leds)
    {
        pin.Write(PinValue.High);
    }

    // Turn on the LED associated with the pressed button
    led.Write(PinValue.Low);
    rounds++;
    var currentTime = DateTime.Now;
    Console.WriteLine($"Button was pressed at {currentTime}. Total presses: {rounds}");

    // Log the round data with time
    string logText = $"Button was pressed at {currentTime}. Total presses: {rounds}" + Environment.NewLine;
    Console.WriteLine("Buttons were pressed: " + rounds + " times");

    // Log the round data
    string logText = "Buttons were pressed: " + rounds + " times" + Environment.NewLine;
    File.AppendAllText("log.txt", logText);
}
}
    }
}
