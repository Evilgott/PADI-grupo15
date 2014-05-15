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
        private string _url;

        public ImAlive(int time, string url)
        {
            imAliveTime = time;
            _url = url;
        }

        public void start()
        {

            Console.WriteLine("im alive start");
            TimerCallback tcb = sendImAlive;
            timer = new Timer(tcb, null, imAliveTime, Timeout.Infinite);

        }

        public void sendImAlive(Object source)
        {
            Stopwatch watch = new Stopwatch();
            RemoteServer server = (RemoteServer)Activator.GetObject(
            typeof(RemoteServer),
            _url); //secondary remote server
            CheckPrimaryLife cLife = server.getCheckLife();
            watch.Start();

            cLife.setIsAlive(true);
            
            timer.Change(Math.Max(0, imAliveTime - watch.ElapsedMilliseconds), Timeout.Infinite);
        }

        public void setTime(int time) { imAliveTime = time; }
        public void setUrl(string url) { _url = url; }

    }

    public class CheckPrimaryLife : MarshalByRefObject
    {
        private static Timer timer;
        private int _time;
        private bool _isAlive, _toDestroy;
        private int _deadCount;
        private string _url; //primary
        private string _secondaryUrl;

        public CheckPrimaryLife(bool b, int time, string url)
        {
            _isAlive = b;
            _time = time;
            _url = url;
            _toDestroy = false;
        }


        public void start()
        {

            Console.WriteLine("check life start");
            TimerCallback tcb = checkLife;
            timer = new Timer(tcb, null, _time, Timeout.Infinite);

        }

        public void stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            timer.Dispose();
        }

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
                    _toDestroy = true;
                    swapToPrimary();
                    _deadCount = 0;
                }
                Console.WriteLine("isDead!");
            }
            else
            {
                Console.WriteLine("isAlive!");
                _deadCount = 0;
                _isAlive = false;
            }

            if (!_toDestroy)
            timer.Change(Math.Max(0, _time - watch.ElapsedMilliseconds), Timeout.Infinite);
            else
                stop();
        }

        public void swapToPrimary()
        {
             RemoteMasterServer _rMasterServer = (RemoteMasterServer)Activator.GetObject(
                             typeof(RemoteMasterServer),
             "tcp://localhost:8086/MasterServer");

             _rMasterServer.swapToPrimaryServer(_url, _secondaryUrl);
        }

        public void setIsAlive(bool b) { _isAlive = b; }

        public void setSecondaryUrl(string url) { _secondaryUrl = url; }
        public string getSecondaryUrl() { return _secondaryUrl; }
    }
}
