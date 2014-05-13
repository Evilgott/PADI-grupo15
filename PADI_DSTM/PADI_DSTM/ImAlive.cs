using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PADI_DSTM
{
    class ImAlive
    {
        private static Timer timer = new Timer();
        private double imAliveTime;
        private bool isEnabled = false;
        private string _url;

        public void start()
        {
            timer.Elapsed += new ElapsedEventHandler(sendImAlive);
            timer.Interval = imAliveTime;
            isEnabled = true;
            timer.Enabled = isEnabled;
        }

        //esta funcao tem de criar uma mensagem e enviar para o url
        //o servidor destino tem de ter um handler destas mensagens
        public void sendImAlive(object source, ElapsedEventArgs e)
        {

        }

        public void setTime(double time) { imAliveTime = time; }
        public void setUrl(string url) { _url = url; }
        public void setIsEnabled(bool b) { isEnabled = b; }

    }

    class CheckPrimaryLife
    {
        private static Timer timer;
        private double time { get; set; }
        private bool isAlive { get; set; }
        private bool isEnabled { get; set; }
        private static CheckPrimaryLife instance;

        CheckPrimaryLife(bool b)
        {
            isAlive = b;
            timer = new Timer();
        }

        public static CheckPrimaryLife Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CheckPrimaryLife(false);
                }
                return instance;
            }
        }

        public void start()
        {
            timer.Elapsed += new ElapsedEventHandler(checkLife);
            timer.Interval = time;
            isEnabled = true;
            timer.Enabled = isEnabled;
        }

        public void checkLife(object source, ElapsedEventArgs e)
        {
            if (!isAlive)
            {
                //mudar de secundario para primario
            }
        }
    }
}
