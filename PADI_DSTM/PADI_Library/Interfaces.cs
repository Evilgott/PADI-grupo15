using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace PADI_Library
{
    public class RemoteMasterServer : MarshalByRefObject
    {
        private int _tId;
        private ArrayList _primaryServers = new ArrayList();
        private ArrayList _secondaryServers = new ArrayList();
        private Dictionary<PadInt, Tuple<RemoteServer,RemoteServer>> _servers;

        public bool registerServer(String serverURL)
        {
            if (_primaryServers.Count % 2 == 0)
            {
                _primaryServers.Add(serverURL);
                return true;
            }
            else
            {
                _secondaryServers.Add(serverURL);
                return false;
            }
                
        }
    }

    public class RemoteServer : MarshalByRefObject
    {
        private enum _state { normal, frozen, failing };
        private string _url;
        //private Dictionary<PadInt, TX> _wTX; write transactions
        //private Dictionary<PadInt, TX> _rTX; read transactions
        //private List<Requests> _rq;
    }

    public class Library
    {
        //URL = "tcp : // <ip-address >:< port > /Server"
        //TODO:
        /*
        bool Init();
        bool TxBegin();
        bool TxCommit();
        bool TxAbort();
        bool Status();
        bool Fail(string URL);
        bool Freeze(string URL);
        bool Recover(string URL);

        PadInt CreatePadInt(int uid);
        PadInt AcessPadInt(int uid);
         * */
    }
}
