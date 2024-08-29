using PM.DAL.Entities;
using PM.Devices;
using PM.Devices.Base;
using PM.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace PM.Mixter.Services.Devices
{
    public partial class PalletPrinterService : PrinterDeviceService, IPrinterDeviceService
    {
        public PalletPrinterService(DeviceSettings deviceSettings,
                                    LineSettings lineSettings,
                                    LocalDBService localDBService,
                                    ReportTaskService reportTaskService) : base(deviceSettings, lineSettings, localDBService, reportTaskService)
        {

        }

        public override void PrintCode()
        {
            if (Device.IsConnected && _isRun)
            {
                string messageToSend = patternMessage;
                messageToSend = messageToSend.Replace("<SERIAL>", $"{ReportTaskService.Statistic.PalletCodes.Count}");

                _client.SendMessage(messageToSend);
            }
        }
        public override void LoadTemplates()
        {
            if (Device.IsConnected && _isRun)
            {
                LoadPatternTask();
                LoadPatternMessage();
            }
        }

        protected override void SendCommandToClearTask()
        {
            //string commandToClearTask = $"{(char)1}FGA---r--------{(char)23}\r\n";
            //_client.SendMessage(commandToClearTask);
        }
        protected override void SendCommadToSetAutoStatus()
        {
            //string commandToSetAutoStatus = $"\u0027!S\r\n";
            //_client.SendMessage(commandToSetAutoStatus);
        }

        private void LoadPatternTask()
        {
            DAL.Entities.ReportTask reportTask = LocalDBService.ReportTaskDataService.GetWithInclude(r => r.Guid == ReportTaskService.CurrentReportTask.Guid, rt => rt.Nomenclature);
            List<DAL.Entities.Attribute> attributes = LocalDBService.AttributeDataService.GetAll(a => a.NomenclatureId == reportTask.NomenclatureId).ToList();

            if (Device.IsConnected && !File.Exists("TSCLABEL.txt"))
            {
                MessageBox.Show("Отсутствует файл с шаблоном печати. Печать невозможна.");
                Stop();
            }
            else
            {
                StreamReader reader = new("TSCLABEL.txt", System.Text.Encoding.UTF8);
                patternTask = reader.ReadToEnd();
                patternTask = patternTask.Replace("<NAME>", reportTask.Nomenclature.Name.Replace("\"", "\\[\"]"));
                patternTask = patternTask.Replace("<MARK>", attributes[0].Value.Replace("\"", "\\[\"]"));
                patternTask = patternTask.Replace("<STB>", attributes[7].Value);
                patternTask = patternTask.Replace("<NETTO>", attributes[8].Value);

                patternTask = patternTask.Replace("<BRUTTO>", attributes[9].Value);
                patternTask = patternTask.Replace("<NETTOP>", attributes[13].Value);
                patternTask = patternTask.Replace("<NETTOG>", attributes[13].Value);
                string minTemp = attributes[16].Value;
                string maxTemp = attributes[17].Value;
                patternTask = patternTask.Replace("<CONDITION>", $"Хранить при температуре от {minTemp} до {maxTemp}");
                patternTask = patternTask.Replace("<MINT>", $"{minTemp}");
                patternTask = patternTask.Replace("<MAXT>", $"{maxTemp}");

                patternTask = patternTask.Replace("<BATCH>", string.Format("{0:0000}", ReportTaskService.CurrentReportTask.LotNumber));

                patternTask = patternTask.Replace("<SDATE>", ReportTaskService.CurrentReportTask.ManufactureDate.ToString("dd.MM.yy"));
                patternTask = patternTask.Replace("<EDATE>", ReportTaskService.CurrentReportTask.ExpiryDate.ToString("dd.MM.yy"));

                patternTask = patternTask.Replace("<SCODEDATE>", ReportTaskService.CurrentReportTask.ManufactureDate.ToString("yyMMdd"));
                patternTask = patternTask.Replace("<ECODEDATE>", ReportTaskService.CurrentReportTask.ExpiryDate.ToString("yyMMdd"));
                patternTask = patternTask.Replace("<ITEMS>", Convert.ToString(ReportTaskService.CurrentReportTask.CountBoxInPallet));
                patternTask = patternTask.Replace("<GTIN>", ReportTaskService.CurrentReportTask.Nomenclature.Gtin);

                _client.SendMessage(patternTask);
            }
        }
        private void LoadPatternMessage()
        {
            if (Device.IsConnected && !File.Exists("TSCPrint.txt"))
            {
                MessageBox.Show("Отсутствует файл с шаблоном печати. Печать невозможна.");
                Disconnect();
            }
            else
            {
                StreamReader reader = new("TSCPrint.txt", System.Text.Encoding.UTF8);
                patternMessage = reader.ReadToEnd();

                patternMessage = patternMessage.Replace("<SIZE>", "2");
            }
        }
    }
}
