using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionProcessingService.Types;
using SuperWebSocket;

namespace OptionProcessingService
{
    class Client
    {
        public bool IsLoggedIn { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Account AccountInfo { get; set; }

        public WebSocketSession SocketSession { get; private set; }
        public ClientSubscriptions Subscriptions { get; private set; }

        public Client(WebSocketSession session)
        {
            SocketSession = session;
            Subscriptions = new ClientSubscriptions();
            IsLoggedIn = false;
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}
