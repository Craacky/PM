using PM.Models;
using PM.Services;
using PM.DAL.Entities;

namespace PM.Devices.Base
{
    public interface IPrinterDeviceService
    {
        Device Device { get; set; }


        DeviceSettings DeviceSettings { get; set; }
        LineSettings LineSettings { get; set; }
        LocalDBService LocalDBService { get; set; }
        ReportTaskService ReportTaskService { get; set; }


        event PrinterDeviceService.ConnectionHandler ConnectionChanged;
        event PrinterDeviceService.MessageHandler MessageReceived;


        void ConnectAsync();
        void Disconnect();
        void LoadTemplates();
        void PrintCode();
        void RepeatePrinteCode();
        void SendMessage(string message);
        void Start();
        void Stop();
    }
}