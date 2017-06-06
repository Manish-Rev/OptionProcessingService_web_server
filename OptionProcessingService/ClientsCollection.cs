using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SuperWebSocket;

namespace OptionProcessingService
{
    static class ClientsCollection
    {
        private static List<Client> _connections = new List<Client>();

        public static List<Client> GetAll()
        {
            lock (_connections)
                return new List<Client>(_connections);
        }

        public static void Add(Client client)
        {
            lock(_connections)
                _connections.Add(client);
        }

        public static Client Get(WebSocketSession socketClient)
        {
            int pos = GetPosition(socketClient);
            lock (_connections)
                return pos != -1 ? _connections.ElementAt(pos) : null;
        }

        public static Client GetByID(string id)
        {
            foreach (var client in _connections)
            {
                if (client.AccountInfo.ID == id)
                    return client;
            }
            return null;
        }

        public static Client GetByUserName(string userName)
        {
            foreach (var client in _connections)
            {
                if (client.AccountInfo.Username == userName)
                    return client;
            }
            return null;
        }

        public static void SetCredentials(WebSocketSession socketClient, string username, string password)
        {
            int pos = GetPosition(socketClient);
            if (pos != -1)
            {
                lock (_connections)
                {
                    var client = _connections.ElementAt(pos);
                    client.Username = username;
                    client.Password = password;
                }
            }
        }

        public static void Remove(WebSocketSession socketClient)
        {
            int pos = GetPosition(socketClient);
            if (pos != -1)
            {
                lock (_connections)
                    _connections.RemoveAt(pos);
            }
        }

        private static int GetPosition(WebSocketSession socketClient)
        {
            var i = 0;
            lock (_connections)
            {
                foreach (Client c in _connections)
                {
                    if (Object.ReferenceEquals(c.SocketSession, socketClient))
                    {
                        return i;
                    }
                    i++;
                }
            }
            return -1;
        }

    }
}
