﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using PADI_DSTM;

namespace MasterServer
{
    class MasterServer
    {
        private TcpChannel _channel;
        private int _port;

        MasterServer(int port)
        {
            _port = port;
            _channel = new TcpChannel(_port);
            ChannelServices.RegisterChannel(_channel, true);

            RemoteMasterServer obj = new RemoteMasterServer();

            try
            {
                RemotingServices.Marshal(obj, "MasterServer", typeof(RemoteMasterServer));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        static void Main(string[] args)
        {
            //System.Console.Write("Insert the server port: ");
            //int port = Convert.ToInt32(System.Console.ReadLine());

            //Master server fixo
            int port = 8086;
            MasterServer server = new MasterServer(port);

            System.Console.WriteLine("server running on the port: " + port);
            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }
    }
}

