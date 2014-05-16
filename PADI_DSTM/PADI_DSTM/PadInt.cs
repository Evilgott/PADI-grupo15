using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
        private ArrayList failedTxs = new ArrayList();
        private ArrayList doneTxs = new ArrayList();

        public PadInt(int id, String serverUrl)
        {
            _id = id;
            _serverUrl = serverUrl;
        }

        public void Write(int newValue)
        {
            lock (_lockThisPadInt)
            {
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