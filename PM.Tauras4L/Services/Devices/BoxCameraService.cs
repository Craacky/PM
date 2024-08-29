using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Tauras4L.Services.Devices
{
    public class BoxCameraService : CameraDeviceService, ICameraDeviceService
    {
        public BoxCameraService(DeviceSettings deviceSettings,
                                LineSettings lineSettings) : base(deviceSettings, lineSettings)

        { }

        public override void SendCommandFindDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetFindDevice = "||>SET OUTPUT.EVENTS 0 1 0 0 0 0 0 0 0\r\n" +
                                              "||>SET OUTPUT.EVENTS 1 0 1 0 0 0 0 0 0\r\n";
                _client.SendMessage(commandSetFindDevice);
            }
        }
        public override void SendCommandLostDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetLostDevice = "||>SET OUTPUT.EVENTS 0 0 0 0 0 0 0 0 0\r\n" +
                                              "||>SET OUTPUT.EVENTS 1 0 0 0 0 0 0 0 0\r\n";
                _client.SendMessage(commandSetLostDevice);
            }
        }
    }
}
