using PM.DAL.Entities;
using PM.Devices.Base;
using PM.Protocols.Socket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PM.Mixter.Services.Devices
{
    public class ProductSlaveCameraService : CameraDeviceService, ICameraDeviceService
    {
        public ProductSlaveCameraService(DeviceSettings deviceSettings,
                                         LineSettings lineSettings) : base(deviceSettings, lineSettings)
        { }
    }
}
