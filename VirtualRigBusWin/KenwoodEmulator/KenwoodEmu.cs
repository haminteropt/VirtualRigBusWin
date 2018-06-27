using HamBusLib;
using HamBusLib.UdpNetwork;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

using System.Threading.Tasks;

namespace KenwoodEmulator
{
    public class KenwoodEmu
    {
        private SerialPort _serialPort;
        NetworkThreadRunner networkThreadRunner = NetworkThreadRunner.GetInstance();
        public string Id
        {
            get
            {
                return state.Id;
            }
            set
            {
                state.Id = value;
            }
        }
        public enum Mode
        {
            LSB = 1,
            USB = 2,
            CW = 3,
            FM = 4,
            AM = 5,
            FSK = 6,
            CWR = 7,
            Tune = 8,
            FSR = 9,
            ERROR = 10
        }

        private RigOperatingState state = new RigOperatingState();
        bool _continue;
        public KenwoodEmu()
        {
            state.Type = "RigOperatingState";
        }
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
            _serialPort.ReadTimeout = 5000;
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
            string subcmd = cmd.Substring(0, 2);
            switch (subcmd)
            {
                case "FA":
                    FreqCommand(cmd);
                    break;
                case "MD":
                    ModeCommand(cmd);
                    break;
                case "TX":
                    TXRXCommand(cmd);
                    break;
                case "RX":
                    TXRXCommand(cmd);
                    break;
                case "?;":
                    Console.WriteLine("Error: {0}", cmd);
                    break;
            }
        }
        private void TXRXCommand(string cmd)
        {
            if (cmd == "TX;")
            {
                state.Tx = true;
            }
            else
            {
                state.Tx = false;
            }
            networkThreadRunner.SendBroadcast(state, 7300);
        }
        private void ModeCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                Console.WriteLine("command: {0}", cmd);

                int mode = Convert.ToInt32(ModeStdToKenwoodEnum());
                var modeFmt = string.Format("MD{0};", mode.ToString());
                _serialPort.Write(modeFmt);
                return;
            }
            var semiLoc = cmd.IndexOf(';');
            var modeEnumStr = cmd.Substring(2, semiLoc - 2);
            var modeInt = Convert.ToInt32(modeEnumStr);
            state.Mode = ((Mode)modeInt).ToString();
            networkThreadRunner.SendBroadcast(state, 7300);
        }
        private void FreqCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                Console.WriteLine("command: {0}", cmd);
                _serialPort.Write(state.Freq.ToString("D11"));
                return;
            }

            var semiLoc = cmd.IndexOf(';');
            var freqStr = cmd.Substring(2, semiLoc - 2);
            var freqInt = Convert.ToInt64(freqStr);
            state.Freq = freqInt;
            networkThreadRunner.SendBroadcast(state, 7300);
            Console.WriteLine("freq: {0}", freqInt);
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
                        Console.WriteLine("cmd: {0}", sb);
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
        private Mode ModeStdToKenwoodEnum()
        {
            switch (state.Mode.ToUpper())
            {
                case "USB":
                    return Mode.USB;
                case "LSB":
                    return Mode.LSB;
                case "CW":
                    return Mode.CW;
                case "CWL":
                    return Mode.CW;
                case "CWU":
                    return Mode.CW;
                case "AM":
                    return Mode.AM;
                case "FM":
                    return Mode.FM;
                case "FSK":
                    return Mode.FSK;
                case "DIGH":
                    return Mode.FSK;
                case "DIGL":
                    return Mode.FSR;
                case "CWR":
                    return Mode.CWR;
                case "FSR":
                    return Mode.FSR;
                case "TUNE":
                    return Mode.Tune;
            }
            return Mode.ERROR;

        }

    }
}
