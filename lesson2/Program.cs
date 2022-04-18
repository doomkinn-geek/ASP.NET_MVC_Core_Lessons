// See https://aka.ms/new-console-template for more information
/*int nWorkerThreads;
int nCompletionThreads;
ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionThreads);
Console.WriteLine("Максимальное количество потоков: " + nWorkerThreads
    + "\nПотоков ввода-вывода доступно: " + nCompletionThreads);
for (int i = 0; i < 5; i++)
    ThreadPool.QueueUserWorkItem(JobForAThread);
Thread.Sleep(3000);

Console.ReadLine();

static void JobForAThread(object state)
{
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine("цикл {0}, выполнение внутри потока из пула {1}",
            i, Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(50);
    }
}*/

using lesson2;

using (var pool = new SimplePool(3))
{
    var random = new Random();
    Action<int> randomizer = (index =>
    {
        Console.WriteLine("{0}: Работа с индексом {1}", Thread.CurrentThread.Name, index);
        Thread.Sleep(random.Next(20, 400));
        Console.WriteLine("{0}: Завершение {1}", Thread.CurrentThread.Name, index);
    });

    for (var i = 0; i < 40; ++i)
    {
        var i1 = i;
        pool.QueueTask(() => randomizer(i1));
    }
}
