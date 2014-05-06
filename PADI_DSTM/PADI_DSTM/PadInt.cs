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
        private int shared;
        private ArrayList liveReadTxs = new ArrayList();
        private ArrayList liveWriteTxs = new ArrayList();
        private ArrayList failedTxs = new ArrayList();
        private ArrayList doneTxs = new ArrayList();

        public void Write(int newValue)
        {
            //locks
            shared = newValue;
        }

        public int Read()
        {
            return shared;
        }

        public void revertChanges(int txId, int oldValue)
        {
            shared = oldValue;
            liveReadTxs.Remove(txId);
            liveWriteTxs.Remove(txId);
            failedTxs.Add(txId);
        }

        public void confirmChanges(int txId)
        {
            liveReadTxs.Remove(txId);
            liveWriteTxs.Remove(txId);
            doneTxs.Add(txId);
        }
    }
}