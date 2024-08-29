using System.Net;
using System.Net.Sockets;

namespace PM.Protocols.Socket
{
    public class SocketServer
    {
        private readonly List<SocketClient> _clients;
        private System.Net.Sockets.Socket _server;
        private ConnectionState _connectionState;


        public bool IsStarted => _connectionState == ConnectionState.ServerStarted;
        public string Address => Ip + " : " + Port;
        public string Ip { get; private set; }
        public int Port { get; private set; }


        public delegate void ConnectionHandler(SocketServer server, DateTime datetime, ConnectionState connectionState);
        public event ConnectionHandler ConnectionChanged;

        public delegate void ClientConnectionHandler(SocketServer server, DateTime datetime, SocketClient client);
        public event ClientConnectionHandler ClientConnected;
        public event ClientConnectionHandler ClientDisconnected;

        public delegate void MessageHandler(SocketServer server, DateTime datetime, string message, SocketClient client);
        public event MessageHandler MessageReceived;
        public event MessageHandler MessageSent;


        public SocketServer(int port)
        {
            _clients = new();
            _connectionState = ConnectionState.ServerStopped;

            //Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[^2].ToString();
            Ip = "127.0.0.1";
            Port = port;
        }


        public void Start()
        {
            if (!IsStarted)
            {
                try
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);

                    _server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _server.Bind(ipPoint);
                    _server.Listen(10);

                    _connectionState = ConnectionState.ServerStarted;
                    AcceptClientsAsync();
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AccessDenied)
                {
                    _connectionState = ConnectionState.ConnectionRefused;
                }

                ConnectionChanged?.Invoke(this, DateTime.Now, _connectionState);
            }
        }
        public void Stop()
        {
            if (IsStarted)
            {
                _connectionState = ConnectionState.ServerStopped;
                ConnectionChanged?.Invoke(this, DateTime.Now, _connectionState);

                for (int i = 0; i < _clients.Count; i++)
                {
                    SocketClient client = _clients[i];
                    client.Disconnect();
                }

                _clients.Clear();
                _server?.Shutdown(SocketShutdown.Both);
                _server?.Close();
            }
        }
        public void SendMessage(string message)
        {
            if (IsStarted)
            {
                foreach (SocketClient client in _clients)
                {
                    client.SendMessage(message);
                }
            }
        }

        private async void AcceptClientsAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (IsStarted)
                    {
                        System.Net.Sockets.Socket newClient = _server.Accept();
                        SocketClient client = new(newClient);
                        client.MessageSent += (client, dateTime, message) => MessageSent?.Invoke(this, dateTime, message, client);
                        client.MessageReceived += (client, dateTime, message) => MessageReceived?.Invoke(this, dateTime, message, client);
                        client.ConnectionChanged += (client, dateTime, connectionState) =>
                        {
                            if (!client.IsConnected)
                            {
                                if (IsStarted)
                                {
                                    _clients.Remove(client);
                                    ClientDisconnected?.Invoke(this, DateTime.Now, client);
                                }
                            }
                            else
                            {
                                ClientConnected?.Invoke(this, DateTime.Now, client);
                            }
                        };

                        if (!IsStarted)
                        {
                            break;
                        }
                        else
                        {
                            _clients.Add(client);
                            ClientConnected?.Invoke(this, DateTime.Now, client);
                        }
                    }
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    Stop();
                }
            });
        }
    }
}

