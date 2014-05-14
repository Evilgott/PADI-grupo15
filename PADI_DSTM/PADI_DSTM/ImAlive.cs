using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; //for timer class
using System.Diagnostics;

namespace PADI_DSTM
{
    class ImAlive
    {
        private static Timer timer;
        private int imAliveTime;
        //private bool isEnabled = false;
        private string _url;

        public ImAlive(int time, string url)
        {
            imAliveTime = time;
            _url = url;
        }

        public void start()
        {
            /*
            timer.Elapsed += new ElapsedEventHandler(sendImAlive);
            timer.Interval = imAliveTime;
            isEnabled = true;
            timer.Enabled = isEnabled;
             */
            timer = new Timer(sendImAlive, null, imAliveTime, Timeout.Infinite);
        }

        //esta funcao tem de criar uma mensagem e enviar para o url
        //o servidor destino tem de ter um handler destas mensagens
        //public void sendImAlive(object source, ElapsedEventArgs e)
        public void sendImAlive(Object source)
        {
            Stopwatch watch = new Stopwatch();
            RemoteServer server = (RemoteServer)Activator.GetObject(
            typeof(RemoteServer),
            _url); //secondary remote server
            CheckPrimaryLife cLife = server.getCheckLife();
            cLife.setIsAlive(true);

            watch.Start();
            // Long running operation

            timer.Change(Math.Max(0, imAliveTime - watch.ElapsedMilliseconds), Timeout.Infinite);
        }

        public void setTime(int time) { imAliveTime = time; }
        public void setUrl(string url) { _url = url; }
        //public void setIsEnabled(bool b) { isEnabled = b; }

    }

    class CheckPrimaryLife
    {
        private static Timer timer;
        private int _time;
        private bool _isAlive;
        private int _deadCount;
        private string _url;

        public CheckPrimaryLife(bool b, int time, string url)
        {
            _isAlive = b;
            _time = time;
            _url = url;
        }


        public void start()
        {
            /*
            timer.Elapsed += new ElapsedEventHandler(checkLife);
            timer.Interval = time;
            isEnabled = true;
            timer.Enabled = isEnabled;
             */
            timer = new Timer(checkLife, null, _time, Timeout.Infinite);

        }

        //public void checkLife(object source, ElapsedEventArgs e)
        public void checkLife(Object source)
        {
            Stopwatch watch = new Stopwatch();
            RemoteServer server = (RemoteServer)Activator.GetObject(
            typeof(RemoteServer),
            _url); //secondary remote server

            watch.Start();

            if (!_isAlive)
            {
                _deadCount++;
                if (_deadCount >= 3) //mudar de secundario para primario
                {
                    //mudar de secundario para primario TODO!!!
                    _deadCount = 0;
                }
                
            }
            else
            {
                _isAlive = false;
            }

            timer.Change(Math.Max(0, _time - watch.ElapsedMilliseconds), Timeout.Infinite);

        }

        public void setIsAlive(bool b) { _isAlive = b; }
    }
}
