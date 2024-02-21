namespace Seminar_3
{
    internal class Program
    {
        public static void Method()
        {
            for (int i = 0; i < 80; i++)
            {
                Thread.Sleep(100);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("-");
                throw new Exception();
            }
        }

        //помечаем ключевым словом async метод
        public async static Task MethodAsync()
        {

            //Эта часть кода будет работать в основном потоке
            //создаем задачу передаем в делегат метод Method
            Task t = new Task(Method);
            //запускаем задачу
            t.Start();


            //а вот эта часть завершится в 2 потоке
            //ожидаем завершения задачи
            //await t;
        }

        public async static Task<int> Method2()
        {
            throw new Exception();
        }
 
        static void Main(string[] args)
        {
            var t = Method2();
            Console.WriteLine("Main завершился");
            Console.ReadKey();
            var result = t.Result;
            Console.WriteLine(result);





            int x = 0;
            object locker = new();  // объект-заглушка
                                    // запускаем пять потоков
            for (int i = 1; i < 6; i++)
            {
                Thread myThread = new(Print);
                myThread.Name = $"Поток {i}";
                myThread.Start();
            }

            async void Print()
            {
                lock (locker)
                {
                    x = 1;
                    for (int i = 1; i < 6; i++)
                    {
                        var res = Method2().Result;
                        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
                        x++;
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }

    class Reader
    {
        // создаем семафор
        static Semaphore sem = new Semaphore(3, 3);
        Thread myThread;
        int count = 3;// счетчик чтения

        public Reader(int i)
        {
            myThread = new Thread(Read);
            myThread.Name = $"Читатель {i}";
            myThread.Start();
        }

        public async void Read()
        {
            while (count > 0)
            {
                sem.WaitOne();  // ожидаем, когда освободиться место

                await Method1();

                Console.WriteLine($"{Thread.CurrentThread.Name} входит в библиотеку");

                Console.WriteLine($"{Thread.CurrentThread.Name} читает");
                Thread.Sleep(1000);

                Console.WriteLine($"{Thread.CurrentThread.Name} покидает библиотеку");

                sem.Release();  // освобождаем место

                count--;
                Thread.Sleep(1000);
            }
        }
        static async Task<int> Method1()
        {
            int res = 0;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Method 1: {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(2000);
                res++;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(res);
            }
            return res;
        }
    }
}
