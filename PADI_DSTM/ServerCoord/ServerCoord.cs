using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
//using System.Runtime.Remoting.RemotingServices;
using PADI_DSTM;

namespace ServerCoord
{
    class ServerCoord
    {
        private TcpChannel _channel;
        private int _port;
        String _serverName;
        private RemoteMasterServer _rMasterServer;
        private RemoteServerCoord _rServer;

        ServerCoord(int port)
        {
            _port = port;
            _channel = new TcpChannel(_port);
            ChannelServices.RegisterChannel(_channel, true);

            _rServer = new RemoteServerCoord(port);

            try
            {
                RemotingServices.Marshal(_rServer, "Server", typeof(RemoteServerCoord));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void registerOnMasterServerCoord()
        {

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            _rMasterServer.registerServerCoord("tcp://localhost:" + _port + "/Server");
           /*
            if (name == "secondary")
            {
                //_rServer.setCheckPrimaryLife();
                _rServer.setUpServer(name);
            }
            else
            {
                //_rServer.setImAlive()
                _rServer.setName("primary");
            }
            */

            Console.WriteLine("Server iniciou como coord");
        }

        static void Main(string[] args)
        {
            System.Console.Write("Insert the server port: ");
            int port = Convert.ToInt32(System.Console.ReadLine());
            ServerCoord server = new ServerCoord(port);

            server.registerOnMasterServerCoord();

            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }
    }
}
