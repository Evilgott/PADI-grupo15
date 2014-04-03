using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using PADI_Library;

namespace MasterServer
{
    class MasterServer : RemoteMasterServer
    {
        private TcpChannel _channel;
        private int _port;

        MasterServer(int port)
        {
            _port = port;
            _channel = new TcpChannel(_port);
            ChannelServices.RegisterChannel(_channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemoteServer),
                "MasterServer",
                WellKnownObjectMode.Singleton);
        }
        static void Main(string[] args)
        {
            System.Console.Write("Insert the server port: ");
            int port = Convert.ToInt32(System.Console.ReadLine());
            MasterServer server = new MasterServer(port);


            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }
    }
}
