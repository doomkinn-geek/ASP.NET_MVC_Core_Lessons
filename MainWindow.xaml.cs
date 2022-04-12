using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace lesson1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static long fiboLast = 0;
        private static long fiboPreLast = 0;
        private static long fiboPrePreLast = 0;

        public MainWindow()
        {
            InitializeComponent();
            fiboText.Text = "HELLO";
            Thread f = new Thread(Foo);
            f.Start();
        }
        public void Foo()
        {            
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
            new Action(() =>
            {
                while (true)
                {                    
                    fiboText.Text = FiboNext().ToString();
                    Thread.Sleep(1000);
                }
            }));
        }
        static long FiboNext()
        {
            long fiboNumber = 0, fiboNext = 0;
            if (fiboLast == 0)
            {
                fiboNext = 1;
                fiboLast = 1;
                fiboPreLast = 1;
            }
            else if (fiboPreLast == 0)
            {
                fiboPreLast = 1;
                fiboPrePreLast = 1;
                fiboNext = 1;
            }
            else
            {
                fiboNext = fiboPreLast + fiboLast;
                fiboPrePreLast = fiboPreLast;
                fiboPreLast = fiboLast;
                fiboLast = fiboNext;
            }
            return fiboNext;
        }

    }
}
