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
            _channel = new TcpChannel(8086);
            ChannelServices.RegisterChannel(_channel, true);
            InitializeRemoteMasterServer();

            _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                typeof(RemoteMasterServer),
                "tcp://localhost:8086/MasterServer");
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
