using PM.DAL.Entities;
using PM.Models;

namespace PM.Devices.Base
{
    public interface ICameraDeviceService
    {
        Device Device { get; set; }
        DeviceSettings DeviceSettings { get; set; }
        LineSettings LineSettings { get; set; }

        event CameraDeviceService.ConnectionHandler ConnectionChanged;
        event CameraDeviceService.MessageHandler MessageReceived;

        void ConnectAsync();
        void Disconnect();
        void SendCommandChangeSetup(int numberSetup);
        void SendCommandFindDevice();
        void SendCommandLostDevice();
        void SendMessage(string message);
        void Start();
        void Stop();
    }
}