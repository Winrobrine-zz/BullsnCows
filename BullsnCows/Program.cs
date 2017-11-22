using System;
using System.Linq;

namespace BullsnCows
{
    class Program
    {
        static Game game;

        static void InteractiveGame()
        {
            game = new Game(Game.Digit.Three);

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
            int stepSum;
            int stepMin, stepMax;
            int[] distribution = new int[10];

            var permutations = Permutation.Create("1234567890", 4);

            stepSum = 0;
            stepMin = int.MaxValue;
            stepMax = int.MinValue;

            string[] numbers = permutations.Shuffle().ToArray();

            for (int n = 0; n < numbers.Length; n++)
            {
                string number = numbers[n];

                game = new Game(Game.Digit.Four);

                while (!game.IsFinished())
                {
                    string question = game.GetQuestion();

                    int bulls = 0, cows = 0;

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (number[i] == question[i])
                            bulls++;
                        else if (number.Contains(question[i]))
                            cows++;
                    }

                    game.PutAnswer(bulls, cows);
                }

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

                Console.WriteLine("#{0} num: {1} avg: {2} best: {3} worst: {4}", n + 1, number, stepSum / (double)(n + 1), stepMin, stepMax);
            }

            for (int i = 1; i <= 9; i++)
                Console.WriteLine(distribution[i]);
        }

        static void Main(string[] args)
        {
            while (true)
            {
                //InteractiveGame();
                TestNumbers();
            }
        }
    }
}
