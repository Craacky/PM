using PM.DAL.Entities;
using PM.Devices.Base;
using PM.Protocols.Socket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PM.Trepko.Services.Devices
{
    public class ProductMasterCameraService : CameraDeviceService, ICameraDeviceService
    {
        public ProductMasterCameraService(DeviceSettings deviceSettings,
                                          LineSettings lineSettings) : base(deviceSettings, lineSettings)
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

        public override void SendCommandFindDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetFindDevice = "||>SET OUTPUT.ACTION 2 1\r\n";
                _client.SendMessage(commandSetFindDevice);
                commandSetFindDevice = "||>SET OUTPUT.ACTION 3 1\r\n";
                _client.SendMessage(commandSetFindDevice);
            }
        }
        public override void SendCommandLostDevice()
        {
            if (Device.IsConnected)
            {
                string commandSetLostDevice = "||>SET OUTPUT.ACTION 2 0\r\n";
                _client.SendMessage(commandSetLostDevice);
                commandSetLostDevice = "||>SET OUTPUT.ACTION 3 0\r\n";
                _client.SendMessage(commandSetLostDevice);
            }
        }
    }
}
