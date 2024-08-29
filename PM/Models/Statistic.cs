using PM.Models.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace PM.Models
{
    public class Statistic : ObservableObject
    {
        private List<DAL.Entities.Pallet> palletCodes;
        public List<DAL.Entities.Pallet> PalletCodes
        {
            get => palletCodes;
            set
            {
                palletCodes = value;
                OnPropertyChanged(nameof(PalletCodes));
            }
        }

        private List<DAL.Entities.Box> boxCodes;
        public List<DAL.Entities.Box> BoxCodes
        {
            get => boxCodes;
            set
            {
                boxCodes = value;
            }
        }

        private List<DAL.Entities.Product> productCodes;
        public List<DAL.Entities.Product> ProductCodes
        {
            get => productCodes;
            set
            {
                productCodes = value;
            }
        }

        private int countBoxInCurrentPallet;
        public int CountBoxInCurrentPallet
        {
            get => countBoxInCurrentPallet;
            set
            {
                countBoxInCurrentPallet = value;
                OnPropertyChanged(nameof(CountBoxInCurrentPallet));
            }
        }

        private int countProducts;
        public int CountProducts
        {
            get => countProducts;
            set
            {
                countProducts = value;
                OnPropertyChanged(nameof(CountProducts));
            }
        }

        private int countBoxes;
        public int CountBoxes
        {
            get => countBoxes;
            set
            {
                countBoxes = value;
                OnPropertyChanged(nameof(CountBoxes));
            }
        }


        private List<СameraReadingResult> cameraReadingResults;
        public ObservableCollection<СameraReadingResult> CameraReadingResults{ get; set; }
    //    {
    //        get => cameraReadingResults;
    //        set
    //        {
    //            cameraReadingResults = value;
    //            OnPropertyChanged(nameof(CameraReadingResults));
    //}
//}
private object _syncLock = new object();
        public Statistic()
        {
            CameraReadingResults = new ObservableCollection<СameraReadingResult>();
            BindingOperations.EnableCollectionSynchronization(CameraReadingResults, _syncLock);
            PalletCodes = new List<DAL.Entities.Pallet>();
            BoxCodes = new List<DAL.Entities.Box>();
            ProductCodes = new List<DAL.Entities.Product>();


        }
    }
}

