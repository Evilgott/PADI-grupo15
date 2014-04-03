using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using PADI_Library;

namespace MasterServer
{
    class MasterServer
    {
        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel(8086);
            ChannelServices.RegisterChannel(channel, true);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemoteMasterServer),
                "MyRemoteObjectName",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> para sair...");
            System.Console.ReadLine();
        }
    }
}
