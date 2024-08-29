using PM.DAL.Entities;
using PM.Devices.Base;

namespace PM.Mixter.Services.Devices
{
    public class BoxCameraService : CameraDeviceService, ICameraDeviceService
    {
        public BoxCameraService(DeviceSettings deviceSettings,
                                LineSettings lineSettings) : base(deviceSettings, lineSettings)
        { }

        //protected override async void PingingAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        string commandSetEnableDMCCResponce = "||>SET COM.DMCC-RESPONSE 1\r\n";
        //        _client.SendMessage(commandSetEnableDMCCResponce);

        //        string commandSetEnableInputString = "||>SET INPUT-STRING.ENABLE ON\r\n";
        //        _client.SendMessage(commandSetEnableInputString);

        //        string commandSetHeader = "||>SET INPUT-STRING.HEADER \"<start>\"\r\n";
        //        _client.SendMessage(commandSetHeader);

        //        string commandSetFooter = "||>SET INPUT-STRING.FOOTER \"<stop>\"\r\n";
        //        _client.SendMessage(commandSetFooter);

        //        string commandInputString = $"||>SET INPUT-STRING.VALUE \"{LineSettings.ShortName}\"\r\n";
        //        _client.SendMessage(commandInputString);

        //        string commandTriggerON = "||>TRIGGER ON\r\n";
        //        _client.SendMessage(commandTriggerON);

        //        while (Device.IsConnected)
        //        {
        //            if (_isRun)
        //            {
        //                commandInputString = $"||>SET INPUT-STRING.VALUE \"{LineSettings.ShortName}\"\r\n";
        //            }
        //            else
        //            {
        //                commandInputString = $"||>SET INPUT-STRING.VALUE \"PING\"\r\n";
        //            }
        //            _client.SendMessage(commandInputString);

        //            Thread.Sleep(600);
        //        }
        //    });
        //}
    }
}
