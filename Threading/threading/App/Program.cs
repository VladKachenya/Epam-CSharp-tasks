using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            Console.WriteLine("Press any key to start");
            Console.ReadKey();

            ThreadStart get = () =>
            {
                Console.WriteLine("t1 started");
                do
                {
                    for (int i = 1; i < lastId + 1; i++)
                    {
                        if (exit)
                        {
                            Console.WriteLine("t2 ended");
                            return;
                        }
                        var receivedUser = service.Get(i);
                        Console.WriteLine("Get({0})", receivedUser != null ? receivedUser.Id.ToString() : "User not found");
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
                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        service.Add(user);
                        Console.WriteLine($"User with Id {user.Id} added (ms: {sw.ElapsedMilliseconds})");
                        sw.Stop();
                    }
                    catch (UserExistsException e)
                    {
                        Console.WriteLine($"User with Id {user.Id} already added");
                    }

                } while (!exit);
            };

            var threadWrite = new Thread(set);
            threadWrite.Start();
            Console.ReadKey();

            exit = true;

            Console.ReadKey();
        }
    }
}