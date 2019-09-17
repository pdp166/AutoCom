using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.IO;

namespace AutoCom
{
    class AutoTest
    {
        List<string> serialPorts = null;
        SerialPort serialPort = null;
        int testcasecount = 1;

        /* identifies serial connections available*/
        public void FindSerialPorts()
        {
            serialPorts = new List<string>();
            foreach (String port in SerialPort.GetPortNames())
            {
                serialPorts.Add(port);
            }
        }

       
        public string GetSerialPort()
        {
            string conselect = string.Empty;
            Console.WriteLine("Select com port:");
            if (serialPorts.Count == 0)
            {
                Console.WriteLine("No Serial Connections identified");
                return string.Empty;
            }
            foreach (string s in serialPorts)
            {
                Console.WriteLine(s);
            }
            conselect = Console.ReadLine();
            return conselect.ToString().Trim();
        }

        /* connect to a serial port*/
        public void ConnectSerialPort(string com, int braud)
        {
            serialPort = new SerialPort(com, braud, Parity.None, 8, StopBits.One);
            serialPort.Handshake = Handshake.None;
            /*register handler for data received from serial console*/
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serialPort.WriteTimeout = 1000000;
            serialPort.Open();
        }

        /* handle for data received from serial port*/
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.Write(indata);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        /*write commands to serial console*/
        public void WriteSerial()
        {
            try
            {
                if (serialPort != null)
                {
                    /*************** TEST CASES TO BE EXECUTED********/
                    /******CREATE YOUR OWN TEST ARCHITECTURE*********/
                    //serialPort.WriteLine(Console.ReadLine().ToString());
                    Test coexTest = new Test();

                    string dir = "C:\\Crash_Dump\\test";
                    for (int i = 1; i < 5; i++)
                    {
                        if(i == 1)
                        {
                            string test_1 = "C:\\Crash_Dump\\test_oev1";
                            if (System.IO.File.Exists(dir + "\\wifi_fw_ubi.img"))
                            {
                                System.IO.File.Delete(dir + "\\wifi_fw_ubi.img");
                            }
                            System.IO.File.Copy(test_1 + "\\wifi_fw_ubi.img", dir + "\\wifi_fw_ubi.img");

                            if (System.IO.File.Exists(dir + "\\noload.bin"))
                            {
                                System.IO.File.Delete(dir + "\\noload.bin");
                            }
                            System.IO.File.Copy(test_1 + "\\noload.bin", dir + "\\noload.bin");

                            if (System.IO.File.Exists(dir + "\\init_q6.bin"))
                            {
                                System.IO.File.Delete(dir + "\\init_q6.bin");
                            }
                            System.IO.File.Copy(test_1 + "\\init_q6.bin", dir + "\\init_q6.bin");

                            if (System.IO.File.Exists(dir + "\\fwbin.elf"))
                            {
                                System.IO.File.Delete(dir + "\\fwbin.elf");
                            }
                            System.IO.File.Copy(test_1 + "\\fwbin.elf", dir + "\\fwbin.elf");

                        }
                        else if(i == 9)
                        {
                            string test_9 = "C:\\Crash_Dump\\test_9";
                            if (System.IO.File.Exists(dir + "\\wifi_fw_ubi.img"))
                            {
                                System.IO.File.Delete(dir + "\\wifi_fw_ubi.img");
                            }
                            System.IO.File.Copy(test_9 + "\\wifi_fw_ubi.img", dir + "\\wifi_fw_ubi.img");

                            if (System.IO.File.Exists(dir + "\\noload.bin"))
                            {
                                System.IO.File.Delete(dir + "\\noload.bin");
                            }
                            System.IO.File.Copy(test_9 + "\\noload.bin", dir + "\\noload.bin");

                            if (System.IO.File.Exists(dir + "\\init_q6.bin"))
                            {
                                System.IO.File.Delete(dir + "\\init_q6.bin");
                            }
                            System.IO.File.Copy(test_9 + "\\init_q6.bin", dir + "\\init_q6.bin");

                            if (System.IO.File.Exists(dir + "\\fwbin.elf"))
                            {
                                System.IO.File.Delete(dir + "\\fwbin.elf");
                            }
                            System.IO.File.Copy(test_9 + "\\fwbin.elf", dir + "\\fwbin.elf");
                        }
                        coexTest.CoexEventCollectionTest(serialPort);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        public void CloseSerial()
        {
            if (this.serialPort != null)
            {
                this.serialPort.Close();
            }
        }

        /* Entry point of program*/
        public static void Main(string[] args)
        {
            AutoTest com = new AutoTest();
            com.FindSerialPorts();
            string port = com.GetSerialPort();
            if (port != string.Empty)
            {
                com.ConnectSerialPort(port, 115200);
            }

            Thread writeThread = new Thread(com.WriteSerial);

            writeThread.Start();
            writeThread.Join();
            Console.ReadLine();
            return;
        }
    }
}


