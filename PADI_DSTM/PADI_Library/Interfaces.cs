using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace iPADI
{
    public class RemoteMasterServer : MarshalByRefObject
    {
        private int _tId = 0;
        private List<string> _primaryServers = new List<string>();
        private List<string> _secondaryServers = new List<string>();
        private Dictionary<int, Tuple<string,string>> _servers;

        public String registerServer(String serverURL)
        {
            if (_primaryServers.Count % 2 == 0)
            {
                _primaryServers.Add(serverURL);
                return "primary";
            }
            else
            {
                _secondaryServers.Add(serverURL);
                return "secondary";
            }
                
        }

        public ArrayList getAllServersURLs()
        {
            ArrayList mergedArray = new ArrayList();

            foreach(String url in _primaryServers){
                mergedArray.Add(url);
            }
            foreach(String url in _secondaryServers){
                mergedArray.Add(url);
            }

            return mergedArray;
        }

        public Tuple<String, String> getUrl(int uid) {
            if (_servers.ContainsKey(uid))
            {
                return _servers[uid];
            }
            else {
                return new Tuple<string, string>(_primaryServers[0], _primaryServers[0]);
            }
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
        private Dictionary<int, PadInt> padintList; 

        public bool changeServerState(String newState)
        {
            if (newState == "frozen") _serverState = State.frozen;
            else if (newState == "fail") _serverState = State.failing;
            else if (newState == "recover") _serverState = State.normal;
            return true;
        }

        public void getServerStatus()
        {
            Console.WriteLine( _serverState.ToString());
        }

        public string getUrl() {
            return _url;
        }

        public PadInt createPadint(int uid)
        {
            padintList[uid] = new PadInt();
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
