using PM.DAL.Entities;
using PM.Devices;
using PM.Models;
using PM.Protocols.Socket;
using PM.Services;
using PM.Tauras4L.Services.Devices;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace PM.Tauras4L.Services
{
    public class DeviceService : DeviceServiceBase, IDeviceService
    {
        public DeviceService(ISettingsService settingsService,
                             ReportTaskService reportTaskService,
                             LocalDBService localDBService,
                             ProcessingCodeService processingCodeService) : base(settingsService, reportTaskService, localDBService, processingCodeService)
        {
        }

        public override void CreateDevice()
        {
            StopDevices();
            DisconnectDevices();

            ProductMasterCameraService = null;
            ProductSlaveCameraService = null;
            BoxCameraService = null;
            BoxPrinterService = null;
            PalletPrinterService = null;

            Devices = new ObservableCollection<Device>();

            if (Settings.LocalDB.IsUsed)
            {
                Devices.Add(LocalDBService.Device);
                LocalDBService.ConnectionChanged += LocalDBService_ConnectionChanged;
            }

            ProductMasterCameraService = new ProductMasterCameraService(SettingsService.Settings.ProductCameraMaster,
                                                            SettingsService.Settings.Line);
            if (SettingsService.Settings.ProductCameraMaster.IsUsed)
            {

                Devices.Add(ProductMasterCameraService.Device);
                ((ProductMasterCameraService)ProductMasterCameraService).ConnectionChanged += CameraService_ConnectionChanged;
                ((ProductMasterCameraService)ProductMasterCameraService).MessageReceived += ProductMasterCameraService_MessageReceived;
            }

            ProductSlaveCameraService = new ProductSlaveCameraService(SettingsService.Settings.ProductCameraSlave,
                                                                      SettingsService.Settings.Line);
            if (SettingsService.Settings.ProductCameraSlave.IsUsed)
            {

                Devices.Add(ProductSlaveCameraService.Device);
                ((ProductSlaveCameraService)ProductSlaveCameraService).ConnectionChanged += CameraService_ConnectionChanged;
                ((ProductSlaveCameraService)ProductSlaveCameraService).MessageReceived += ProductSlaveCameraService_MessageReceived;
            }

            BoxCameraService = new BoxCameraService(SettingsService.Settings.BoxCamera,
                                                    SettingsService.Settings.Line);
            if (SettingsService.Settings.BoxCamera.IsUsed)
            {

                Devices.Add(BoxCameraService.Device);
                ((BoxCameraService)BoxCameraService).ConnectionChanged += CameraService_ConnectionChanged;
                ((BoxCameraService)BoxCameraService).MessageReceived += BoxCameraService_MessageReceived;
            }

            BoxPrinterService = new BoxPrinterService(SettingsService.Settings.BoxPrinter,
                                                          SettingsService.Settings.Line,
                                                          LocalDBService,
                                                          ReportTaskService);
            if (SettingsService.Settings.BoxPrinter.IsUsed)
            {

                Devices.Add(BoxPrinterService.Device);
                ((BoxPrinterService)BoxPrinterService).ConnectionChanged += PrinterDeviceService_ConnectionChanged;
            }


            PalletPrinterService = new PalletPrinterService(SettingsService.Settings.PalletPrinter,
                                                               SettingsService.Settings.Line,
                                                               LocalDBService,
                                                               ReportTaskService);
            if (SettingsService.Settings.PalletPrinter.IsUsed)
            {

                Devices.Add(PalletPrinterService.Device);
                ((PalletPrinterService)PalletPrinterService).ConnectionChanged += PrinterDeviceService_ConnectionChanged;
            }

            ConnectDevices();
            FindedDevice();
        }

        private void LocalDBService_ConnectionChanged(DAL.EFCore.DBContext db, System.DateTime datetime, DAL.EFCore.ConnectionState connectionState)
        {
            if (db.IsConnected)
            {
                FindedDevice();
            }
            else
            {
                LostedDevice();
            }
        }
        private void CameraService_ConnectionChanged(SocketClient client, System.DateTime datetime, Protocols.Socket.ConnectionState connectionState)
        {
            if (client.IsConnected)
            {
                FindedDevice();
            }
            else
            {
                LostedDevice();
            }
        }
        private void PrinterDeviceService_ConnectionChanged(SocketClient client, DateTime datetime, ConnectionState connectionState)
        {
            if (client.IsConnected)
            {
                FindedDevice();
            }
            else
            {
                LostedDevice();
            }
        }
        bool isErrorState = false;

        private void BoxCameraService_MessageReceived(Protocols.Socket.SocketClient client, System.DateTime datetime, string message)
        {
            string startPattern = "<start>";
            string stopPattern = "<stop>";
            string failPattern = "fail";
            string nextPattern = "<next>";

            bool isCorrectMessage = message.Contains(startPattern) && message.Contains(stopPattern);
            if (isCorrectMessage)
            {

                if (ReportTaskService.Statistic.BoxCodes.Count != 0)

                {   //Если триггер сработал дважды
                    Box currentBox = ReportTaskService.Statistic.BoxCodes[^1];
                    int index = ReportTaskService.Statistic.BoxCodes.Count;
                    if (currentBox.Products.Count != 12)
                    {
                        LocalDBService.BoxDataService.Delete(currentBox.Id);
                        ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                        int countDeleted = ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                        ReportTaskService.Statistic.CountProducts -= countDeleted;
                        return;
                    }

                }


                bool isFaiLMessage = message.Contains(failPattern);
                if (isFaiLMessage)
                {
                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult() { BoxCameraReadingResult = $"Несчитано" });
                    return;
                }

                string[] batches = message.Split(startPattern, StringSplitOptions.RemoveEmptyEntries);
                foreach (string batch in batches)
                {
                    if (batch.Contains(stopPattern))
                    {
                        string markingCode = batch.Split(stopPattern)[0];


                        bool isCurrentTaskCode = ProcessingCodeService.IsBoxCodeTheCurrentTask(markingCode);
                        if (!isCurrentTaskCode)
                        {
                            isErrorState = true;

                            ProductMasterCameraService.SendCommandLostDevice();
                            StopDevices();

                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult() { BoxCameraReadingResult = $"Несоответсвие кода короба {markingCode}" });
                            
                            MessageBox.Show("Несоответсвие кода короба.\n" +
                                            $"{markingCode}\n" +
                                             "Короб не будет добавлен.\n" +
                                            "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                            StartDevices();
                            ProductMasterCameraService.SendCommandFindDevice();


                            isErrorState = false;
                            return;
                        }

                        bool isRepeateCode = ProcessingCodeService.IsRepeatBoxCode(markingCode);
                        if (isRepeateCode)
                        {
                            isErrorState = true;

                            ProductMasterCameraService.SendCommandLostDevice();
                            StopDevices();

                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult() { BoxCameraReadingResult = $"Повтор кода короба {markingCode}" });
                   
                            MessageBox.Show("Повтор кода короба.\n" +
                                              $"{markingCode}\n" +
                                              "Короб не будет добавлен.\n" +
                                              "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                            StartDevices();
                            ProductMasterCameraService.SendCommandFindDevice();


                            isErrorState = false;
                            return;
                        }


                        if (ReportTaskService.Statistic.PalletCodes.Count == 0 || ReportTaskService.Statistic.PalletCodes[^1].IsFulled)
                        {
                            ReportTaskService.GeneratePalletCode();
                        }

                        Box newBox = new Box
                        {
                            LineId = SettingsService.Settings.Line.LineId,
                            MarkingCode = markingCode,
                            ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                            PalletId = ReportTaskService.Statistic.PalletCodes[^1].Id
                        };

                        try
                        {
                            newBox = LocalDBService.BoxDataService.Create(newBox);

                        }
                        catch (Exception ex)
                        {
                            isErrorState = true;
                            Thread.Sleep(400);
                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                            ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Короб";
                            isErrorState = false;

                            return;
                        }
                        ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                        ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Короб {newBox.MarkingCode}";
                        
                        ReportTaskService.Statistic.BoxCodes.Add(newBox);

                        countBox++;
                    }
                }
            }
        }

        private void ProductSlaveCameraService_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
            string startPattern = "<start>";
            string stopPattern = "<stop>";
            string failPattern = "fail";
            string nextPattern = "<next>";

            bool isCorrectMessage = message.Contains(startPattern) && message.Contains(stopPattern);
            if (isCorrectMessage)
            {
                if (isErrorState)
                {
                    return;
                }

                if (ReportTaskService.Statistic.BoxCodes.Count == 0)
                    return;

                int j = 10;
                while (countBox <= countSlvae && j != 0)
                {
                    Thread.Sleep(100);
                    j--;

                    if (isErrorState)
                    {
                        return;
                    }

                }

                Box currentBox = ReportTaskService.Statistic.BoxCodes[^1];
                int index = ReportTaskService.Statistic.BoxCodes.Count;
                
                if (currentBox.Products.Count != 0)
                {
                    return;
                }

                bool isFaiLMessage = message.Contains(failPattern);
                if (isFaiLMessage)
                {
                    if (currentBox != null)
                    {
                        LocalDBService.BoxDataService.Delete(currentBox.Id);
                        ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                    }

                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                    ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                    ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Несчитано";

                    countSlvae++;
                    return;
                }

                string[] codes = message.Split("<start>")[1].Split("<stop>")[0].Split("<next>", StringSplitOptions.RemoveEmptyEntries);
                foreach (var code in codes)
                {
                    Product newProduct = new()
                    {
                        MarkingCode = code,
                        LineId = SettingsService.Settings.Line.LineId,
                        ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                        BoxId = currentBox?.Id
                    };


                    bool isCurrenttaskCode = ProcessingCodeService.IsProductCodeTheCurrentTask(code);
                    if (!isCurrenttaskCode)
                    {
                        isErrorState = true;

                        countSlvae++;

                        ProductMasterCameraService.SendCommandLostDevice();
                        StopDevices();

                        if (currentBox != null)
                        {
                            LocalDBService.BoxDataService.Delete(currentBox.Id);
                            ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                            int countDeleted = ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                            ReportTaskService.Statistic.CountProducts -= countDeleted;
                        }

                        MessageBox.Show("Несоответсвие кода продукта.\n" +
                                        $"{code}\n" +
                                        "Короб не будет добавлен.\n" +
                                        "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);


                        ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                        ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                        ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Несоответсвие кода";


                        StartDevices();
                        ProductMasterCameraService.SendCommandFindDevice();

                        isErrorState = false;
                        return;
                    }

                    bool isRepeateCode = ProcessingCodeService.IsRepeatProductCode(code);
                    if (isRepeateCode)
                    {
                        isErrorState = true;

                        countSlvae++;
                        ProductMasterCameraService.SendCommandLostDevice();
                        StopDevices();

                        if (currentBox != null)
                        {
                            LocalDBService.BoxDataService.Delete(currentBox.Id);
                            ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                            int countDeleted = ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                            ReportTaskService.Statistic.CountProducts -= countDeleted;
                        }

                        MessageBox.Show("Повтор кода продукта.\n" +
                                        $"{code}\n" +
                                        "Короб не будет добавлен.\n" +
                                        "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);


                        ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                        ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                        ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Повтор кода";

                        StartDevices();
                        ProductMasterCameraService.SendCommandFindDevice();

                        isErrorState = false;

                        return;
                    }

                    newProduct = LocalDBService.ProductDataService.Create(newProduct);
                    ReportTaskService.Statistic.ProductCodes.Add(newProduct);
                    ReportTaskService.Statistic.BoxCodes[index - 1].Products.Add(newProduct);
                }

                ReportTaskService.Statistic.CountProducts += 6;
                countSlvae++;
            }
        }

        private void ProductMasterCameraService_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
            new Thread(() =>
            {
                string startPattern = "<start>";
                string stopPattern = "<stop>";
                string failPattern = "fail";
                string nextPattern = "<next>";

                bool isCorrectMessage = message.Contains(startPattern) && message.Contains(stopPattern);
                if (isCorrectMessage)
                {
                    if (isErrorState)
                    {
                        return;
                    }

                    if (ReportTaskService.Statistic.BoxCodes.Count == 0)
                        return;

                    int j = 10;
                    while (countMaster >= countSlvae && j != 0)
                    {
                        Thread.Sleep(200);
                        j--;

                        if (isErrorState)
                        {
                            return;
                        }
                    }

                    Box currentBox = ReportTaskService.Statistic.BoxCodes[^1];
                    int index = ReportTaskService.Statistic.BoxCodes.Count;
                    if (currentBox.Products.Count != 6)
                    {
                        ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                        ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Не успело";

                        string commandSetLostDevice = "||>OUTPUT.USER2\r\n";
                        client.SendMessage(commandSetLostDevice);

                        return;
                    }

                    bool isFaiLMessage = message.Contains(failPattern);
                    if (isFaiLMessage)
                    {
                        if (currentBox != null)
                        {
                            LocalDBService.BoxDataService.Delete(currentBox.Id);
                            ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                            ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                            ReportTaskService.Statistic.CountProducts -= 6;
                        }

                        ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                        ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                        ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Считано";
                        ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraMasterReadingResult = "Несчитано";

                        countMaster++;
                        return;
                    }

                    string[] codes = message.Split("<start>")[1].Split("<stop>")[0].Split("<next>", StringSplitOptions.RemoveEmptyEntries);
                    foreach (var code in codes)
                    {
                        Product newProduct = new()
                        {
                            MarkingCode = code,
                            LineId = SettingsService.Settings.Line.LineId,
                            ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                            BoxId = currentBox?.Id
                        };

                        bool isCurrenttaskCode = ProcessingCodeService.IsProductCodeTheCurrentTask(code);
                        if (!isCurrenttaskCode)
                        {
                            isErrorState = true;
                            ProductMasterCameraService.SendCommandLostDevice();
                            StopDevices();

                            if (currentBox != null)
                            {
                                LocalDBService.BoxDataService.Delete(currentBox.Id);
                                ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                                int countDeleted = ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                                ReportTaskService.Statistic.CountProducts -= 6;
                            }

   
                            MessageBox.Show("Несоответсвие кода продукта.\n" +
                                            $"{code}\n" +
                                            "Короб не будет добавлен.\n" +
                                            "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                            ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                            ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Считано";
                            ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraMasterReadingResult = "Несоответсвие кода";

                            countMaster++;

                            StartDevices();
                            ProductMasterCameraService.SendCommandFindDevice();


                            isErrorState = false;
                            return;
                        }


                        bool isRepeateCode = ProcessingCodeService.IsRepeatProductCode(code);
                        if (isRepeateCode)
                        {
                            isErrorState = true;
                            ProductMasterCameraService.SendCommandLostDevice();
                            StopDevices();

                            if (currentBox != null)
                            {
                                LocalDBService.BoxDataService.Delete(currentBox.Id);
                                ReportTaskService.Statistic.BoxCodes.Remove(currentBox);
                                int countDeleted = ReportTaskService.Statistic.ProductCodes.RemoveAll(p => p.BoxId == currentBox.Id);
                                ReportTaskService.Statistic.CountProducts -= 6;
                            }

                            MessageBox.Show("Повтор кода продукта.\n" +
                                            $"{code}\n" +
                                            "Короб не будет добавлен.\n" +
                                            "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                            ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Удалено {currentBox?.MarkingCode}";
                            ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Считано";
                            ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraMasterReadingResult = "Повтор кода";

                            countMaster++;

                            StartDevices();
                            ProductMasterCameraService.SendCommandFindDevice();
                            isErrorState = false;

                            return;
                        }

                        newProduct = LocalDBService.ProductDataService.Create(newProduct);
                        ReportTaskService.Statistic.ProductCodes.Add(newProduct);
                        ReportTaskService.Statistic.BoxCodes[index - 1].Products.Add(newProduct);
                    }

                    {
                        string commandSetLostDevice = "||>OUTPUT.USER1\r\n";
                        client.SendMessage(commandSetLostDevice);
                    }

                    ReportTaskService.Statistic.CountProducts += 6;
                    countMaster++;

                    ReportTaskService.Statistic.CountBoxInCurrentPallet++;
                    ReportTaskService.Statistic.CountBoxes++;

                    if (ReportTaskService.Statistic.CountBoxInCurrentPallet % countBoxInPallet == 0)
                    {
                        ReportTaskService.ClosePallet();
                        PalletPrinterService?.PrintCode();
                    }

                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult());
                    ReportTaskService.Statistic.CameraReadingResults[0].BoxCameraReadingResult = $"Считано {currentBox?.MarkingCode}";
                    ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraSlaveReadingResult = "Считано";
                    ReportTaskService.Statistic.CameraReadingResults[0].ProductCameraMasterReadingResult = "Считано";
                }
            })
            { IsBackground = true }.Start();
        }
    }
}


