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
        private int _currentTxId = 0;

        //string = URL do server
        //int = quantidade de padint's guardados neste servidor
        private Dictionary<string, int> _primaryServers = new Dictionary<string, int>();
        //string = URL do primary server
        //string = URL do secondary server
        private Dictionary<string, string> _secondaryServers = new Dictionary<string, string>();

        //string = URL do server failed
        //string = URL do server que substitui (secondary)
        private Dictionary<string, string> _failedServers = new Dictionary<string, string>();

        private Dictionary<int, Tuple<string, string>> _servers = new Dictionary<int, Tuple<string, string>>();

        private string _coordUrl;

        public void registerServerCoord(string coordUrl)
        {
            _coordUrl = coordUrl;
        }

        public string getServerCoordUrl()
        {
            return _coordUrl;
        }
        
        public String registerServer(String serverURL)
        {
            

            if (_failedServers.ContainsKey(serverURL))
            {

                string primaryUrl =_failedServers[serverURL];
                _secondaryServers.Add(primaryUrl, serverURL);
                _secondaryServers.Remove(serverURL);
                _failedServers.Remove(serverURL);
                Console.WriteLine("server recovered is now secondary: " + serverURL);
                return "secondary";
            }

            if ((_primaryServers.Count + _secondaryServers.Count) % 2 == 0)
            {
                _primaryServers.Add(serverURL, 0);
                Console.WriteLine("new primary server: " + serverURL);
                return "primary";
            }
            else
            {
                int primaryIndex = _primaryServers.Count - 1;
                _secondaryServers.Add(_primaryServers.ElementAt(primaryIndex).Key, serverURL);
                Console.WriteLine("new secondary server: " + serverURL);
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
            foreach (String url in _secondaryServers.Values)
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
                Tuple<string,string> newPadintLocation = getServerUrlForNewPadInt();
                registNewPadInt(uid, newPadintLocation.Item1);
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

        public int getNextTxId()
        {
            int txIdToReturn = _currentTxId;
            _currentTxId++;
            return txIdToReturn;
        }

        public String getPrimary(String url)
        {
            foreach (KeyValuePair<string, string> pair in _secondaryServers)
            {
                if(pair.Value.Equals(url)) return pair.Key;
            }
            return null;
        }

        public void swapToPrimaryServer(string url, string secondaryUrl)
        {
            int numInt = _primaryServers[url];

            _primaryServers.Add(secondaryUrl, numInt);
            _primaryServers.Remove(url);

            _failedServers.Add(url, secondaryUrl);

            swapUrls(url, secondaryUrl);


            RemoteServer server = (RemoteServer)Activator.GetObject(
            typeof(RemoteServer),
            secondaryUrl); //secondary remote server

            Console.WriteLine("new primary server with url: " + secondaryUrl + " and secondary server with url: " + url);

            //_servers


        }

        public void swapUrls(string primaryUrl, string secondaryUrl) {

            Dictionary<int, Tuple<string, string>> temp = new Dictionary<int, Tuple<string, string>>();

            foreach (KeyValuePair<int, Tuple<string, string>> pair in _servers)
            {
                if (pair.Value.Item1.Equals(primaryUrl))
                {
                    temp.Add(pair.Key, new Tuple<string, string>(secondaryUrl, primaryUrl));
                }
                else
                {
                    temp.Add(pair.Key, pair.Value);
                }
            }

            _servers = temp;
           

        }

        
    }



    public class RemoteServerCoord : MarshalByRefObject
    {
        private static RemoteMasterServer _rMasterServer;
        private string _url;
        private bool _serverCanCommit = true;

        public RemoteServerCoord(int port)
        {

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            _url = "tcp://localhost:" + port + "/Server";

            Console.WriteLine("Server URL: " + _url);
        }

        public bool canCommit()
        {
            if (_serverCanCommit)
            {
                _serverCanCommit = false;
                return true;
            }
            return false;
        }

        public void setServerCanCommit(bool newState)
        {
            _serverCanCommit = newState;
        }

    }

    public class RemoteServer : MarshalByRefObject
    {
        private enum State { normal, frozen, failing };
        private string _url, _name, _otherServerUrl;
        private State _serverState = State.normal;
        private ArrayList _calls = new ArrayList();
        private Dictionary<int, PadInt> padintList = new Dictionary<int, PadInt>();
        private ImAlive _imAlive;
        private CheckPrimaryLife _checkLife;

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
            padintList.Add(uid, new PadInt(uid, _url));
            Console.WriteLine("O int: " + uid + " foi criado");
            //informMasterNewPadInt(uid, _url);
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

        public bool revertPadIntChange(int txId, int padIntId, int oldValue)
        {
            padintList[padIntId].revertChanges(txId, oldValue);
            return true;
        }

        public bool confirmPadIntChanges(int txId, int padIntId)
        {
            padintList[padIntId].confirmChanges(txId);
            return true;
        }
        
        public void setUpServer(String name)
        {
            _name = name; // secondary or primary

            if (name == "secondary") {
                _imAlive = null; // force it to be null

                _otherServerUrl = _rMasterServer.getPrimary(_url); //url do primary server
                RemoteServer otherServer = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                _otherServerUrl);

                _checkLife = new CheckPrimaryLife(false, 2000, _otherServerUrl);
                _checkLife.setSecondaryUrl(_url);

                padintList = otherServer.updateReq(_url); //update his padint list, send secondaryserver url to start imalives
                _checkLife.start();
            }
        }
        
        //private List<Requests> _rq;
        public Dictionary<int, PadInt> updateReq(String url)
        {
            _otherServerUrl = url; //secondary server url
            _checkLife = null; //force it to be null

            _imAlive = new ImAlive(2000, _otherServerUrl);
            _imAlive.start();

            return padintList;
        }

        public void setName(String name)
        {
            _name = name;
        }

        public void printAllInts()
        {
            foreach(KeyValuePair<int,PadInt> pInt in padintList){
                Console.WriteLine("inteiro: "+pInt.Key + " valor: "+ pInt.Value.Read());
            }
        }

        public ImAlive getImAlive() { return _imAlive;}
        public CheckPrimaryLife getCheckLife() { return _checkLife;}
        public void setImAlive(ImAlive a) { _imAlive = a; }
        public void setCheckPrimaryLife(CheckPrimaryLife c) { _checkLife = c; }

    }
}
