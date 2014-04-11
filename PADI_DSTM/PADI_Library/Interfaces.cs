using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PADI_Library
{
    public class RemoteMasterServer : MarshalByRefObject
    {
        private int _tId;
        private Dictionary<PadInt, Dictionary<RemoteServer, RemoteServer>> _servers;
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
