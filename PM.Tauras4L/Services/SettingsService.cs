﻿using PM.DAL.Entities;
using PM.Devices;
using PM.Models.Base;
using PM.Services;
using System.Linq;
using System.Windows;

namespace PM.Tauras4L.Services
{
    public class SettingsService : ObservableObject, ISettingsService
    {
        private Settings settings;
        public Settings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }


        public LocalDBService LocalDBService { get; set; }


        public SettingsService(LocalDBService localDBService)
        {
            LocalDBService = localDBService;

            Settings = new Settings
            {
                Line = new LineSettings()
                {
                    FullName = "Tauras 4L",
                    ShortName = "4L",
                    LineId = 2,
                    PathSaveReportTaskFiles = @"\\172.25.0.30\apk\In\BrGTU\",
                    PathLoadNomenclatureFiles = @"\\172.25.0.30\apk\Out\BrGTU\",
                    JobStoragePeriodInDays = 60
                },

                ProductCameraMaster = new DeviceSettings()
                {
                    Name = "Камера считывания продукта (master)",
                    Ip = "172.25.4.101",
                    Port = 23,
                    //Ip = "127.0.0.1",
                    //Port = 25,
                    IsUsed = true,
                    Path = ""
                },

                ProductCameraSlave = new DeviceSettings()
                {
                    Name = "Камера считывания продукта (slave)",
                    Ip = "172.25.4.102",
                    Port = 23,
                    //Ip = "127.0.0.1",
                    //Port = 24,
                    IsUsed = true,
                    Path = ""
                },

                BoxCamera = new DeviceSettings()
                {
                    Name = "Камера считывания короба",
                    Ip = "172.25.4.103",
                    Port = 23,
                    //Ip = "127.0.0.1",
                    //Port = 23,
                    IsUsed = true,
                    Path = ""
                },

                BoxPrinter = new DeviceSettings()
                {
                    Name = "Принтер этикетки короба",
                    Ip = "10.162.0.6",
                    Port = 9100,
                    IsUsed = false,
                    Path = ""
                },

                PalletPrinter = new DeviceSettings()
                {
                    Name = "Принтер этикетки паллеты",
                    Ip = "172.25.4.104",
                    Port = 9100,
                    IsUsed = true,
                    Path = ""
                },

                ServerDB = new DBSettings()
                {
                    Name = "База данных (сервер)",
                    ServerName = "172.25.4.116",
                    DataBaseName = "prommark1M",
                    IsAuthentification = true,
                    Login = "sa",
                    Password = "!1qazxcv",
                    IsUsed = true,
                }
            };
        }

        public void LoadSettings()
        {
            Settings settings = LocalDBService.SettingsDataService.GetAll(s => s.Line.LineId == Settings.Line.LineId).LastOrDefault();
            if (settings != null)
            {
                if (settings.ServerDB != null &&
                   settings.ProductCameraSlave != null && settings.BoxCamera != null && settings.ProductCameraMaster != null &&
                   settings.BoxPrinter != null && settings.PalletPrinter != null)
                {
                    Settings = settings;
                    Settings.Id = 0;
                }
            }
        }

        public void SaveSettings(Settings settings)
        {
            Settings lastSettings = LocalDBService.SettingsDataService.GetAll(s => s.Line.LineId == Settings.Line.LineId).LastOrDefault();

            if (lastSettings == null || !lastSettings.Equals(settings))
            {
                Settings = settings;
                Settings.Id = 0;
                LocalDBService.SettingsDataService.Create(Settings);
                MessageBox.Show("Настройки обновлены");
            }
        }
    }
}
