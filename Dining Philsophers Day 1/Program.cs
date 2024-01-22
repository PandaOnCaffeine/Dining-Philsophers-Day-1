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
        static Random _rng = new Random();
        static object[] _forks = new object[5];
        static void Main(string[] args)
        {
            Thread[] philsophers = new Thread[5];
            for (int i = 0; i < philsophers.Length; i++)
            {
                philsophers[i] = new Thread(() => PhilsophersLife(i));
                philsophers[i].Start();
            }
            Console.ReadLine();
        }
        static void PhilsophersLife(int nr)
        {
            int rightFork;
            int leftFork;
            int philsopherNr;
            switch (nr)
            {
                case 0:
                    rightFork = 4;
                    leftFork = 0;
                    philsopherNr = 0;
                    break;
                case 1:
                    rightFork = 0;
                    leftFork = 1;
                    philsopherNr = 1;
                    break;
                case 2:
                    rightFork = 1;
                    leftFork = 2;
                    philsopherNr = 2;
                    break;
                case 3:
                    rightFork = 2;
                    leftFork = 3;
                    philsopherNr = 3;
                    break;
                case 4:
                    rightFork = 3;
                    leftFork = 4;
                    philsopherNr = 4;
                    break;
                default:
                    rightFork = 0;
                    leftFork = 0;
                    philsopherNr = 0;
                    break;
            }
            while (true)
            {
                Think(philsopherNr);
                Eat(rightFork, leftFork, philsopherNr);
            }
        }
        static void Think(int philsopherNr)
        {
            int thinkTime = _rng.Next(1000, 5000);
            Console.WriteLine("Philsopher " + philsopherNr + " is now Thinking For:" + (thinkTime/1000) + " Seconds");

            Thread.Sleep(thinkTime);
        }
        static void Eat(int rightFork, int leftFork, int philsopherNr)
        {
            if (_forks[rightFork] == false && _forks[leftFork] == false)
            {
                lock (_forks[1])
                {
                    _forks[rightFork] = true;
                    _forks[leftFork] = true;
                    Console.WriteLine("Philsopher " + philsopherNr + " is now Eating");
                    Thread.Sleep(3000);
                    _forks[rightFork] = false;
                    _forks[leftFork] = false;
                }
            }
        }
    }
}
