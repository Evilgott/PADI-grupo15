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
using PADI_DSTM;
using iPADI;

namespace Client
{
    public partial class ClientWindow : Form
    {
        private PadInt _copyVal;

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void create(){
           
        }

        private void ClientWindow_Load(object sender, EventArgs e)
        {

        }
        /*
         * Code to test functions responsible for the server state
         */
        private void failButton_Click(object sender, EventArgs e)
        {
            PadiDstm.Freeze("tcp://localhost:1001/Server");
        }

        private void getStatusButton_Click(object sender, EventArgs e)
        {
            PadiDstm.Status();
        }

        private void startConButton_Click(object sender, EventArgs e)
        {

        }
    }
}
