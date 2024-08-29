using PM.BLL.Services;
using PM.DAL.EFCore;
using PM.DAL.Entities;
using PM.Models;
using System;

namespace PM.Devices
{
    public class LocalDBService
    {
        public Device Device { get; set; }


        public NomenclatureDataService NomenclatureDataService { get; set; }
        public AttributeDataService AttributeDataService { get; set; }
        public ReportTaskDataService ReportTaskDataService { get; set; }
        public ProductDataService ProductDataService { get; set; }
        public BoxDataService BoxDataService { get; set; }
        public PalletDataService PalletDataService { get; set; }
        public LineDataService LineDataService { get; set; }
        public SettingsDataService SettingsDataService { get; set; }


        public delegate void ConnectionHandler(DBContext db, DateTime datetime, ConnectionState connectionState);
        public event ConnectionHandler ConnectionChanged;


        public LocalDBService()
        {
            Device = new Device
            {
                Name = Settings.LocalDB.Name,
                Address = Settings.LocalDB.ConnectionString,
                IsUsed = Settings.LocalDB.IsUsed,
            };

            NomenclatureDataService = new NomenclatureDataService(Device.Address);
            AttributeDataService = new AttributeDataService(Device.Address);
            ReportTaskDataService = new ReportTaskDataService(Device.Address);
            ProductDataService = new ProductDataService(Device.Address);
            BoxDataService = new BoxDataService(Device.Address);
            PalletDataService = new PalletDataService(Device.Address);
            LineDataService = new LineDataService(Device.Address);
            SettingsDataService = new SettingsDataService(Device.Address);

            DBContext.ConnectionChanged += DBContext_ConnectionChanged;
            using DBContext dBContext = new DBContext(Device.Address);
        }


        private void DBContext_ConnectionChanged(DBContext db, DateTime datetime, ConnectionState connectionState)
        {
            if (db.IsConnected != Device.IsConnected)
            {
                ConnectionChanged?.Invoke(db, datetime, connectionState);

                if (db.ConnectionString == Device.Address)
                {
                    Device.IsConnected = db.IsConnected;
                }
            }
        }
    }
}
