using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson2
{
    public sealed class SimplePool : IDisposable
    {
        public SimplePool(int size)
        {
            this._workers = new LinkedList<Thread>();
            for (var i = 0; i < size; ++i)
            {
                var worker = new Thread(this.Worker) { Name = string.Concat("Обработчик ", i) };
                worker.Start();
                this._workers.AddLast(worker);
            }
        }

        public void Dispose()
        {
            var waitForThreads = false;
            lock (this._tasks)
            {
                if (!this._disposed)
                {
                    GC.SuppressFinalize(this);

                    this._disallowAdd = true;
                    while (this._tasks.Count > 0)
                    {
                        Monitor.Wait(this._tasks);
                    }

                    this._disposed = true;
                    Monitor.PulseAll(this._tasks);
                    waitForThreads = true;
                }
            }
            if (waitForThreads)
            {
                foreach (var worker in this._workers)
                {
                    worker.Join();
                }
            }
        }

        public void QueueTask(Action task)
        {
            lock (this._tasks)
            {
                if (this._disallowAdd) { throw new InvalidOperationException("Данный поток в процессе уничтожения. Невозможно запустить операцию снова"); }
                if (this._disposed) { throw new ObjectDisposedException("Данный поток уже был уничтожен"); }
                this._tasks.AddLast(task);
                Monitor.PulseAll(this._tasks); // pulse because tasks count changed
            }
        }

        private void Worker()
        {
            Action task = null;
            while (true) 
            {
                lock (this._tasks)
                {
                    while (true)
                    {
                        if (this._disposed)
                        {
                            return;
                        }
                        if (null != this._workers.First && object.ReferenceEquals(Thread.CurrentThread, this._workers.First.Value) && this._tasks.Count > 0)
                        {
                            task = this._tasks.First.Value;
                            this._tasks.RemoveFirst();
                            this._workers.RemoveFirst();
                            Monitor.PulseAll(this._tasks);
                            break; 
                        }
                        Monitor.Wait(this._tasks);
                    }
                }

                task(); // process the found task
                lock (this._tasks)
                {
                    this._workers.AddLast(Thread.CurrentThread);
                }
                task = null;
            }
        }

        private readonly LinkedList<Thread> _workers; 
        private readonly LinkedList<Action> _tasks = new LinkedList<Action>(); 
        private bool _disallowAdd; 
        private bool _disposed; 
    }
}
