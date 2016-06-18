using System;
using System.Threading;

namespace MEAM
{
    public abstract class Bee
    {
        private Thread _thread = null;
        protected readonly Hive _hive;

        protected Random rand = new Random();

        protected Bee(Hive hive)
        {
            _hive = hive;
        }

        protected abstract void Behavior();

        private void Runner()
        {
            while (true)
            {
                Thread.Sleep(100);
                Behavior();
            }
        }

        public void Run()
        {
            _thread = new Thread(Runner);
            _thread.Start();
        }

        public void Stop()
        {
            _thread?.Abort();
            _thread = null;
        }
    }
}