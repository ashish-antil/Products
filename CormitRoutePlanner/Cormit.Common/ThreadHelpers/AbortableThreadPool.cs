using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Imarda.Lib
{
    public class AbortableThreadPool
    {
        private readonly EventWaitHandle _Done = new AutoResetEvent(false);
        private readonly string _Prefix;
        private readonly object _Sync = new object();
        private HashSet<ThreadControl> _Busy;
        private int _Capacity;
        private Stack<ThreadControl> _Free;
        private Thread _Recycler;
        private volatile bool _Run;

        public AbortableThreadPool(string name)
        {
            _Prefix = name;
        }

        public AbortableThreadPool(string name, int minThreads, int maxThreads)
        {
            _Prefix = name;
            Start(minThreads, maxThreads);
        }

        public void Start(int minThreads, int maxThreads)
        {
            lock (_Sync)
            {
                if (_Recycler != null && _Recycler.IsAlive) throw new InvalidOperationException("Thread pool already initialized");
                _Capacity = maxThreads;
                _Free = new Stack<ThreadControl>(maxThreads);
                _Busy = new HashSet<ThreadControl>();
                _Run = true;
                _Recycler = new Thread(RecyclerAborter) { Name = _Prefix + "-recycler", IsBackground = true };
                _Recycler.Start();
                for (int i = 0; i < minThreads; i++)
                {
                    var ctl = new ThreadControl(_Prefix, this);
                    _Free.Push(ctl);
                    ctl.Start();
                }
            }
        }


        /// <summary>
        /// Run by recycler thread.
        /// </summary>
        private void RecyclerAborter()
        {
            try
            {
                while (_Run)
                {
                    //DebugLog.Write("Check status of Busy threads");
                    lock (_Sync)
                    {
                        Array.ForEach(_Busy.ToArray(),
                            delegate(ThreadControl ctl)
                            {
                                DateTime now = DateTime.UtcNow;
                                if (now > ctl.Due) ctl.Status = ThreadControlStatus.TimedOut;

                                //DebugLog.Write(() => ctl.Status != JobStatus.Busy, ".. thread {0} took {1} ms to get {2} on {3}", ctl.Thread.Name, ctl.Duration(), ctl.Status, ctl.Arg);

                                switch (ctl.Status)
                                {
                                    case ThreadControlStatus.Done:
                                    case ThreadControlStatus.Exception:
                                        //DebugLog.Write(".. recycle {0}", ctl);
                                        if (ctl.Thread.IsAlive) _Free.Push(ctl);
                                        _Busy.Remove(ctl);
                                        ctl.OnCompletion(ctl.Status, ctl.Exception);
                                        ctl.Status = ThreadControlStatus.New;
                                        ctl.Due = DateTime.MaxValue;
                                        break;
                                    case ThreadControlStatus.TimedOut:
                                        //DebugLog.Write(".. abort {0}", ctl);
                                        _Busy.Remove(ctl);
                                        ctl.Thread.Abort(); // throw a ThreadAbortException into the worker thread
                                        ctl.OnCompletion(ctl.Status, new TimeoutException("Atom Processing Time Out", ctl.Exception));
                                        ctl.Status = ThreadControlStatus.Aborted;
                                        break;
                                }
                            }
                            );
                    }
                    _Done.WaitOne(1000); // time out value is here to detect hanging jobs that don't want to do Done.Set()
                }
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// Run by each ThreadControl.Thread
        /// </summary>
        /// <param name="obj"></param>
        internal void ProcessWork(object obj)
        {
            bool loop = true;
            var ctl = (ThreadControl)obj;
            while (loop)
            {
                ctl.Go.WaitOne();
                if (!_Run)
                {
                    //DebugLog.Write("Break " + Thread.CurrentThread.Name);
                    break;
                }
                try
                {
                    //DebugLog.Write("ProcessWork: BEGIN {0}", ctl.Arg);

                    ctl.Begin = DateTime.UtcNow;
                    ctl.Status = ThreadControlStatus.Busy;
                    ctl.Exception = null;

                    ctl.Proc.Invoke(ctl.Arg);

                    ctl.Status = ThreadControlStatus.Done;
                    //DebugLog.Write("ProcessWork: END {0}", ctl.Arg);
                    _Done.Set();
                }
                catch (ThreadAbortException)
                {
                    //DebugLog.Write("Thread Abort Exception");
                    loop = false;
                    Thread.ResetAbort();
                }
                catch (Exception ex)
                {
                    ctl.Status = ThreadControlStatus.Exception;
                    ctl.Exception = ex;
                    //DebugLog.Write("ProcessWork: EXC {0} {1}", ctl.Arg, ex.Message);
                    _Done.Set();
                }
            }

        }

        public void Close()
        {
            lock (_Sync)
            {
                _Run = false;
                foreach (ThreadControl w in _Busy) w.Go.Set();
                foreach (ThreadControl w in _Free) w.Go.Set();
            }
        }


        /// <summary>
        /// Get an abortable thread from the pool. Wait until Run() is called on the returned IThreadControl.
        /// </summary>
        /// <param name="work">actual work to be executed by this thread</param>
        /// <param name="arg">application arg</param>
        /// <param name="done">called when the work is finished</param>
        /// <returns>control object for the abortable thread</returns>
        public IThreadControl GetThread(WaitCallback work, object arg, CompletionCallback done)
        {
            ThreadControl ctl;
            lock (_Sync)
            {
                if (_Free.Count > 0)
                {
                    ctl = _Free.Pop();
                    ctl.Proc = work;
                    ctl.Arg = arg;
                }
                else if (_Busy.Count < _Capacity)
                {
                    ctl = new ThreadControl(_Prefix, this) { Proc = work, Arg = arg };
                    ctl.Start();
                }
                else return null;
                _Busy.Add(ctl);
            }
            ctl.OnCompletion = done;
            return ctl;
        }
    }
}