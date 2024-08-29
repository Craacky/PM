using PM.DAL.Entities.Base;

namespace PM.DAL.Entities
{
    public class DeviceSettings : ObservableObject
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string ip;
        public string Ip
        {
            get => ip;
            set
            {
                ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        private int port;
        public int Port
        {
            get => port;
            set
            {
                port = value;
                OnPropertyChanged(nameof(Port));
            }
        }

        private bool isUsed;
        public bool IsUsed
        {
            get => isUsed;
            set
            {
                isUsed = value;
                OnPropertyChanged(nameof(IsUsed));
            }
        }

        private string path;
        public string Path
        {
            get => path;
            set
            {
                path = value;
                OnPropertyChanged(nameof(Path));
            }
        }
    }
}
