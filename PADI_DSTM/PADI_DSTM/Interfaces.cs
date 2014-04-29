using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace PADI_DSTM
{
    public class RemoteMasterServer : MarshalByRefObject
    {
        private int _currentTId = 0;

        //string = URL do server
        //int = quantidade de padint's guardados neste servidor
        private Dictionary<string, int> _primaryServers = new Dictionary<string, int>();
        //string = URL do secondary server
        //string = URL do primary server
        private Dictionary<string, string> _secondaryServers = new Dictionary<string, string>();

        private Dictionary<int, Tuple<string, string>> _servers = new Dictionary<int, Tuple<string, string>>();

        public String registerServer(String serverURL)
        {
            if ((_primaryServers.Count + _secondaryServers.Count) % 2 == 0)
            {
                _primaryServers.Add(serverURL, 0);
                Console.WriteLine("new server: "+serverURL);
                return "primary";
            }
            else
            {
                int primaryIndex = _primaryServers.Count-1;
                _secondaryServers.Add(_primaryServers.ElementAt(primaryIndex).Key, serverURL);
                return "secondary";
            }

        }

        public ArrayList getAllServersURLs()
        {
            ArrayList mergedArray = new ArrayList();

            foreach (String url in _primaryServers.Keys)
            {
                mergedArray.Add(url);
            }
            foreach (String url in _secondaryServers.Keys)
            {
                mergedArray.Add(url);
            }

            return mergedArray;
        }

        public Tuple<String, String> getUrl(int uid)
        {
            if (_servers.ContainsKey(uid))
            {
                return null;
            }
            else
            {
                return getServerUrlForNewPadInt();
            }
        }

        public Tuple<String, String> getUrlOfPadInt(int uid)
        {
            if (_servers.ContainsKey(uid))
            {
                return _servers[uid];
            }
            else
            {
                return null;
            }
        }

        private Tuple<string, string> getServerUrlForNewPadInt()
        {
            string primaryServerForNewPadInt = null;
            string secondaryServerForNewPadInt = null;
            //99999 assumed as a infinite value
            int minNumberPadInt = 99999;
            for (int i = 0; i < _primaryServers.Count; i++)
            {
                if(_primaryServers.ElementAt(i).Value < minNumberPadInt){
                    primaryServerForNewPadInt = _primaryServers.ElementAt(i).Key;
                    secondaryServerForNewPadInt = _secondaryServers[primaryServerForNewPadInt];
                    minNumberPadInt = _primaryServers.ElementAt(i).Value;
                }
            }
            return new Tuple<string, string>(primaryServerForNewPadInt, secondaryServerForNewPadInt);
        }

        public bool registNewPadInt(int uid, string url)
        {
            if (_primaryServers.ContainsKey(url))
            {
                _primaryServers[url] = _primaryServers[url] + 1;
                _servers.Add(uid, new Tuple<string, string>(url, _secondaryServers[url]));
                return true;
            }
            else return false;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    public class RemoteServer : MarshalByRefObject
    {
        private enum State { normal, frozen, failing };
        private string _url;
        private State _serverState = State.normal;
        private ArrayList _calls = new ArrayList();
        private Dictionary<int, PadInt> padintList = new Dictionary<int, PadInt>();

        private static RemoteMasterServer _rMasterServer;

        public RemoteServer(int port)
        {

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            _url = "tcp://localhost:" + port + "/Server";

            Console.WriteLine("Server URL: "+ _url);
        }

        public bool changeServerState(String newState)
        {
            if (newState == "frozen") _serverState = State.frozen;
            else if (newState == "fail") _serverState = State.failing;
            else if (newState == "recover") _serverState = State.normal;
            return true;
        }

        public void getServerStatus()
        {
            Console.WriteLine(_serverState.ToString());
        }

        public string getUrl()
        {
            return _url;
        }

        public PadInt createPadint(int uid)
        {
            Console.WriteLine("uid: "+ uid);
            padintList.Add(uid, new PadInt());
            Console.WriteLine("O int: " + uid + " foi criado");
            informMasterNewPadInt(uid, _url);
            return padintList[uid];
        }

        private bool informMasterNewPadInt(int uid, string url)
        {
            return _rMasterServer.registNewPadInt(uid, url);
        }

        public PadInt accessPadint(int uid)
        {
            return padintList[uid];
        }







        public override object InitializeLifetimeService()
        {
            return null;
        }

        //private Dictionary<PadInt, TX> _wTX; write transactions
        //private Dictionary<PadInt, TX> _rTX; read transactions
        //private List<Requests> _rq;
    }
}
