namespace PM.DAL.EFCore
{
    public enum ConnectionState
    {
        Connected,
        Created,
        NotFoundDB,
        Disconnected,
        NotCorrectConnectionString
    }

    public static class ConnectionStateExtensions
    {
        public static string ConvertToStringRus(this ConnectionState connectionState)
        {
            return connectionState switch
            {
                ConnectionState.Connected => "База данных подключена",
                ConnectionState.Created => "База данных создана и подключена",
                ConnectionState.NotFoundDB => "Найдена база данных с данным именем, не соответсвующая формату",
                ConnectionState.Disconnected => "База данных отключена",
                ConnectionState.NotCorrectConnectionString => "Не корректная строка подключения",
                _ => "",
            };
        }

        public static string ConvertToStringEng(this ConnectionState connectionState)
        {
            return connectionState switch
            {
                ConnectionState.Connected => "The database is connected",
                ConnectionState.Created => "The database has been created and connected",
                ConnectionState.NotFoundDB => "Found a database with name data that does not match the format",
                ConnectionState.Disconnected => "Database is disconnected",
                ConnectionState.NotCorrectConnectionString => "Incorrect connection string",
                _ => "",
            };
        }
    }
}
