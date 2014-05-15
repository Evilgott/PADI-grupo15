using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PADI_DSTM
{
    [Serializable]
    class TxException : RemotingException, ISerializable
    {
        private string _internalMessage;

        public TxException()
        {
            _internalMessage = String.Empty;
        }

        public TxException(string message)
        {
            _internalMessage = message;
        }
        /*
        public TxException(string message, Exception inner)
            : base(message, inner)
        {
        }
        */
        public TxException(SerializationInfo info, StreamingContext context)
        {
            _internalMessage = (string)info.GetValue("_internalMessage", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
             info.AddValue("_internalMessage", _internalMessage);
        }

         // Returns the exception information. 
         public override string Message
         {
             get
             {
                 return _internalMessage;
             }
         }
    }
}