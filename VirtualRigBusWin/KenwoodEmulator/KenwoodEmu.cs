using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KenwoodEmulator
{
    public class KenwoodEmu
    {
        SerialPort _serialPort;
        bool _continue;
        public void OpenPort(string portName)
        {
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = "com20";
            _serialPort.BaudRate = 57600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;


            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            while (_continue)
            {
                Thread.Sleep(10);
            }

            readThread.Join();
            _serialPort.Close();

        }
        public void command(string cmd)
        {
            switch (cmd)
            {
                case "FA;":
                    Console.WriteLine("command: {0}", cmd);
                    _serialPort.Write("FA00007000000;");
                    break;
                case "MD;":
                    Console.WriteLine("command: {0}", cmd);
                    _serialPort.Write("MD5;");
                    break;
                case "?;":
                    Console.WriteLine("Error: {0}", cmd);
                    break;
            }
        }
        public void Read()
        {
            StringBuilder sb = new StringBuilder();
            while (_continue)
            {
                try
                {
                    //string message = _serialPort.ReadLine();
                    int c = _serialPort.ReadChar();
                    if (c < 0)
                    {
                        Console.WriteLine("Serial port read error");
                        return;
                    }
                    char ch = Convert.ToChar(c);
                    if (ch == ';')
                    {
                        sb.Append(ch);
                        command(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                }
                catch (TimeoutException) { }
            }
        }
        // Display Port values and prompt user to enter a port.
        public string SelectPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }

    }
}
