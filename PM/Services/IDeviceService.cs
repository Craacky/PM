using PM.Devices;
using PM.Devices.Base;
using PM.Models;
using System.Collections.ObjectModel;

namespace PM.Services
{
    public interface IDeviceService
    {
        ICameraDeviceService ProductMasterCameraService { get; set; }
        ICameraDeviceService ProductSlaveCameraService { get; set; }
        ICameraDeviceService BoxCameraService { get; set; }
        IPrinterDeviceService BoxPrinterService { get; set; }
        IPrinterDeviceService PalletPrinterService { get; set; }


        ObservableCollection<Device> Devices { get; set; }


        LocalDBService LocalDBService { get; set; }
        ProcessingCodeService ProcessingCodeService { get; set; }
        ReportTaskService ReportTaskService { get; set; }
        ISettingsService SettingsService { get; set; }


        void ChangeSetup(int numberSetup);
        void ConnectDevices();
        void DisconnectDevices();
        void FindedDevice();
        void LostedDevice();
        void StartDevices();
        void StopDevices();
        void CreateDevice();
    }
}