using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace PADI_DSTM
{
    public class PadInt : MarshalByRefObject
    {
        private int _id;
        private int _shared;
        private int txId;
        private String _serverUrl;
        private Object _lockThisPadInt = new Object();
        private ArrayList liveReadTxs = new ArrayList();
        private ArrayList liveWriteTxs = new ArrayList();
        private ArrayList doneReadTxs = new ArrayList();
        private ArrayList doneWriteTxs = new ArrayList();
        private ArrayList failedTxs = new ArrayList();
        private ArrayList doneTxs = new ArrayList();

        public PadInt(int id, String serverUrl)
        {
            _id = id;
            _serverUrl = serverUrl;

        }

        public void setLiveReadTxs(int txId)
        {
            if(!liveReadTxs.Contains(txId)){
                liveReadTxs.Add(txId);
            }
        }

        public ArrayList getAllLocalVariables()
        {
            ArrayList arrayToReturn = new ArrayList();
            arrayToReturn.Add(_id);
            arrayToReturn.Add(_shared);
            arrayToReturn.Add(txId);
            arrayToReturn.Add(_serverUrl);
            arrayToReturn.Add(liveReadTxs);
            arrayToReturn.Add(liveWriteTxs);
            arrayToReturn.Add(doneReadTxs);
            arrayToReturn.Add(doneWriteTxs);
            arrayToReturn.Add(failedTxs);
            arrayToReturn.Add(doneTxs);
            return arrayToReturn;
        }

        public void setAllLocalVariables(ArrayList arrayData)
        {
            _id = (int) arrayData[0];
            _shared = (int) arrayData[1];
            txId = (int) arrayData[2];
            _serverUrl = (string) arrayData[3];
            liveReadTxs = (ArrayList) arrayData[4];
            liveWriteTxs = (ArrayList) arrayData[5];
            doneReadTxs = (ArrayList) arrayData[6];
            doneWriteTxs = (ArrayList) arrayData[7];
            failedTxs = (ArrayList) arrayData[8];
            doneTxs = (ArrayList) arrayData[9];
        }

        public void Write(int newValue)
        {
            lock (_lockThisPadInt)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                _serverUrl);
                server.accessPadint(_id);
                _shared = newValue;
            }
        }

        public bool WriteToServer(int newValue, int txId)
        {
            lock (_lockThisPadInt)
            {
                try
                {
                    foreach (int tx in doneTxs)
                    {
                        if (tx > txId)
                        {
                            //throw new TxException("Transação de write atrasada");
                            return false;
                        }
                    }
                    _shared = newValue;
                    confirmChanges(txId);
                    return true;
                }
                catch (TxException txE)
                {
                    Console.WriteLine(txE.Message);
                    return false;
                }
            }
        }

        public int Read()
        {

            return _shared;
        }

        public void revertChanges(int txId, int oldValue)
        {
            lock (_lockThisPadInt)
            {
                _shared = oldValue;
                liveReadTxs.Remove(txId);
                liveWriteTxs.Remove(txId);
                doneTxs.Remove(txId);
                failedTxs.Add(txId);
            }
        }

        public void confirmChanges(int txId)
        {
            liveReadTxs.Remove(txId);
            liveWriteTxs.Remove(txId);
            doneTxs.Add(txId);
        }

        public void setTxId(int newTxId){
            txId = newTxId;
        }

        public String getUrl()
        {
            return _serverUrl;
        }
    }
}