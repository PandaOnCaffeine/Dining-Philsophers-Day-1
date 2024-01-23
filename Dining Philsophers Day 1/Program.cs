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
        static int _eatCount = 0;
        static object _eatCountLock = new object();
        static object _writeLock = new object();
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
            WriteAt($"Philosopher {philosopherNumber} is thinking.", 40, "||");

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
                WriteAt($"Philosopher {philosopherNumber} picks up right fork. {leftFork}", 40, "||");

                Monitor.Enter(forks[leftFork]);
                WriteAt($"Philosopher {philosopherNumber} picks up left fork. {rightFork}", 40, "||");
            }
            else
            {
                Monitor.Enter(forks[leftFork]);
                WriteAt($"Philosopher {philosopherNumber} picks up left fork. {leftFork}", 40, "||");

                Monitor.Enter(forks[rightFork]);
                WriteAt($"Philosopher {philosopherNumber} picks up right fork. {rightFork}", 40, "||");
            }
        }

        static void Eat(int philosopherNumber)
        {
            lock (_eatCountLock)
            {
                _eatCount++;
                WriteAt($"Philosopher {philosopherNumber} is eating.", 40, $"|| Eat Count: {_eatCount}");
            }
            Thread.Sleep(3000);
        }

        static void PutDownForks(int philosopherNumber)
        {
            //false for left fork || true for right fork
            int leftFork = ForkPicker(philosopherNumber, false);
            int rightFork = ForkPicker(philosopherNumber, true);

            Monitor.Exit(forks[leftFork]);
            WriteAt($"Philosopher {philosopherNumber} puts down left fork. {leftFork}🍴 ", 40, $"||");

            Monitor.Exit(forks[rightFork]);
            WriteAt($"Philosopher {philosopherNumber} puts down right fork. {rightFork}🍴", 40, $"||");

            Thread.Sleep(500);
        }

        static int ForkPicker(int philosopherNumber, bool rightFork)
        {
            int forkNr;
            if (!rightFork)
            {
                forkNr = philosopherNumber - 1;
            }
            else
            {
                if (philosopherNumber == 1)
                {
                    forkNr = 4;
                }
                else
                {
                    forkNr = philosopherNumber - 2;
                }
            }
            return forkNr;
        }
        static void WriteAt(string startText, int x, string text)
        {
            lock (_writeLock)
            {
                Console.Write(startText);

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                int currentY = Console.CursorTop;

                Console.SetCursorPosition(x, currentY);
                Console.Write(text);

                // Move to the next line
                Console.SetCursorPosition(0, currentY + 1);

                Console.ResetColor();
            }
        }
    }
}
