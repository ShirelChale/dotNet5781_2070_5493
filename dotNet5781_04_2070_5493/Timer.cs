using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace dotNet5781_04_2070_5493
{
    class Timer
    {
        private Stopwatch stoper;
        private int stopCounter;
        private BackgroundWorker watchWorker;
        private string completedMessage;

        public Timer()
        {
            Stoper = new Stopwatch();
            WatchWorker = new BackgroundWorker();
            WatchWorker.DoWork += WatchWorker_DoWork;
            //WatchWorker.ProgressChanged += WatchWorker_ProgressChanged;
            WatchWorker.WorkerReportsProgress = true;
            watchWorker.RunWorkerCompleted += WatchWorker_RunWorkerCompleted;
           // completedMessage = _completedMessage;
        }

        private void WatchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CompletedMessage = completedMessage;
            object x = e.Result;
            string str= x as string;
        }

        public int StopCounter
        {
            get => stopCounter;
            set => stopCounter = value;
        }
        public Stopwatch Stoper
        { 
            get => stoper; 
            set => stoper = value; 
        }
        public BackgroundWorker WatchWorker
        { 
            get => watchWorker; 
            set => watchWorker = value; 
        }
        public string CompletedMessage
        { 
            get => completedMessage; 
            set => completedMessage = value; 
        }

        private void WatchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string timerText = Stoper.Elapsed.ToString();
            timerText = timerText.Substring(0, 8);

        }

        private void WatchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (StopCounter >= 0)
            {
                WatchWorker.ReportProgress(1);
                System.Threading.Thread.Sleep(1000);
                StopCounter--;
            }
            Stoper.Stop();
            return;
        }

        public override string ToString()
        {
            string timerText = Stoper.Elapsed.ToString();
            timerText = timerText.Substring(0, 8);
            return timerText;
        }

    }

}
