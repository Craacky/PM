using PM.DAL.Entities;
using PM.Models;
using PM.Protocols.Socket;
using PM.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PM.Devices.Base
{
    public class PrinterDeviceService : IPrinterDeviceService
    {
        protected DateTime _dateTimeLastReceivedMessage;
        protected SocketClient _client;
        protected bool _isRun;
        protected string patternTask;
        protected string patternMessage;


        public Device Device { get; set; }


        public LocalDBService LocalDBService { get; set; }
        public ReportTaskService ReportTaskService { get; set; }
        public DeviceSettings DeviceSettings { get; set; }
        public LineSettings LineSettings { get; set; }


        public delegate void ConnectionHandler(SocketClient client, DateTime datetime, ConnectionState connectionState);
        public event ConnectionHandler ConnectionChanged;

        public delegate void MessageHandler(SocketClient client, DateTime datetime, string message);
        public event MessageHandler MessageReceived;


        public PrinterDeviceService(DeviceSettings deviceSettings,
                                   LineSettings lineSettings, LocalDBService localDBService, ReportTaskService reportTaskService)
        {
            DeviceSettings = deviceSettings;
            LineSettings = lineSettings;
            LocalDBService = localDBService;
            ReportTaskService = reportTaskService;

            _dateTimeLastReceivedMessage = DateTime.MinValue;
            _isRun = false;
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
            {
                MessageReceived?.Invoke(client, datetime, message);
                ProcessingMessage(message);
            }
        }
        private void Client_ConnectionChanged(SocketClient client, DateTime datetime, ConnectionState connectionState)
        {
            if (client.IsConnected != Device.IsConnected)
            {
                Device.IsConnected = _client.IsConnected;
                ConnectionChanged?.Invoke(client, datetime, connectionState);

            }

            //if (Device.IsConnected)
            //{
                
            //    //PingingAsync();
            //    //CheskConnectionStateAsync();

                
            //    SendCommandToClearTask();
            //    SendCommadToSetAutoStatus();
            //}

            //if (client.IsConnected != Device.IsConnected && Device.IsConnected && _isRun)
            //{
            //    LoadTemplates();
            //}
        }


        public void ConnectAsync()
        {
            if(Device.IsUsed)
                _client.ConnectAsync();
        }
        public void Disconnect()
        {
            _client.Disconnect();
        }
        public void Start()
        {
            if (_client.IsConnected)
            {
                _isRun = true;

                LoadTemplates();
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

        protected virtual async void PingingAsync()
        {
            //await Task.Run(() =>
            //{
            //    while (_client.IsConnected)
            //    {
            //        string sendCommandToStatus = $"\u0001S \u0017\r\n";
            //        _client.SendMessage(sendCommandToStatus);
            //        Thread.Sleep(1000);
            //    }
            //});
        }
        protected virtual async void CheskConnectionStateAsync()
        {
            //await Task.Run(() =>
            //{
            //    while (Device.IsConnected)
            //    {
            //        double differenceMillisecond = DateTime.Now.Subtract(_dateTimeLastReceivedMessage).TotalMilliseconds;
            //        if (_dateTimeLastReceivedMessage != DateTime.MinValue && differenceMillisecond > 3000)
            //        {
            //            if (Device.IsConnected)
            //            {
            //                Disconnect();
            //            }
            //            _client.RecoveryConnectAsync();
            //        }

            //        Thread.Sleep(500);
            //    }
            //});
        }
        protected virtual void SendCommandToClearTask() { }
        protected virtual void SendCommadToSetAutoStatus() { }
        protected virtual void ProcessingMessage(string message) { }

        public virtual void PrintCode() { }
        public virtual void RepeatePrinteCode() { }
        public virtual void LoadTemplates() { }
    }
}
