using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PM.Protocols.Socket
{
    public class SocketClient
    {
        private readonly bool _isServerClient;
        private System.Net.Sockets.Socket _client;
        private ConnectionState _connectionState;


        public bool IsConnected => _connectionState == ConnectionState.ClientConnected;
        public string Address => Ip + " : " + Port;
        public string Ip { get; private set; }
        public int Port { get; private set; }


        public delegate void ConnectionHandler(SocketClient client, DateTime datetime, ConnectionState connectionState);
        public event ConnectionHandler ConnectionChanged;

        public delegate void MessageHandler(SocketClient client, DateTime datetime, string message);
        public event MessageHandler MessageReceived;
        public event MessageHandler MessageSent;


        public SocketClient(string ip, int port)
        {
            _connectionState = ConnectionState.ClientDisconnected;

            Ip = ip;
            Port = port;
        }
        public SocketClient(System.Net.Sockets.Socket client)
        {
            _connectionState = ConnectionState.ClientDisconnected;

            _client = client;
            Ip = (_client.RemoteEndPoint as IPEndPoint).Address.ToString();
            Port = (_client.RemoteEndPoint as IPEndPoint).Port;
            _isServerClient = true;

            Connect();
        }


        public async void RecoveryConnectAsync()
        {
            await Task.Run(() =>
            {
                do
                {
                    ChangeConnectionState(ConnectionState.ClientRecoveryConnection);
                    TryConnect();
                } while (_connectionState == ConnectionState.ClientRecoveryConnection);
            });
        }
        public async void ConnectAsync()
        {
            await Task.Run(() =>
            {
                TryConnect();
                if (!IsConnected)
                {
                    RecoveryConnectAsync();
                }
            });
        }
        public void Connect()
        {
            TryConnect();
            if (!IsConnected)
            {
                RecoveryConnectAsync();
            }
        }
        public void Disconnect()
        {
            bool isRecoveryConnection = _connectionState == ConnectionState.ClientRecoveryConnection;

            if (IsConnected || isRecoveryConnection)
            {
                _client?.Close();
                ChangeConnectionState(ConnectionState.ClientDisconnected);
            }
        }
        public void SendMessage(string message)
        {
            if (IsConnected)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    _client.Send(data);
                    MessageSent?.Invoke(this, DateTime.Now, message);

                }
                catch (Exception ex)
                {
                }
            }
        }

        private void TryConnect()
        {
            if (!IsConnected)
            {
                try
                {
                    if (!_isServerClient)
                    {
                        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);
                        _client = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _client.Connect(ipPoint);
                    }

                    ChangeConnectionState(ConnectionState.ClientConnected);
                    PoolingServerAsync();
                }
                catch (SocketException ex) when (ex.SocketErrorCode is SocketError.ConnectionRefused or SocketError.TimedOut)
                {
                    if (_connectionState != ConnectionState.ClientRecoveryConnection)
                    {
                        ChangeConnectionState(ConnectionState.ConnectionRefused);
                    }
                }
                catch (SocketException ex)
                {
                    if (_connectionState != ConnectionState.ClientRecoveryConnection)
                    {
                        ChangeConnectionState(ConnectionState.ConnectionRefused);
                    }
                }
            }
        }
        private void ChangeConnectionState(ConnectionState connectionState)
        {
            _connectionState = connectionState;
            ConnectionChanged?.Invoke(this, DateTime.Now, _connectionState);
        }
        private string ReceiveMessage()
        {
            StringBuilder response = new();

            do
            {
                byte[] data = new byte[1024];
                int bytes = _client.Receive(data, data.Length, 0);
                string decodingBytes = Encoding.UTF8.GetString(data, 0, bytes);
                response.Append(decodingBytes);
            }
            while (IsConnected && _client.Available > 0);

            MessageReceived?.Invoke(this, DateTime.Now, response.ToString());
            return response.ToString();
        }

        private async void PoolingServerAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (_client.Connected)
                    {
                        string message = ReceiveMessage();

                        if (string.IsNullOrEmpty(message))
                        {
                            ChangeConnectionState(ConnectionState.ServerStopped);

                            if (!_isServerClient)
                            {
                                RecoveryConnectAsync();
                            }
                            break;

                        }
                        //else if (message.Contains("Login succeeded"))
                        //{
                        //    ChangeConnectionState(ConnectionState.ClientConnected);
                        //}
                        //else if (message.Contains("Login failed"))
                        //{
                        //    Disconnect();
                        //    Thread.Sleep(200);
                        //    RecoveryConnectAsync();
                        //}
                        else if (!IsConnected && !string.IsNullOrEmpty(message))
                        {
                            ChangeConnectionState(ConnectionState.ClientConnected);
                        }

                        Thread.Sleep(1000);
                    }
                }
                catch (IOException ex) when (ex.Message.Contains("Unable to read data"))
                {
                    if (IsConnected && !_isServerClient)
                    {
                        Disconnect();
                        RecoveryConnectAsync();
                    }

                }
                catch (SocketException ex)
                {
                    if (IsConnected && !_isServerClient)
                    {
                        Disconnect();
                        RecoveryConnectAsync();
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    if (IsConnected && !_isServerClient)
                    {
                        Disconnect();
                        RecoveryConnectAsync();
                    }
                }
            });
        }
    }
}
