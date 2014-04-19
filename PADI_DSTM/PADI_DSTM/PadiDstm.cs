using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using iPADI;

namespace PADI_DSTM
{
    public class PadiDstm
    {
        //URL = "tcp : // <ip-address >:< port > /Server"
        //TODO:
        /*
        bool Init();
        bool TxBegin();
        bool TxCommit();
        bool TxAbort();
        */
        public bool Status()
        {

            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteMasterServer masterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");

            ArrayList allServersURLs = masterServer.getAllServersURLs();

            foreach (String url in allServersURLs)
            {
                RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                url);

                server.getServerStatus();
            }

            ChannelServices.UnregisterChannel(channel);
            return true;
        }
        public bool Fail(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("fail");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        public bool Freeze(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("frozen");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        public bool Recover(string URL)
        {
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, true);

            RemoteServer server = (RemoteServer)Activator.GetObject(
                typeof(RemoteServer),
                URL);

            bool res = server.changeServerState("recover");

            ChannelServices.UnregisterChannel(channel);
            return res;
        }

        /*
        PadInt CreatePadInt(int uid);
        PadInt AccessPadInt(int uid);
         * */
    }
}
