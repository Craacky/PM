using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Tauras6L.Services.Devices
{
    public class ProductSlaveCameraService : CameraDeviceService, ICameraDeviceService
    {
        public ProductSlaveCameraService(DeviceSettings deviceSettings,
                                         LineSettings lineSettings) : base(deviceSettings, lineSettings)
        { }

        public override void SendCommandLostDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetLostDevice = "||>SET OUTPUT.ACTION 0 0\r\n";
                _client.SendMessage(commandSetLostDevice);
            }
        }
        public override void SendCommandFindDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetFindDevice = "||>SET OUTPUT.ACTION 0 1\r\n";
                _client.SendMessage(commandSetFindDevice);
            }
        }
    }
}
