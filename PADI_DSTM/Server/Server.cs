﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
//using System.Runtime.Remoting.RemotingServices;
using PADI_DSTM;

namespace Server
{
    class Server
    {
        private TcpChannel _channel;
        private int _port;
        String _serverName;
        private RemoteMasterServer _rMasterServer;

        Server(int port)
        {
            _port = port;
            _channel = new TcpChannel(_port);
            ChannelServices.RegisterChannel(_channel, true);

            RemoteServer obj = new RemoteServer();

            try
            {
                RemotingServices.Marshal(obj, "Server", typeof(RemoteServer));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void registerOnMasterServer()
        {

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            Console.WriteLine("Server iniciou como " + _rMasterServer.registerServer("tcp://localhost:"+_port+"/Server"));
        }

        static void Main(string[] args)
        {
            System.Console.Write("Insert the server port: ");
            int port = Convert.ToInt32(System.Console.ReadLine());
            Server server = new Server(port);

            server.registerOnMasterServer();

            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }
    }
}
