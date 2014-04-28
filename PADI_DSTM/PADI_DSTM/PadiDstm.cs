using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net.NetworkInformation;

namespace PADI_DSTM
{
    public static class PadiDstm
    {
        
        private static TcpChannel _channel;
        private static RemoteMasterServer _rMasterServer;

        public static void Init()
        {

            _channel = new TcpChannel(0);
            ChannelServices.RegisterChannel(_channel, true);
            //InitializeRemoteMasterServer();

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");
        }

        public static bool Status()
        {
            ArrayList allServersURLs = _rMasterServer.getAllServersURLs();

            foreach (String url in allServersURLs)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                url);

                server.getServerStatus();
            }

            return true;
        }
        public static bool Fail(string URL)
        {

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("fail");

            return res;
        }

        public static bool Freeze(string URL)
        {

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("frozen");

            return res;
        }

        public static bool Recover(string URL)
        {

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("recover");

            return res;
        }

        public static PadInt CreatePadInt(int uid) {

            Tuple<string, string> urls = _rMasterServer.getUrl(uid);
            if(urls!=null){
                RemoteServer _primaryServer = (RemoteServer)Activator.GetObject(typeof(RemoteServer),urls.Item1);
                RemoteServer _secondaryServer = (RemoteServer)Activator.GetObject(typeof(RemoteServer), urls.Item2);
                _secondaryServer.createPadint(uid);
                return _primaryServer.createPadint(uid);
            }
            else return null;
            
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

        public static PadInt AccessPadInt(int uid)
        {
            Tuple<string, string> urls = _rMasterServer.getUrlOfPadInt(uid);

            if (urls != null)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(typeof(RemoteServer), urls.Item1);

                return server.accessPadint(uid);
            }
            else return null;
        }

        public static bool TxBegin()
        {
            return true;
        }

        public static bool TxCommit()
        {
            return true;
        }

        public static bool TxAbort()
        {
            return true;
        }
    }
}
