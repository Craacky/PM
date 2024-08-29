using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Tauras6L.Services.Devices
{
    public class BoxCameraService : CameraDeviceService, ICameraDeviceService
    {
        public BoxCameraService(DeviceSettings deviceSettings,
                                LineSettings lineSettings) : base(deviceSettings, lineSettings)
        { }

        protected override void CheskConnectionStateAsync()
        {
        }
    }
}
