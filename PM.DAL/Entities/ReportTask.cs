using PM.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PM.DAL.Entities
{
    public class ReportTask : Entity
    {
       public Guid Guid { get; set; } = Guid.NewGuid();


        private string lotNumber;
        public string LotNumber
        {
            get => lotNumber;
            set
            {
                lotNumber = value;
                OnPropertyChanged(nameof(LotNumber));
            }
        }

        private string countProductInBox;
        public string CountProductInBox
        {
            get => countProductInBox;
            set
            {
                countProductInBox = value;
                OnPropertyChanged(nameof(CountProductInBox));
            }
        }

        private string countBoxInPallet;
        public string CountBoxInPallet
        {
            get => countBoxInPallet;
            set
            {
                countBoxInPallet = value;
                OnPropertyChanged(nameof(CountBoxInPallet));
            }
        }

        private DateTime manufactureDate;
        public DateTime ManufactureDate
        {
            get => manufactureDate;
            set
            {
                manufactureDate = value;
                ExpiryDateInDays = Nomenclature != null ? Convert.ToInt32(Nomenclature.Attributes.FirstOrDefault(c => c.Code == 11).Value) : 0;
                ExpiryDate = ManufactureDate.AddDays(ExpiryDateInDays);
                OnPropertyChanged(nameof(ManufactureDate));
            }
        }

        private DateTime expiryDate;
        public DateTime ExpiryDate
        {
            get => expiryDate;
            set
            {
                expiryDate = value;
                OnPropertyChanged(nameof(ExpiryDate));
            }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                OnPropertyChanged(nameof(StartTime));

                Status = "Запущено";
            }
        }

        private DateTime stopTime;
        public DateTime StopTime
        {
            get => stopTime;
            set
            {
                stopTime = value;
                OnPropertyChanged(nameof(StopTime));

                Status = "Завершено";
            }
        }

        private bool isUsedCap;
        public bool IsUsedCap
        {
            get => isUsedCap;
            set
            {
                isUsedCap = value;
                OnPropertyChanged(nameof(IsUsedCap));
            }
        }

        private string status = "Новое";
        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }

        }


        public int LineId { get; set; }

        public virtual List<Pallet> Pallets { get; set; } = new List<Pallet>();

        public int? NomenclatureId { get; set; }
        public virtual Nomenclature Nomenclature { get; set; }

        [NotMapped]
        private int expiryDateInDays;

        [NotMapped]
        public int ExpiryDateInDays
        {
            get => expiryDateInDays;
            set
            {
                expiryDateInDays = value;
                OnPropertyChanged(nameof(ExpiryDateInDays));
            }

        }
    }
}
