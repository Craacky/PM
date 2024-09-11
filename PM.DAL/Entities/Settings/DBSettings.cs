using PM.DAL.Entities.Base;

namespace PM.DAL.Entities
{
    public class DBSettings : ObservableObject
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

        private string serverName;
        public string ServerName
        {
            get => serverName;
            set
            {
                serverName = value;
                OnPropertyChanged(nameof(ServerName));
            }
        }

        private string dataBaseName;
        public string DataBaseName
        {
            get => dataBaseName;
            set
            {
                dataBaseName = value;
                OnPropertyChanged(nameof(DataBaseName));
            }
        }

        private bool isAuthentification;
        public bool IsAuthentification
        {
            get => isAuthentification;
            set
            {
                isAuthentification = value;
                OnPropertyChanged(nameof(IsAuthentification));
            }
        }

        private string login;
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
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



        //public string ConnectionString => IsAuthentification ? @$"Data Source={ServerName};Initial Catalog={DataBaseName}; User ID= {Login};Password= {Password};Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False;" : @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;";
        public string ConnectionString => IsAuthentification ? @$"Data Source={ServerName};Initial Catalog={DataBaseName}; User ID= {Login};Password= {Password};Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False;"
            : @$"Data Source={ServerName};Initial Catalog={DataBaseName};Trusted_Connection=True;";
    }
}
