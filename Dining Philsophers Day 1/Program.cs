using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dining_Philsophers_Day_1
{
    internal class Program
    {
        static object[] forks = new object[5];

        static void Main()
        {
            for (int i = 0; i < 5; i++)
            {
                forks[i] = new object();
            }

            for (int i = 0; i < 5; i++)
            {
                int philosopherNumber = i + 1;
                Thread philosopherThread = new Thread(() => PhilosopherLife(philosopherNumber));
                philosopherThread.Start();
            }

            Console.ReadLine();
        }
        static void PhilosopherLife(int philosopherNumber)
        {
            while (true)
            {
                Think(philosopherNumber);
                PickUpForks(philosopherNumber);
                Eat(philosopherNumber);
                PutDownForks(philosopherNumber);
            }
        }
        static void Think(int philosopherNumber)
        {
            Console.WriteLine($"Philosopher {philosopherNumber} is thinking.");
            Thread.Sleep(new Random().Next(1000, 3000));
        }
        static void PickUpForks(int philosopherNumber)
        {
            //false for left fork || true for right fork
            int leftFork = ForkPicker(philosopherNumber, false); 
            int rightFork = ForkPicker(philosopherNumber, true);

            if (philosopherNumber % 2 == 0)
            {
                Monitor.Enter(forks[rightFork]);
                Console.WriteLine($"Philosopher {philosopherNumber} picks up right fork. {leftFork}");

                Monitor.Enter(forks[leftFork]);
                Console.WriteLine($"Philosopher {philosopherNumber} picks up left fork. {rightFork}");
            }
            else
            {
                Monitor.Enter(forks[leftFork]);
                Console.WriteLine($"Philosopher {philosopherNumber} picks up left fork. {leftFork}");

                Monitor.Enter(forks[rightFork]);
                Console.WriteLine($"Philosopher {philosopherNumber} picks up right fork. {rightFork}");
            }
        }

        static void Eat(int philosopherNumber)
        {
            Console.WriteLine($"Philosopher {philosopherNumber} is eating.");
            Thread.Sleep(3000); 
        }

        static void PutDownForks(int philosopherNumber)
        {
            //false for left fork || true for right fork
            int leftFork = ForkPicker(philosopherNumber,false);
            int rightFork = ForkPicker(philosopherNumber, true);

            Monitor.Exit(forks[leftFork]);
            Console.WriteLine($"Philosopher {philosopherNumber} puts down left fork. {leftFork}");

            Monitor.Exit(forks[rightFork]);
            Console.WriteLine($"Philosopher {philosopherNumber} puts down right fork. {rightFork}");

            Thread.Sleep(500);
        }

        static int ForkPicker(int philosopherNumber, bool rightFork)
        {
            int forkNr;
            if (!rightFork)
            {
                forkNr = philosopherNumber -1;
            }
            else
            {
                if (philosopherNumber == 1)
                {
                    forkNr = 4;
                }
                else
                {
                    forkNr = philosopherNumber -2;
                }
            }
            return forkNr;
        }
    }
}
