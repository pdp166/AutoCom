using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using Renci.SshNet;
using System.Diagnostics;


namespace AutoCom
{
    class Test
    {
        private int count = 1;
        private int testcase = 0;

        /* to execute any shell scripts through SSH which acts as a side connection to existing serial connection*/
        public void SSHCommands()
        {
            using (var client = new SshClient("192.168.1.1", "root", "\n"))
            {
                client.Connect();
                client.RunCommand("/etc/BTCommands.sh &");
                Thread.Sleep(50000);
                client.Disconnect();
            }
        }

        public void DumpCollector()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "C:\\Users\\cdcapfw\\Desktop\\fw_Dump\\FWDump.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = "--O"+ " " + "C:\\Crash_Dump\\test_o"+ count.ToString();
            
            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
                count++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void CoexEventCollectionTest(SerialPort serialPort)
        {
            try
            {
                testcase++;
                
                TestCase reboot = new TestCase("reboot -f");
                TestCase uboot = new TestCase("\n");
                TestCase flashFW = new TestCase("run loadfw");
                TestCase configPTA = null;
                TestCase enableCoex = null;

                if (testcase == 1 || testcase == 2)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 2 0x80800505");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if (testcase == 1 || testcase == 2)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if (testcase == 3 || testcase == 4)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 1");
                }

#if (false)
                if (testcase == 1 || testcase == 2)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0x80800505");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if(testcase == 3 || testcase == 4)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if(testcase == 5 || testcase == 6)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0x80800505");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 1");
                }
                else if (testcase == 7 || testcase == 8)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 1");
                }
                else if (testcase == 9 || testcase == 10)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0x80800505");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if (testcase == 11 || testcase == 12)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 0");
                }
                else if (testcase == 13 || testcase == 14)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0x80800505");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 1");
                }
                else if (testcase == 15 || testcase == 16)
                {
                    configPTA = new TestCase("wifitool ath1 coex_cfg 17 0 2 0x18 0x12 1 0xEEEEEEEE");
                    enableCoex = new TestCase("wifitool ath1 coex_cfg 5 1 1");
                }
#endif

                TestCase setIP = new TestCase("ifconfig br-lan 192.168.1.2");

                TestCase stratIperClient = new TestCase("iperf -c 192.168.1.31 -u -t 50 -b 200M -i 1");
                TestCase assertAP = new TestCase("iwpriv wifi1 set_fw_hang 1");
                TestCase ping = new TestCase("ping 192.168.1.31");

                TestCase configSOCName = new TestCase("export BTHOST_8311_SOC_TYPE=cp01");
                TestCase openBTAPP = new TestCase("/usr/bin/LinuxSPPLE 1 /dev/ttyMSM1 115200");
                TestCase startAdvertise = new TestCase("advertisele 1");
                TestCase startScanning = new TestCase("startscanning");
                TestCase exitBT = new TestCase("quit");

                
                serialPort.Write("\n");
                serialPort.Write("\n");

                serialPort.WriteLine(reboot.GetTestCommand());

                if(testcase == 1 || testcase == 9)
                {
                    int count = 10;
                    while (count != 0)
                    {
                        Thread.Sleep(1000);
                        serialPort.Write("\n");
                        serialPort.Write("\n");
                        serialPort.Write("\n");
                        count--;

                    }
                    Thread.Sleep(10);
                    serialPort.WriteLine(flashFW.GetTestCommand());
                }
          
                Thread.Sleep(60000);
                serialPort.Write("\n");
                serialPort.Write("\n");
                serialPort.WriteLine(configPTA.GetTestCommand());
                serialPort.WriteLine(enableCoex.GetTestCommand());
                serialPort.Write("\n");
               

                Thread sshTh = new Thread(this.SSHCommands);
                sshTh.Start();

                Thread.Sleep(5000);
                serialPort.WriteLine(configSOCName.GetTestCommand());
                serialPort.WriteLine(openBTAPP.GetTestCommand());
                serialPort.WriteLine(startAdvertise.GetTestCommand());
                serialPort.WriteLine(startScanning.GetTestCommand());
                                
                sshTh.Join();

                Thread.Sleep(50);
                serialPort.WriteLine(exitBT.GetTestCommand());
                Thread.Sleep(500);
                serialPort.WriteLine(assertAP.GetTestCommand());
                Thread.Sleep(1200000);
                
                Thread dumpCollector = new Thread(DumpCollector);
                dumpCollector.Start();

                dumpCollector.Join();
                Thread.Sleep(200000);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            
        }

    }
}
