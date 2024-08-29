using PM.Models.Base;

namespace PM.Models
{
    public class Device : ObservableObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set 
            { 
                _name = value; 
                OnPropertyChanged(nameof(Name)); 
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private bool _isUsed;
        public bool IsUsed
        {
            get => _isUsed;
            set
            {
                _isUsed = value;
                OnPropertyChanged(nameof(IsUsed));
            }
        }

    }
}
