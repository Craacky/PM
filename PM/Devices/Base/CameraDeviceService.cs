using PM.Protocols.Socket;
using System;
using PM.Models;
using PM.DAL.Entities;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;

namespace PM.Devices.Base
{
    public class CameraDeviceService : ICameraDeviceService
    {
        protected SocketClient _client;
        protected bool _isRun;


        public Device Device { get; set; }


        public DeviceSettings DeviceSettings { get; set; }
        public LineSettings LineSettings { get; set; }


        public delegate void ConnectionHandler(SocketClient client, DateTime datetime, ConnectionState connectionState);
        public event ConnectionHandler ConnectionChanged;

        public delegate void MessageHandler(SocketClient client, DateTime datetime, string message);
        public event MessageHandler MessageReceived;


        public CameraDeviceService(DeviceSettings deviceSettings, 
                                   LineSettings lineSettings)
        {
            _isRun = false;

            DeviceSettings = deviceSettings;
            LineSettings = lineSettings;

            _client = new SocketClient(DeviceSettings.Ip, DeviceSettings.Port);

            Device = new Device()
            {
                Name = DeviceSettings.Name,
                Address = _client.Address,
                IsConnected = false,
                IsUsed = DeviceSettings.IsUsed
            };

            _client.ConnectionChanged += Client_ConnectionChanged;
            _client.MessageReceived += Client_MessageReceived;
        }

        private void Client_MessageReceived(SocketClient client, DateTime datetime, string message)
        {
            if (_isRun)
            {
                MessageReceived?.Invoke(client, datetime, message);
            }
        }
        private void Client_ConnectionChanged(SocketClient client, DateTime datetime, ConnectionState connectionState)
        {
            if (client.IsConnected != Device.IsConnected)
            {
                Device.IsConnected = _client.IsConnected;
                ConnectionChanged?.Invoke(client, datetime, connectionState);

                if(client.IsConnected)
                {
                    CheskConnectionStateAsync();
                }

            }
        }

        public void ConnectAsync()
        {
            if (Device.IsUsed)
            {
                _client.ConnectAsync();
            }
        }
        public void Disconnect()
        {
            _client.Disconnect();
        }

        public void Start()
        {
            if (Device.IsConnected)
            {
                _isRun = true;
            }
        }
        public void Stop()
        {
            if (Device.IsConnected && _isRun)
            {
                _isRun = false;
            }
        }
        public void SendMessage(string message)
        {
            if (Device.IsConnected && _isRun)
            {
                _client.SendMessage(message);
            }
        }

        protected virtual void ProcessingMessage(string message) { }

        public virtual void SendCommandLostDevice() { }
        public virtual void SendCommandFindDevice() { }
        public virtual void SendCommandChangeSetup(int numberSetup) { }

        protected virtual async void CheskConnectionStateAsync()
        {
            await Task.Run(() =>
            {
                while (Device.IsConnected)
                {
                    Ping pinger = null;

                    try
                    {
                        pinger = new Ping();
                        PingReply reply = pinger.Send(_client.Ip);
                        if (reply.Status != IPStatus.Success)
                        {
                            if (Device.IsConnected)
                            {
                                Disconnect();
                            }
                            _client.RecoveryConnectAsync();
                        }
                    }
                    catch (PingException)
                    {

                    }

                    Thread.Sleep(5000);
                }
            });
        }
    }
}
