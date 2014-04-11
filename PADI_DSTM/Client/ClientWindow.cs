using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using PADI_Library;

namespace Client
{
    public partial class ClientWindow : Form
    {
        private TcpChannel _channel;
        private RemoteMasterServer _rMasterServer;
        private PadInt _copyVal;

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void InitializeRemoteMasterServer()
        {
            RemotingConfiguration.RegisterWellKnownClientType(
            typeof(RemoteMasterServer), "tcp://localhost:8086/MasterServer"); //port e ip address fixos, MasterServer
        }

        private void connectToMasterServer()
        {
            _channel = new TcpChannel();
            ChannelServices.RegisterChannel(_channel, true);
            //InitializeRemoteMasterServer();

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
        /*
         * Code to test functions responsible for the server state
         */
        private void failButton_Click(object sender, EventArgs e)
        {
            Library lib = new Library();
            lib.Freeze("tcp://localhost:1001/Server");
        }

        private void getStatusButton_Click(object sender, EventArgs e)
        {
            Library lib = new Library();
            bool res = lib.Status();
        }

        private void startConButton_Click(object sender, EventArgs e)
        {
            InitializeRemoteMasterServer();
            connectToMasterServer();

        }
    }
}
