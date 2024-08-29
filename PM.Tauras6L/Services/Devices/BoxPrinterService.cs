using PM.DAL.Entities;
using PM.Devices;
using PM.Devices.Base;
using PM.Protocols.Socket;
using PM.Services;
using System;

namespace PM.Tauras6L.Services.Devices
{
    public class BoxPrinterService : PrinterDeviceService, IPrinterDeviceService
    {
        public BoxPrinterService(DeviceSettings deviceSettings,
                                 LineSettings lineSettings,
                                 LocalDBService localDBService,
                                 ReportTaskService reportTaskService) : base(deviceSettings, lineSettings, localDBService, reportTaskService)
        { }
    }
}
