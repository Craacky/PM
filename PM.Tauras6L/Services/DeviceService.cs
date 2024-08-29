using PM.DAL.Entities;
using PM.Devices;
using PM.Models;
using PM.Protocols.Socket;
using PM.Services;
using PM.Tauras6L.Services.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PM.Tauras6L.Services
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


        private void BoxCameraService_MessageReceived(Protocols.Socket.SocketClient client, System.DateTime datetime, string message)
        {
        }

        private void ProductSlaveCameraService_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
        }

        private void ProductMasterCameraService_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
            Task.Run(() =>
            {
                string startPattern = "<start>";
                string stopPattern = "<stop>";
                string failPattern = "fail";
                string nextPattern = "<next>";

                bool isCorrectMessage = message.Contains(startPattern) && message.Contains(stopPattern);
                if (isCorrectMessage)
                {
                    string[] batches = message.Split(startPattern, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string batch in batches)
                    {
                        if (batch.Contains(stopPattern))
                        {
                            string part = batch.Split(stopPattern)[0];

                            bool isFaiLMessage = part.Contains(failPattern);
                            if (isFaiLMessage)
                            {
                                string[] decodeResults = part.Split(nextPattern);

                                if (Convert.ToInt32(decodeResults[1]) == 1)
                                {
                                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = "Считано", ProductCameraMasterReadingResult = "Несчитано", ProductCameraSlaveReadingResult = "Несчитано" });
                                }
                                else
                                {
                                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = "Несчитано" });
                                }

                                return;
                            }


                            string[] markingCodes = part.Split(nextPattern);

                            if (ReportTaskService.Statistic.PalletCodes.Count == 0 || ReportTaskService.Statistic.PalletCodes[^1].IsFulled)
                            {
                                ReportTaskService.GeneratePalletCode();
                            }

                            Box newBox = new Box
                            {
                                LineId = SettingsService.Settings.Line.LineId,
                                MarkingCode = markingCodes[0],
                                ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid,
                                PalletId = ReportTaskService.Statistic.PalletCodes[^1].Id
                            };


                            bool isBoxCodeTheCurrentTask = ProcessingCodeService.IsBoxCodeTheCurrentTask(markingCodes[0]);
                            if (!isBoxCodeTheCurrentTask)
                            {
                                string command = "||>SET OUTPUT.ACTION 1 0\r\n";
                                client.SendMessage(command);

                                Thread.Sleep(100);

                                //LostedDevice();
                                //StopDevices();

                                //MessageBoxResult result = MessageBox.Show("Код короба не соответствует коду короба текущего задания.\n" +
                                //                                           $"{markingCodes[0]}\n" +
                                //                                          "Короб не будет добавлен.\n" +
                                //                                          "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                                ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = $"Несоответсвие кода {markingCodes[0]}" });

                                //FindedDevice();
                                //StartDevices();

                                command = "||>SET OUTPUT.ACTION 1 1\r\n";
                                client.SendMessage(command);

                                return;
                            }

                            bool isRepeateBoxCode = ProcessingCodeService.IsRepeatBoxCode(markingCodes[0]);
                            if (isRepeateBoxCode)
                            {
                                string command = "||>SET OUTPUT.ACTION 1 0\r\n";
                                client.SendMessage(command);

                                Thread.Sleep(100);

                                //LostedDevice();
                                //StopDevices();

                                //MessageBoxResult result = MessageBox.Show("Повтор кода короба.\n" +
                                //                                           $"{markingCodes[0]}\n" +
                                //                                           "Короб не будет добавлен.\n" +
                                //                                          "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);


                                ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = $"Повтор кода {markingCodes[0]}" });

                                //FindedDevice();
                                //StartDevices();

                                command = "||>SET OUTPUT.ACTION 1 1\r\n";
                                client.SendMessage(command);

                                return;
                            }

                            List<Product> products = new List<Product>();

                            for (int i = 1; i < markingCodes.Length; i++)
                            {
                                Product newProduct = new()
                                {
                                    MarkingCode = markingCodes[i],
                                    BoxId = newBox.Id,
                                    LineId = SettingsService.Settings.Line.LineId,
                                    ReportTaskGuid = ReportTaskService.CurrentReportTask.Guid
                                };

                                bool isProductCodeTheCurrentTask = ProcessingCodeService.IsProductCodeTheCurrentTask(markingCodes[i]);
                                if (!isProductCodeTheCurrentTask)
                                {
                                    string command = "||>SET OUTPUT.ACTION 1 0\r\n";
                                    client.SendMessage(command);

                                    Thread.Sleep(100);

                                    //LostedDevice();
                                    //StopDevices();

                                    //MessageBoxResult result = MessageBox.Show("Код продукта не соответствует коду продукта текущего задания.\n" +
                                    //                                          $"{markingCodes[i]}\n" +
                                    //                                          "Короб не будет добавлен.\n" +
                                    //                                          "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = $"Считано {newBox.MarkingCode}", ProductCameraMasterReadingResult = $"Несоответствие кода {markingCodes[i]}" });

                                    command = "||>SET OUTPUT.ACTION 1 1\r\n";
                                    client.SendMessage(command);
                                    //StartDevices();
                                    //FindedDevice();

                                    return;
                                }

                                bool isRepeatProductCode = ProcessingCodeService.IsRepeatProductCode(markingCodes[i]);
                                bool isRepeatInCurrentBox = products.Any(p => p.MarkingCode.Replace("\u001d", "") == markingCodes[i].Replace("\u001d", ""));
                                if (isRepeatProductCode || isRepeatInCurrentBox)
                                {

                                    string command = "||>SET OUTPUT.ACTION 1 0\r\n";
                                    client.SendMessage(command);

                                    Thread.Sleep(100);

                                    //LostedDevice();
                                    //StopDevices();

                                    //MessageBoxResult result = MessageBox.Show("Повтор кода продукта.\n" +
                                    //                                          $"{markingCodes[i]}\n" +
                                    //                                          "Короб не будет добавлен.\n" +
                                    //                                          "Уберите с паллеты последний прошедший короб!!!", "Ошибка", MessageBoxButton.OK);

                                    ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = $"Считано {newBox.MarkingCode}", ProductCameraMasterReadingResult = $"Повтор кода {markingCodes[i]}" });

                                    command = "||>SET OUTPUT.ACTION 1 1\r\n";
                                    client.SendMessage(command);
                                    //StartDevices();
                                    //FindedDevice();


                                    return;
                                }

                                products.Add(newProduct);
                            }

                            newBox = LocalDBService.BoxDataService.Create(newBox);
                           
                            for (int i = 0; i < products.Count; i++)
                            {
                                products[i].BoxId = newBox.Id;
                                var newProduct = LocalDBService.ProductDataService.Create(products[i]);
                                ReportTaskService.Statistic.ProductCodes.Add(newProduct);
                            }

                            ReportTaskService.Statistic.CountProducts += products.Count;
                            ReportTaskService.Statistic.BoxCodes.Add(newBox);
                            ReportTaskService.Statistic.CountBoxes++;
                            ReportTaskService.Statistic.CountBoxInCurrentPallet++;


                            ReportTaskService.Statistic.CameraReadingResults.Insert(0, new СameraReadingResult { BoxCameraReadingResult = $"Считано {newBox.MarkingCode}", ProductCameraMasterReadingResult = "Cчитано", ProductCameraSlaveReadingResult = "Cчитано" });

                            if (ReportTaskService.Statistic.CountBoxInCurrentPallet % countBoxInPallet == 0)
                            {
                                ReportTaskService.ClosePallet();
                                PalletPrinterService?.PrintCode();
                            }
                        }
                    }
                }
            });
        }
    }
}


