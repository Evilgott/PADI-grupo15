using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using PADI_Library;

namespace Client
{
    public class Client
    {
        private TcpChannel _channel;


        Client()
        {
            _channel = new TcpChannel();
            ChannelServices.RegisterChannel(_channel, true);
        }

        public RemoteMasterServer connectMaster()
        {
            return (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:1001/MasterServer");
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Client cl = new Client();

            RemoteMasterServer obj = cl.connectMaster(); 


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
