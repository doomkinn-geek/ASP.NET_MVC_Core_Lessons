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
        private long fiboNumber = 0;
        public MainWindow()
        {
            InitializeComponent();
            Foo();
        }
        public void Foo()
        {
            Thread.Sleep(200);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
            new Action(() =>
            {
                fiboText.Text = fib(fiboNumber++);
            }));
        }
        static long fib(long n)
        {
            return n > 1 ? fib(n - 1) + fib(n - 2) : n;
        }

    }
}
