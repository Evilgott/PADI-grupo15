using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; //for timer class
using System.Diagnostics;

namespace PADI_DSTM
{
    public class ImAlive : MarshalByRefObject
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
         
            //timer = new Timer(sendImAlive, null, imAliveTime, Timeout.Infinite);
            Console.WriteLine("im alive start");
            TimerCallback tcb = sendImAlive;
            timer = new Timer(tcb, null, imAliveTime, Timeout.Infinite);

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
            watch.Start();

            cLife.setIsAlive(true);

            
            // Long running operation
            timer.Change(Math.Max(0, imAliveTime - watch.ElapsedMilliseconds), Timeout.Infinite);
        }

        public void setTime(int time) { imAliveTime = time; }
        public void setUrl(string url) { _url = url; }
        //public void setIsEnabled(bool b) { isEnabled = b; }

    }

    public class CheckPrimaryLife : MarshalByRefObject
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
            Console.WriteLine("check life start");
            TimerCallback tcb = checkLife;
            timer = new Timer(tcb, null, _time, Timeout.Infinite);

        }

        public void stop()
        {
            timer.Dispose();
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
                    swapToPrimary();
                    _deadCount = 0;
                }
                Console.WriteLine("isDead!");
            }
            else
            {
                Console.WriteLine("isAlive!");
                _isAlive = false;
            }

            timer.Change(Math.Max(0, _time - watch.ElapsedMilliseconds), Timeout.Infinite);

        }

        public void swapToPrimary()
        {
             RemoteMasterServer _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                             typeof(RemoteMasterServer),
             "tcp://localhost:8086/MasterServer");

             _rMasterServer.swapToPrimaryServer(_url);
        }

        public void setIsAlive(bool b) { _isAlive = b; }
    }
}
