using System;
using System.Linq;
using System.Threading;

namespace BullsnCows
{
    class Program
    {
        static void InteractiveGame()
        {
            Game game = new Game(Game.Digit.Four);

            while (!game.IsFinished())
            {
                Console.WriteLine("Question #{0}: {1}", game.GetStep() + 1, game.GetQuestion());

                int[] answer = Console.ReadLine().Split(' ').Select(e => int.Parse(e)).ToArray();

                game.PutAnswer(answer[0], answer[1]);
            }

            string result;

            if (game.IsCorrect(out result))
                Console.WriteLine("Your number is {0}. It took me {1} steps to guess it.", result, game.GetStep());
            else
                Console.WriteLine("It seems that you've made a mistake somewhere.");

            Console.ReadLine();
        }

        static void TestNumbers()
        {
            int stepSum = 0;
            int stepMin = int.MaxValue, stepMax = int.MinValue;
            int[] distribution = new int[10];

            var numbers = Permutation.Create("1234567890", 4).Shuffle().ToList();

            for (int n = 0; n < numbers.Count; n++)
            {
                string number = numbers[n];

                Game game = new Game(Game.Digit.Four);

                while (!game.IsFinished())
                    game.PutAnswer(game.GetAnswer(number, game.GetQuestion()));

                string result;

                if (!game.IsCorrect(out result))
                {
                    Console.WriteLine("Error");
                    Console.ReadLine();
                }

                int step = game.GetStep();
                stepSum += step;
                stepMin = Math.Min(stepMin, step);
                stepMax = Math.Max(stepMax, step);
                distribution[step]++;

                Console.Clear();

                for (int i = 1; i <= 9; i++)
                    Console.WriteLine(distribution[i]);

                Console.WriteLine("#{0} num: {1} avg: {2} best: {3} worst: {4}", n + 1, number, stepSum / (double)(n + 1), stepMin, stepMax);
            }

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //new Thread(new ThreadStart(InteractiveGame)).Start();
            new Thread(new ThreadStart(TestNumbers)).Start();
        }
    }
}
