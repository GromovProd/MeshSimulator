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
    public static class ReportWriter
    {
        private static string path = AppDomain.CurrentDomain.BaseDirectory;

        private static string fileReportName = "";
        private static string fileInfoExpandReportName = "";

        public static void Init()
        {
            var folderName = "Reports_" + GenerateReportNameByTime()+"/";
            path += folderName;

            System.IO.Directory.CreateDirectory(folderName);
        }

        public static void GenerateReport(ModelVariables variables)
        {
            fileReportName = "Report.csv";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Time: {0}\nCount of stations: {1}\nWidth: {2}\nHeight: {3}\nEmulation time: {4}\n",
                DateTime.Now, variables.CountOfReports, variables.Width, variables.Height, variables.EndTime);
            sb.AppendFormat("Connection radius: {0}\nCycles in supercycle: {1}\nSlot time: {2}\nPacket transmition time: {3}\n", variables.ConnectionRadius, variables.CyclesInSuperCycle, variables.SlotTimeMilliSeconds, variables.PacketTransmitTime);

            sb.Append("Id; Emulation time; Global time; Messages sended; Messages recieved; Efficiency\n");

            var text = sb.ToString();

            File.WriteAllText(path + fileReportName, text, Encoding.UTF8);
        }

        public static void GenerateInfoExpandReport(ModelVariables variables)
        {
            fileInfoExpandReportName = "InfoExpandReport.csv";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Time: {0}\nCount of stations: {1}\nWidth: {2}\nHeight: {3}\nEmulation time: {4}\n",
                DateTime.Now, variables.CountOfReports, variables.Width, variables.Height, variables.EndTime);
            sb.AppendFormat("Connection radius: {0}\nCycles in supercycle: {1}\nSlot time: {2}\nPacket transmition time: {3}\n", variables.ConnectionRadius, variables.CyclesInSuperCycle, variables.SlotTimeMilliSeconds, variables.PacketTransmitTime);

            sb.Append("Id; Emulation time; Global time; Messages sended; Messages recieved; Efficiency; StationId\n");

            var text = sb.ToString();

            File.WriteAllText(path + fileInfoExpandReportName, text, Encoding.UTF8);
        }

        public static void GenerateFinishReport(ModelVariables variables, FinishReport report)
        {
            fileReportName = "FinishReport.csv";
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Time: {0}\nCount of stations: {1}\nWidth: {2}\nHeight: {3}\nEmulation time: {4}\n",
                DateTime.Now, variables.CountOfReports, variables.Width, variables.Height, variables.EndTime);
            sb.AppendFormat("Connection radius: {0}\nCycles in supercycle: {1}\nSlot time: {2}\nPacket transmition time: {3}\n", variables.ConnectionRadius, variables.CyclesInSuperCycle, variables.SlotTimeMilliSeconds, variables.PacketTransmitTime);

            for (int i = -1; i < report.StationsData.Keys.Count; i++)
            {
                if (i == -1)
                {
                    sb.Append("№ by № Average hocs;");
                }
                else
                {
                    sb.Append(i + ";");
                }
            }

            sb.Append("\n");

            for (int i = 0; i < report.StationsData.Keys.Count; i++)
            {
                var data = report.StationsData[i].OrderBy(z => z.Id).ToList();

                sb.Append(i + ";");

                for (int j = 0; j < report.StationsData.Keys.Count; j++)
                {
                    if (data.Exists(z => z.Id == j))
                    {
                        var value = data.Single(z => z.Id == j).AverageHoc;
                        sb.Append(value + ";");
                    }
                    else
                    {
                        sb.Append(";");
                    }

                }
                sb.Append("\n");
            }

            sb.Append("\n");

            for (int i = -1; i < report.StationsData.Keys.Count; i++)
            {
                if (i == -1)
                {
                    sb.Append("№ by № Average update time minutes;");
                }
                else
                {
                    sb.Append(i + ";");
                }
            }

            sb.Append("\n");

            for (int i = 0; i < report.StationsData.Keys.Count; i++)
            {
                var data = report.StationsData[i].OrderBy(z => z.Id).ToList();

                sb.Append(i + ";");

                for (int j = 0; j < report.StationsData.Keys.Count; j++)
                {
                    if (data.Exists(z => z.Id == j))
                    {
                        var value = data.Single(z => z.Id == j).AvarageUpdateTime.TotalMinutes;
                        sb.Append(value + ";");
                    }
                    else
                    {
                        sb.Append(";");
                    }
                }
                sb.Append("\n");
            }

            sb.Append("\n");

            for (int i = -1; i < report.StationsData.Keys.Count; i++)
            {
                if (i == -1)
                {
                    sb.Append("№ by № times recieve;");
                }
                else
                {
                    sb.Append(i + ";");
                }
            }

            sb.Append("\n");

            for (int i = 0; i < report.StationsData.Keys.Count; i++)
            {
                var data = report.StationsData[i].OrderBy(z => z.Id).ToList();

                sb.Append(i + ";");

                for (int j = 0; j < report.StationsData.Keys.Count; j++)
                {
                    if (data.Exists(z => z.Id == j))
                    {
                        var value = data.Single(z => z.Id == j).TimesRecieve;
                        sb.Append(value + ";");
                    }
                    else
                    {
                        sb.Append(";");
                    }
                }
                sb.Append("\n");
            }

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

        public static void WriteReport(InfoExpandReport report)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0};{1};{2};{3};{4};{5}\n", report.Id, report.EmulationTime, report.GlobalTime, report.MessagesRecieved, report.MessagesSended, report.Efficiency);

            var text = sb.ToString();

            File.AppendAllText(path + fileInfoExpandReportName, text, Encoding.UTF8);
        }
    }
}
