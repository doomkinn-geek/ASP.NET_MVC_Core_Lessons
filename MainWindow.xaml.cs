using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace lesson1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private long fiboLast = 0;
        private long fiboPreLast = 0;
        private long fiboPrePreLast = 0;

        private static object lockObj = new object();
        private int interval = 1000;

        public MainWindow()
        {
            InitializeComponent();            
        }
        public void Foo()
        {
            lock (lockObj)
            {
                for (int i = 0; i < 1000; i++)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        Thread.Sleep(interval);
                        fiboText.Text = FiboNext().ToString();
                    }));
                }
            }
        }
        private long FiboNext()
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
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {          
            new Thread(Foo).Start();
        }

        private void intervalText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {            
            lock(lockObj)
            {
                int seconds;
                if(int.TryParse(intervalText.Text.ToString(), out seconds))
                {
                    interval = 1000 * seconds;
                }                
            }
        }
    }
}
