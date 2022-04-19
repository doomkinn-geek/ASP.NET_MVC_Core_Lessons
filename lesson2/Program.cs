// See https://aka.ms/new-console-template for more information

using lesson2;

using (var pool = new SimplePool(7))
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
        pool.QueueTask(() => randomizer(i));
    }
}
