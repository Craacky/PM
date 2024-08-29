using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Tauras6L.Services.Devices
{
    public class ProductMasterCameraService : CameraDeviceService, ICameraDeviceService
    {
        public ProductMasterCameraService(DeviceSettings deviceSettings,
                                          LineSettings lineSettings) : base(deviceSettings, lineSettings)
        { }

        public override void SendCommandFindDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetFindDevice = "||>SET OUTPUT.ACTION 3 1\r\n";
                _client.SendMessage(commandSetFindDevice);
            }
        }
        public override void SendCommandLostDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetLostDevice = "||>SET OUTPUT.ACTION 3 0\r\n";
                _client.SendMessage(commandSetLostDevice);
            }
        }
    }
}
