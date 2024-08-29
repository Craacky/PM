namespace PM.Protocols.Socket
{
    public enum ConnectionState
    {
        ClientDisconnected,
        ClientConnected,
        ClientRecoveryConnection,
        ServerStopped,
        ServerStarted,
        ConnectionRefused,
    }

    public static class ConnectionStateExtensions
    {
        public static string ConvertToStringRus(this ConnectionState connectionState)
        {
            return connectionState switch
            {
                ConnectionState.ClientDisconnected => "Клиент отключён",
                ConnectionState.ClientConnected => "Клиент подключён",
                ConnectionState.ConnectionRefused => "Попытка подключения отклонена",
                ConnectionState.ClientRecoveryConnection => "Восстановление подключения",
                ConnectionState.ServerStopped => "Сервер остановлен",
                ConnectionState.ServerStarted => "Сервер запущен",
                _ => "",
            };
        }

        public static string ConvertToStringEng(this ConnectionState connectionState)
        {
            return connectionState switch
            {
                ConnectionState.ClientDisconnected => "Client disconnected",
                ConnectionState.ClientConnected => "Client is connected",
                ConnectionState.ConnectionRefused => "Connection attempt rejected",
                ConnectionState.ClientRecoveryConnection => "Reconnecting",
                ConnectionState.ServerStopped => "Server is stopped",
                ConnectionState.ServerStarted => "Server is running",
                _ => "",
            };
        }
    }
}
