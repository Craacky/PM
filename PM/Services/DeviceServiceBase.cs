using PM.DAL.Entities;
using PM.Devices;
using PM.Devices.Base;
using PM.Models;
using PM.Protocols.Socket;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;

namespace PM.Services
{
    public class DeviceServiceBase : IDeviceService
    {
        protected int countBox = 0;
        protected int countSlvae = 0;
        protected int countMaster = 0;

        public ICameraDeviceService ProductMasterCameraService { get; set; }
        public ICameraDeviceService ProductSlaveCameraService { get; set; }
        public ICameraDeviceService BoxCameraService { get; set; }
        public IPrinterDeviceService BoxPrinterService { get; set; }
        public IPrinterDeviceService PalletPrinterService { get; set; }
        public ObservableCollection<Device> Devices { get; set; }
        public LocalDBService LocalDBService { get; set; }
        public ProcessingCodeService ProcessingCodeService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public ISettingsService SettingsService { get; set; }

        protected int countBoxInPallet;
        protected int countProductInBox;

        public DeviceServiceBase(ISettingsService settingsService,
                                 ReportTaskService reportTaskService,
                                 LocalDBService localDBService,
                                 ProcessingCodeService processingCodeService)
        {
            SettingsService = settingsService;
            ReportTaskService = reportTaskService;
            LocalDBService = localDBService;
            ProcessingCodeService = processingCodeService;

            CreateDevice();
        }

        public virtual void CreateDevice()
        {
        }


        public void ConnectDevices()
        {
            ProductMasterCameraService?.ConnectAsync();
            ProductSlaveCameraService?.ConnectAsync();
            BoxCameraService?.ConnectAsync();
            BoxPrinterService?.ConnectAsync();
            PalletPrinterService?.ConnectAsync();
        }

        public void DisconnectDevices()
        {
            ProductMasterCameraService?.Disconnect();
            ProductSlaveCameraService?.Disconnect();
            BoxCameraService?.Disconnect();
            BoxPrinterService?.Disconnect();
            PalletPrinterService?.Disconnect();
        }
        public void StartDevices()
        {
            countBox = 0;
            countSlvae = 0;
            countMaster = 0;

            countBoxInPallet = Convert.ToInt32(ReportTaskService.CurrentReportTask.CountBoxInPallet);
            countProductInBox = Convert.ToInt32(ReportTaskService.CurrentReportTask.CountProductInBox);

            FindedDevice();

            ProductMasterCameraService?.Start();
            ProductSlaveCameraService?.Start();
            BoxCameraService?.Start();
            BoxPrinterService?.Start();
            PalletPrinterService?.Start();

            if (ReportTaskService.Statistic.PalletCodes.Count == 0 || ReportTaskService.Statistic.PalletCodes[^1].IsFulled)
            {
                ReportTaskService.GeneratePalletCode();
            }
        }

        public void StopDevices()
        {
            ProductMasterCameraService?.Stop();
            ProductSlaveCameraService?.Stop();
            BoxCameraService?.Stop();
            BoxPrinterService?.Stop();
            PalletPrinterService?.Stop();
        }
        public void LostedDevice()
      {
            ProductMasterCameraService?.SendCommandLostDevice();
            ProductSlaveCameraService?.SendCommandLostDevice();
            BoxCameraService?.SendCommandLostDevice();
        }
        public void FindedDevice()
        {
            if (ProductMasterCameraService != null && (!ProductMasterCameraService.Device.IsUsed || ProductMasterCameraService.Device.IsConnected) &&
                ProductSlaveCameraService != null && (!ProductSlaveCameraService.Device.IsUsed || ProductSlaveCameraService.Device.IsConnected) &&
                BoxCameraService != null && (!BoxCameraService.Device.IsUsed || BoxCameraService.Device.IsConnected) &&
                BoxPrinterService != null && (!BoxPrinterService.Device.IsUsed || BoxPrinterService.Device.IsConnected) &&
                PalletPrinterService != null && (!PalletPrinterService.Device.IsUsed || PalletPrinterService.Device.IsConnected) &&
                LocalDBService != null && (!LocalDBService.Device.IsUsed || LocalDBService.Device.IsConnected))
            {
                ProductSlaveCameraService?.SendCommandFindDevice();
                ProductMasterCameraService?.SendCommandFindDevice();
                BoxCameraService?.SendCommandFindDevice();
            }
        }
        public void ChangeSetup(int numberSetup)
        {
            ProductMasterCameraService?.SendCommandChangeSetup(numberSetup);
            ProductSlaveCameraService?.SendCommandChangeSetup(numberSetup);
            BoxCameraService?.SendCommandChangeSetup(numberSetup);
        }
    }
}
