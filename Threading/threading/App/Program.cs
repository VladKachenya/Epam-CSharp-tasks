using System;
using System.Collections.Generic;
using System.Threading;

namespace App
{

    

    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            var service = new Service();
            int lastId = 0;

            ThreadStart get = () =>
            {
                Console.WriteLine("t1 started");
                do
                {
                    for (int i = 1; i < lastId; i++)
                    {
                        if (exit)
                        {
                            Console.WriteLine("t2 ended");
                            return;
                        }
                        Console.WriteLine("Get({0})", i);
                        service.Get(i);
                    }
                    if (exit)
                    {
                        Console.WriteLine("t2 ended");
                        return;
                    }
                } while (true);
            };

            var threadGet = new Thread[10];

            for (var i = 0; i < 10; i++)
            {
                threadGet[i] = new Thread(get);
                threadGet[i].Start();
            }

            ThreadStart set = () =>
            {
                do
                {
                    var id = Interlocked.Increment(ref lastId);

                    var user = new User()
                    {
                        Id = id,
                        // other fields
                    };
                    service.Add(user);

                } while (!exit);
            };

            var threadWrite = new Thread(set);
            threadWrite.Start();

            Console.WriteLine("Hello World!");
            Console.ReadKey();

            exit = true;

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}