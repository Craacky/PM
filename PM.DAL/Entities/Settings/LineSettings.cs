using PM.DAL.Entities.Base;

namespace PM.DAL.Entities
{
    public class LineSettings : ObservableObject
    {
        private string fullName;
        public string FullName
        {
            get => fullName;
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string shortName;
        public string ShortName
        {
            get => shortName;
            set
            {
                shortName = value;
                OnPropertyChanged(nameof(ShortName));
            }
        }

        private int lineId;
        public int LineId
        {
            get => lineId;
            set
            {
                lineId = value;
                OnPropertyChanged(nameof(LineId));
            }
        }

        private string pathSaveReportTaskFiles;
        public string PathSaveReportTaskFiles
        {
            get => pathSaveReportTaskFiles;
            set
            {
                pathSaveReportTaskFiles = value;
                OnPropertyChanged(nameof(PathSaveReportTaskFiles));
            }
        }

        private string pathLoadNomenclatureFiles;
        public string PathLoadNomenclatureFiles
        {
            get => pathLoadNomenclatureFiles;
            set
            {
                pathLoadNomenclatureFiles = value;
                OnPropertyChanged(nameof(PathLoadNomenclatureFiles));
            }
        }

        private int jobStoragePeriodInDays;
        public int JobStoragePeriodInDays
        {
            get => jobStoragePeriodInDays;
            set
            {
                jobStoragePeriodInDays = value;
                OnPropertyChanged(nameof(JobStoragePeriodInDays));
            }
        }
    }
}
