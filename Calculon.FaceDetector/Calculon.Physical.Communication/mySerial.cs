using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;


// Created by Sean OBryan
// 12/8/2012
// Email: globalw2865@gmail.com

namespace Calculon.Physical.Communication
{
    public class mySerial
    {
        // Making the serial port
        SerialPort mySerialPort = new SerialPort();

        // Constructor adds the needed details for the serial connection
        // and then open's the serial.
        public mySerial()
        {
            setupSerial();
            openSerial();
        }

        private void setupSerial()
        {
            // COM9 was where my bluetooth was located
            // Your "PortName" may be different as your
            // bluetooth (or USB) may be located somewhere else

            mySerialPort.PortName = "COM3";
            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
        }

        // Open the serialport, if its not available
        // then try again.
        private void openSerial()
        {
            try
            {
                if (!mySerialPort.IsOpen)
                {
                    mySerialPort.Open();
                }
            }
            catch (Exception e)
            {
                // Try again code would be posted but
                // I need to be able to test without the
                // the serial connection.

                // openSerial();
            }
        }

        // Close the serialport
        public void closeSerial()
        {
            if (mySerialPort.IsOpen)
            {
                mySerialPort.Close();
            }
        }

        // Used to write to the serial port and get the response

        public void writeSerial(string str)
        {
            openSerial();
            String cmd = str;
            // Checking for any value

            if (!string.IsNullOrEmpty(cmd))
            {
                mySerialPort.Write(cmd);

                // To ensure the conversation is going well, we listen
                // for bot to speak

                // After sending the command, the code on the Arduino
                // will respond via the "Serial.println" portions to 
                // acknowledge the receipt of the message that we just
                // sent.  The next lines listen for this.

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
        }


        // This is used to debug my commands sent
        /*
         * After a command is sent from this machine, the other machine will
         * receive the command, respond back through serial, and act.
         * The console write presents me with a way to check the 
         * command/response situations.
         */

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadLine();
                Console.Write(indata);

            }
            catch (Exception myexception)
            { 
                // For demo purposes... do nothing (not the coolest thing to do)
            }

        }

    }
}
