using PM.Models.Base;

namespace PM.Models
{
    public class ScannedCode : ObservableObject
    {
        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(ScannedCode));

                if (_code.Contains("\0"))
                {
                    if (_code.Contains("<GS>"))
                    {
                        Code = _code.Replace("<GS>", "\u001d");
                        return;
                    }
                    if (_code.Contains(",gs."))
                    {
                        Code = _code.Replace(",gs.", "\u001d");
                        return;
                    }
                   
                    ProccessingScannedCode();
                }
            }
        }

        private string _statusScannedCode;
        public string StatusScannedCode
        {
            get => _statusScannedCode;
            set
            {
                _statusScannedCode = value;
                OnPropertyChanged(nameof(StatusScannedCode));
            }
        }

        private bool _isErrorStatusScannedCode;
        public bool IsErrorStatusScannedCode
        {
            get => _isErrorStatusScannedCode;
            set
            {
                _isErrorStatusScannedCode = value;
                OnPropertyChanged(nameof(IsErrorStatusScannedCode));
            }
        }

        private bool _isEnabledFieldScannedCode;
        public bool IsEnabledFieldScannedCode
        {
            get => _isEnabledFieldScannedCode;
            set
            {
                _isEnabledFieldScannedCode = value;
                OnPropertyChanged(nameof(IsEnabledFieldScannedCode));
            }
        }


        public delegate void DelegateProccessingCode();
        public DelegateProccessingCode ProccessingScannedCode { get; set; }


        public ScannedCode()
        {
            ClearFields();
        }


        public void ClearFields()
        {
            Code = string.Empty;
            StatusScannedCode = string.Empty;
            IsErrorStatusScannedCode = false;
            IsEnabledFieldScannedCode = true;
        }
    }
}
