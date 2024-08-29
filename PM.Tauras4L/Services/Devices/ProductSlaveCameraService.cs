using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Tauras4L.Services.Devices
{
    public class ProductSlaveCameraService : CameraDeviceService, ICameraDeviceService
    {
        
        public ProductSlaveCameraService(DeviceSettings deviceSettings,
                                         LineSettings lineSettings) : base(deviceSettings, lineSettings)

        {}


        public override void SendCommandFindDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetFindDevice = "||>SET OUTPUT.ACTION 2 1\r\n";
                _client.SendMessage(commandSetFindDevice);
            }
        }
        public override void SendCommandLostDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetLostDevice = "||>SET OUTPUT.ACTION 2 0\r\n";
                _client.SendMessage(commandSetLostDevice);
            }
        }
    }
}
