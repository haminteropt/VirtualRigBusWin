﻿namespace KenwoodEmulator
{
    using HamBusLib;
    using HamBusLib.Packets;
    using HamBusLib.UdpNetwork;
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;

    public class KenwoodEmu
    {
        private SerialPort serialPort;

        internal UdpServer udpServer = UdpServer.GetInstance();

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
            /// <summary>
            /// Defines the LSB
            /// </summary>
            LSB = 1,
            /// <summary>
            /// Defines the USB
            /// </summary>
            USB = 2,
            /// <summary>
            /// Defines the CW
            /// </summary>
            CW = 3,
            /// <summary>
            /// Defines the FM
            /// </summary>
            FM = 4,
            /// <summary>
            /// Defines the AM
            /// </summary>
            AM = 5,
            /// <summary>
            /// Defines the FSK
            /// </summary>
            FSK = 6,
            /// <summary>
            /// Defines the CWR
            /// </summary>
            CWR = 7,
            /// <summary>
            /// Defines the Tune
            /// </summary>
            Tune = 8,
            /// <summary>
            /// Defines the FSR
            /// </summary>
            FSR = 9,
            /// <summary>
            /// Defines the ERROR
            /// </summary>
            ERROR = 10
        }

        private RigOperatingState state = RigOperatingState.Instance;

        private void SendSerial(string str)
        {
            serialPort.Write(str);
        }

        private bool continueReadingSerialPort;

        public KenwoodEmu()
        {
            state.DocType = DocTypes.OperatingState;
        }

        public void ClosePort()
        {
            continueReadingSerialPort = false;
        }
        public void OpenPort(string portName)
        {

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(ReadSerialPortThread);

            // Create a new SerialPort object with default settings.
            serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            serialPort.PortName = "com20";
            serialPort.BaudRate = 57600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;


            // Set the read/write timeouts
            serialPort.ReadTimeout = 5000;
            serialPort.WriteTimeout = 500;

            serialPort.Open();
            continueReadingSerialPort = true;
            readThread.Start();
        }

        public void command(string cmd)
        {
            string subcmd = cmd.Substring(0, 2);
            switch (subcmd)
            {
                case "ID":
                    IDCommand(cmd);
                    break;
                case "AI":
                    AICommand(cmd);
                    break;
                case "FA":
                    FreqCommand(cmd);
                    break;
                case "FB":
                    FreqCommand(cmd);
                    break;
                case "FR":
                    FreqCommand(cmd);
                    break;
                case "FT":
                    FTCommand(cmd);
                    break;
                case "MD":
                    ModeCommand(cmd);
                    break;
                case "TX":
                    TXRXCommand(cmd);
                    break;
                case "IF":
                    IFCommand(cmd);
                    break;
                case "RX":
                    TXRXCommand(cmd);
                    break;
                case "KS":
                    KSCommand(cmd);
                    break;
                case "SM":
                    SMCommand(cmd);
                    break;
                case "EX":
                    EXCommand(cmd);
                    break;
                case "?;":
                    Console.WriteLine("Error: {0}", cmd);
                    break;
                default:
                    Console.WriteLine("Unknown: {0}", cmd);
                    break;
            }
        }

        #region parse commands
        private void SMCommand(string cmd)
        {
            SendSerial("SM00000;");
        }

        private void EXCommand(string cmd)
        {
            SendSerial("?;");
        }

        private void KSCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                SendSerial("KS010;");
            }
        }

        private void FTCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                SendSerial("FT0;");
            }
        }

        private void IDCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                SendSerial("ID020;");
            }
        }

        private void AICommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                SendSerial("AI0;");
            }
        }

        private void IFCommand(string cmd)
        {
            string sendStr;
            string extStr;
            if (cmd.Length != 3)
                return;
            int iTx = 0;
            if (state.Tx)
                iTx = 1;
            extStr = string.Format("{0}000000 ",
                Convert.ToInt32(ModeStdToKenwoodEnum()));  // p15 6
            sendStr = string.Format("IF{0}{1}{2}{3}{4}{5}{6}{7}{8};",
                state.Freq.ToString("D11"), //p1
                "TS480",//p2
                "+0000",// p3
                "0", // p4
                "0", // p5
                "0", // p6
                "00", // p7
                iTx.ToString(), //p8
                extStr); // p9

            SendSerial(sendStr);
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
            udpServer.SendBroadcast(state, 7300);
        }

        private void ModeCommand(string cmd)
        {
            if (cmd.Length == 3)
            {

                int mode = Convert.ToInt32(ModeStdToKenwoodEnum());
                var modeFmt = string.Format("MD{0};", mode.ToString());
                SendSerial(modeFmt);

                return;
            }
            var semiLoc = cmd.IndexOf(';');
            var modeEnumStr = cmd.Substring(2, semiLoc - 2);
            var modeInt = Convert.ToInt32(modeEnumStr);
            state.Mode = ((Mode)modeInt).ToString();
            udpServer.SendBroadcast(state, 7300);
        }

        private void FreqCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                if (cmd[1].ToString().ToLower() == "a")
                    SendSerial("FA" + state.FreqA.ToString("D11") + ";");
                else
                    SendSerial("FB" + state.FreqB.ToString("D11") + ";");
                return;
            }

            var semiLoc = cmd.IndexOf(';');
            var freqStr = cmd.Substring(2, semiLoc - 2);
            var freqInt = Convert.ToInt64(freqStr);
            state.Freq = freqInt;
            udpServer.SendBroadcast(state, 7300);
        }

        private void VFOCommand(string cmd)
        {
            if (cmd.Length == 3)
            {
                SendSerial("FR0;");
                return;
            }
        }
        #endregion
        public void ReadSerialPortThread()
        {
            StringBuilder sb = new StringBuilder();
            while (continueReadingSerialPort)
            {
                try
                {
                    //string message = _serialPort.ReadLine();
                    int c = serialPort.ReadChar();
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
            serialPort.Close();
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
