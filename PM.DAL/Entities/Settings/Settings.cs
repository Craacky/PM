using PM.DAL.Entities.Base;
using System;

namespace PM.DAL.Entities
{
    public class Settings : Entity, ICloneable
    {
        private LineSettings line;
        public LineSettings Line
        {
            get => line;
            set
            {
                line = value;
                OnPropertyChanged(nameof(Line));
            }
        }

        private DeviceSettings productCameraMaster;
        public DeviceSettings ProductCameraMaster
        {
            get => productCameraMaster;
            set
            {
                productCameraMaster = value;
                OnPropertyChanged(nameof(ProductCameraMaster));
            }
        }
        private DeviceSettings productCameraSlave;
        public DeviceSettings ProductCameraSlave
        {
            get => productCameraSlave;
            set
            {
                productCameraSlave = value;
                OnPropertyChanged(nameof(ProductCameraSlave));
            }
        }
        private DeviceSettings boxCamera;
        public DeviceSettings BoxCamera
        {
            get => boxCamera;
            set
            {
                boxCamera = value;
                OnPropertyChanged(nameof(BoxCamera));
            }
        }
        private DeviceSettings boxPrinter;
        public DeviceSettings BoxPrinter
        {
            get => boxPrinter;
            set
            {
                boxPrinter = value;
                OnPropertyChanged(nameof(BoxPrinter));
            }
        }
        private DeviceSettings palletPrinter;
        public DeviceSettings PalletPrinter
        {
            get => palletPrinter;
            set
            {
                palletPrinter = value;
                OnPropertyChanged(nameof(PalletPrinter));
            }
        }

        private DBSettings serverDB;
        public DBSettings ServerDB
        {
            get => serverDB;
            set
            {
                serverDB = value;
                OnPropertyChanged(nameof(ServerDB));
            }
        }

        public static DBSettings LocalDB { get; set; }

        static Settings()
        {
            LocalDB = new DBSettings()
            {
                Name = "База данных (локальная)",
                ServerName = "localhost",
                DataBaseName = "prommark1M",
                IsAuthentification = false,
                Login = null,
                Password = null,
                IsUsed = true,
            };
        }

        public object Clone()
        {
            Settings newSettings
            = new Settings
            {
                Line = new LineSettings()
                {
                    FullName = Line.FullName,
                    ShortName = Line.ShortName,
                    LineId = Line.LineId,
                    PathSaveReportTaskFiles = Line.PathSaveReportTaskFiles,
                    PathLoadNomenclatureFiles = Line.PathLoadNomenclatureFiles,
                    JobStoragePeriodInDays = Line.JobStoragePeriodInDays
                },

                ProductCameraMaster = new DeviceSettings()
                {
                    Name = ProductCameraMaster.Name,
                    Ip = ProductCameraMaster.Ip,
                    Port = ProductCameraMaster.Port,
                    IsUsed = ProductCameraMaster.IsUsed,
                    Path = ProductCameraMaster.Path
                },

                ProductCameraSlave = new DeviceSettings()
                {
                    Name = ProductCameraSlave.Name,
                    Ip = ProductCameraSlave.Ip,
                    Port = ProductCameraSlave.Port,
                    IsUsed = ProductCameraSlave.IsUsed,
                    Path = ProductCameraSlave.Path
                },

                BoxCamera = new DeviceSettings()
                {
                    Name = BoxCamera.Name,
                    Ip = BoxCamera.Ip,
                    Port = BoxCamera.Port,
                    IsUsed = BoxCamera.IsUsed,
                    Path = BoxCamera.Path
                },

                BoxPrinter = new DeviceSettings()
                {
                    Name = BoxPrinter.Name,
                    Ip = BoxPrinter.Ip,
                    Port = BoxPrinter.Port,
                    IsUsed = BoxPrinter.IsUsed,
                    Path = BoxPrinter.Path
                },

                PalletPrinter = new DeviceSettings()
                {
                    Name = PalletPrinter.Name,
                    Ip = PalletPrinter.Ip,
                    Port = PalletPrinter.Port,
                    IsUsed = PalletPrinter.IsUsed,
                    Path = BoxPrinter.Path
                },

                ServerDB = new DBSettings()
                {
                    Name = ServerDB.Name,
                    ServerName = ServerDB.ServerName,
                    DataBaseName = ServerDB.DataBaseName,
                    IsAuthentification = ServerDB.IsAuthentification,
                    Login = ServerDB.Login,
                    Password = ServerDB.Password,
                    IsUsed = ServerDB.IsUsed,
                }
            };

            return newSettings;
        }

        public override bool Equals(object obj)
        {
            Settings settings = obj as Settings;
            bool isEqual =
                settings.Line.FullName == Line.FullName &&
                settings.Line.ShortName == Line.ShortName &&
                settings.Line.LineId == Line.LineId &&
                settings.Line.PathSaveReportTaskFiles == Line.PathSaveReportTaskFiles &&
                settings.Line.PathLoadNomenclatureFiles == Line.PathLoadNomenclatureFiles &&
                settings.Line.JobStoragePeriodInDays == Line.JobStoragePeriodInDays &&
                
                settings.ProductCameraMaster.Name == ProductCameraMaster.Name &&
                settings.ProductCameraMaster.Ip == ProductCameraMaster.Ip &&
                settings.ProductCameraMaster.Port == ProductCameraMaster.Port &&
                settings.ProductCameraMaster.IsUsed == ProductCameraMaster.IsUsed &&
                settings.ProductCameraMaster.Path == ProductCameraMaster.Path &&

                settings.ProductCameraSlave.Name == ProductCameraSlave.Name &&
                settings.ProductCameraSlave.Ip == ProductCameraSlave.Ip &&
                settings.ProductCameraSlave.Port == ProductCameraSlave.Port &&
                settings.ProductCameraSlave.IsUsed == ProductCameraSlave.IsUsed &&
                settings.ProductCameraSlave.Path == ProductCameraSlave.Path &&

                settings.BoxCamera.Name == BoxCamera.Name &&
                settings.BoxCamera.Ip == BoxCamera.Ip &&
                settings.BoxCamera.Port == BoxCamera.Port &&
                settings.BoxCamera.IsUsed == BoxCamera.IsUsed &&
                settings.BoxCamera.Path == BoxCamera.Path &&

                settings.BoxPrinter.Name == BoxPrinter.Name &&
                settings.BoxPrinter.Ip == BoxPrinter.Ip &&
                settings.BoxPrinter.Port == BoxPrinter.Port &&
                settings.BoxPrinter.IsUsed == BoxPrinter.IsUsed &&
                settings.BoxPrinter.Path == BoxPrinter.Path &&

                settings.PalletPrinter.Name == PalletPrinter.Name &&
                settings.PalletPrinter.Ip == PalletPrinter.Ip &&
                settings.PalletPrinter.Port == PalletPrinter.Port &&
                settings.PalletPrinter.IsUsed == PalletPrinter.IsUsed && 
                settings.PalletPrinter.Path == PalletPrinter.Path &&

                settings.ServerDB.Name == ServerDB.Name &&
                settings.ServerDB.ServerName == ServerDB.ServerName &&
                settings.ServerDB.DataBaseName == ServerDB.DataBaseName &&
                settings.ServerDB.IsAuthentification == ServerDB.IsAuthentification &&
                settings.ServerDB.Login == ServerDB.Login &&
                settings.ServerDB.Password == ServerDB.Password &&
                settings.ServerDB.IsUsed == ServerDB.IsUsed;

            return isEqual;
        }
    }
}
