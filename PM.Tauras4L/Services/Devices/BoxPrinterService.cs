using PM.DAL.Entities;
using PM.Devices;
using PM.Devices.Base;
using PM.Protocols.Socket;
using PM.Services;
using System;

namespace PM.Tauras4L.Services.Devices
{
    public class BoxPrinterService : PrinterDeviceService, IPrinterDeviceService
    {
        public BoxPrinterService(DeviceSettings deviceSettings,
                                 LineSettings lineSettings,
                                 LocalDBService localDBService,
                                 ReportTaskService reportTaskService) : base(deviceSettings, lineSettings, localDBService, reportTaskService)
        {


            _client.ConnectionChanged += Client_ConnectionChanged;
            _client.MessageReceived += Client_MessageReceived;
            _client.MessageSent += Client_MessageSent;

        }

        private void Client_MessageSent(SocketClient client, DateTime datetime, string message)
        {
            if (Device.IsConnected && _isRun)
            { }
        }
        private void Client_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
            if (Device.IsConnected && _isRun)
            { }
        }
        private void Client_ConnectionChanged(SocketClient client, DateTime datetime, ConnectionState connectionState)
        {
            if (Device.IsConnected && _isRun)
            { }
        }
    }
}
