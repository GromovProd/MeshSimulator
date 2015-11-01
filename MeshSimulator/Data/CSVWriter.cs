using MeshSimulator.Model;
using MeshSimulator.Model.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimulator.Data
{
    public static class CSVWriter
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory;

        private static string fileReportName = "";

        public static void GenerateReport(ModelVariables variables)
        {
            fileReportName = "Report" + GenerateReportNameByTime() + ".csv";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Time: {0}\nCount of stations: {1}\nWidth: {2}\nHeight: {3}\nEmulation time: {4}\n",
                DateTime.Now, variables.CountOfReports, variables.Width, variables.Height, variables.EndTime);
            sb.AppendFormat("Connection radius: {0}\nCycles in supercycle: {1}\nSlot time: {2}\nPacket transmition time: {3}\n", variables.ConnectionRadius, variables.CyclesInSuperCycle, variables.SlotTimeMilliSeconds, variables.PacketTransmitTime);

            sb.Append("Id; Emulation time; Global time; Messages sended; Messages recieved; Efficiency\n");

            var text = sb.ToString();

            File.WriteAllText(path + fileReportName, text, Encoding.UTF8);
        }

        private static string GenerateReportNameByTime()
        {
            return DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second;
        }

        public static void WriteReport(Report report)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0};{1};{2};{3};{4};{5}\n", report.Id, report.EmulationTime, report.GlobalTime, report.MessagesRecieved, report.MessagesSended, report.Efficiency);

            var text = sb.ToString();

            File.AppendAllText(path + fileReportName, text, Encoding.UTF8);
        }
    }
}
