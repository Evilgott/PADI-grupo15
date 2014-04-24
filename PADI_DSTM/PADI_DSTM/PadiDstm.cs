using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using iPADI;
using System.Net.NetworkInformation;

namespace PADI_DSTM
{
    public static class PadiDstm
    {
        //URL = "tcp : // <ip-address >:< port > /Server"
        //TODO:
        /*
        bool Init();
        bool TxBegin();
        bool TxCommit();
        bool TxAbort();
        */
        private static TcpChannel _channel;
        private static RemoteMasterServer _rMasterServer;

        public static void Init()
        {
            RemotingConfiguration.RegisterWellKnownClientType(
            typeof(RemoteMasterServer), "tcp://localhost:8086/MasterServer"); //port e ip address fixos, MasterServer

            _channel = new TcpChannel();
            ChannelServices.RegisterChannel(_channel, true);
            //InitializeRemoteMasterServer();

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");
        }

        public static bool Status()
        {

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteMasterServer masterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            ArrayList allServersURLs = masterServer.getAllServersURLs();

            foreach (String url in allServersURLs)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                url);

                server.getServerStatus();
            }

            ChannelServices.UnregisterChannel(channel);
            return true;
        }
        public static bool Fail(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("fail");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        public static bool Freeze(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("frozen");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        public static bool Recover(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("recover");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        public static PadInt CreatePadInt(int uid) {
            Tuple<String, String> urls = _rMasterServer.getUrl(uid);

            int port = getFreePort();

            TcpChannel tcpChannel = new TcpChannel(port);
            ChannelServices.RegisterChannel(tcpChannel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(typeof(RemoteServer),urls.Item1);

            return server.createPadint(uid);

        }

        private static int getFreePort() {
            int portMin = 2000, portMax = 4000;

            for (int port = portMin; port < portMax; port++)
            {
                if (isFree(port)) {
                    return port;
                }
            }
            return -1;
        }

        private static bool isFree(int port){
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    return false;
                }
            }
            return true;
        }
        /*PadInt AccessPadInt(int uid);
         * */
    }
}
