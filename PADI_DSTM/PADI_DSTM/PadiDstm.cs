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
        private static RemoteServerCoord _rServerCoord;

        private static Dictionary<int,PadInt> padIntList = new Dictionary<int,PadInt>();

        private static Dictionary<int, Tuple<String, String>> padintLocations = new Dictionary<int, Tuple<string, string>>();

        private static int _actualTxId;

        /* Representa a ligação entre uma transacção e um conjunto 
         * de PadInts que se encontram associados a esta transacção
         */
        private static Dictionary<int, ArrayList> _txPadInts = new Dictionary<int, ArrayList>();

        /* Representa uma lista dos valores originais de todos os PadInts alterados dentro da transacção actual 
         * {PadIntId, [int OldValue, ServerURL]}
         */
        private static Dictionary<int, Tuple<int, string>> _actualTxPadIntOldValues = new Dictionary<int, Tuple<int, string>>();

        public static void Init()
        {

            _channel = new TcpChannel(0);
            ChannelServices.RegisterChannel(_channel, true);
            //InitializeRemoteMasterServer();

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            _rServerCoord = (RemoteServerCoord)Activator.GetObject(
                typeof(RemoteServerCoord),
                _rMasterServer.getServerCoordUrl());
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

            try
            {
                Tuple<string, string> urls = _rMasterServer.getUrlOfPadInt(uid);

                if (urls != null)
                {
                    RemoteServer server = (RemoteServer)Activator.GetObject(typeof(RemoteServer), urls.Item1);

                    PadInt requestedPadInt = server.accessPadint(uid);
                    if (!padintLocations.ContainsKey(uid)) padintLocations.Add(uid, new Tuple<string, string>(urls.Item1, urls.Item2));
                    
                    PadInt padintCopy = null;

                    if (padIntList.ContainsKey(uid))
                    {
                        padintCopy = padIntList[uid];
                    }
                    else
                    {
                        padintCopy = new PadInt(uid, urls.Item1);
                    }


                    padintCopy.Write(requestedPadInt.Read());
                    padintCopy.setTxId(_actualTxId);

                    if (!padIntList.ContainsKey(uid))
                    {
                        padIntList.Add(uid, padintCopy);
                    }
                    return padintCopy;
                }
                else return null;
            }
            catch (TxException txE)
            {
                Console.WriteLine(txE.Message);
                return null;
            }
            
        }

        public static bool TxBegin()
        {
            try
            {
                _actualTxId = _rMasterServer.getNextTxId();
                return true;
            }
            catch(TxException txE) {
                Console.WriteLine(txE.Message);
                return false;
            }
        }

        public static bool TxCommit()
        {
            Console.WriteLine("Entrei no commit");
            while (!_rServerCoord.canCommit())
            {
                //fica à espera
                Console.WriteLine("deu falso");
            }

            Console.WriteLine("deu true");

            foreach (KeyValuePair<int, PadInt> padint in padIntList)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(typeof(RemoteServer), padint.Value.getUrl());
                PadInt padintToChange = server.accessPadint(padint.Key);
                _actualTxPadIntOldValues.Add(padint.Key, new Tuple<int, String>(padintToChange.Read(), padintToChange.getUrl()));
                bool commitSuccess = padintToChange.WriteToServer(padint.Value.Read(), _actualTxId);
                if (!commitSuccess)
                {
                    TxAbort();
                    return false;
                }
                RemoteServer serverBackup = (RemoteServer)Activator.GetObject(typeof(RemoteServer), padintLocations[padint.Key].Item2);
                padintToChange = serverBackup.accessPadint(padint.Key);
                padintToChange.WriteToServer(padint.Value.Read(), _actualTxId);

                server.printAllInts();
            }
            _actualTxPadIntOldValues.Clear();

            _rServerCoord.setServerCanCommit(true);
            
            return true;
            
            
        }

        public static bool TxAbort()
        {
            foreach(KeyValuePair<int, Tuple<int,string>> oldPadint in _actualTxPadIntOldValues)
            {

                RemoteServer server = (RemoteServer)Activator.GetObject(typeof(RemoteServer), oldPadint.Value.Item2);
                server.revertPadIntChange(_actualTxId, oldPadint.Key, oldPadint.Value.Item1);
                
            }
            _actualTxPadIntOldValues.Clear();
            
            _rServerCoord.setServerCanCommit(true);
            
            return true;
        }
    }
}
