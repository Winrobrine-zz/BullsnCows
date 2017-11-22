using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullsnCows
{
    public class Game
    {
        public enum Digit
        {
            Three = 3,
            Four = 4,
        }

        List<string> _allowedNumbers;
        List<History> _history = new List<History>();

        Digit _digit;
        History _last;
        int _step;
        string _lastQuestion;

        Answer[] POTENTIAL_ANSWERS;

        public Game() : this(Digit.Four, "1234567890") { }

        public Game(Digit digit) : this(digit, "1234567890") { }

        public Game(Digit digit, string script)
        {
            _digit = digit;
            _allowedNumbers = Permutation.Create(script, (int)digit).ToList();

            switch (digit)
            {
                case Digit.Three:
                    POTENTIAL_ANSWERS = new Answer[]
                    {
                        new Answer(0, 0),
                        new Answer(0, 1),
                        new Answer(0, 2),
                        new Answer(0, 3),
                        new Answer(1, 0),
                        new Answer(1, 1),
                        new Answer(1, 2),
                        new Answer(2, 0),
                        new Answer(3, 0),
                    };
                    break;
                case Digit.Four:
                    POTENTIAL_ANSWERS = new Answer[]
                    {
                        new Answer(0, 0),
                        new Answer(0, 1),
                        new Answer(0, 2),
                        new Answer(0, 3),
                        new Answer(0, 4),
                        new Answer(1, 0),
                        new Answer(1, 1),
                        new Answer(1, 2),
                        new Answer(1, 3),
                        new Answer(2, 0),
                        new Answer(2, 1),
                        new Answer(2, 2),
                        new Answer(3, 0),
                        new Answer(4, 0),
                    };
                    break;
            }
        }

        private bool IsConsistent(string number, History history)
        {
            int bulls = 0, cows = 0;

            for (int i = 0; i < number.Length; i++)
            {
                if (number[i] == history.Question[i])
                    bulls++;
                else if (number.Contains(history.Question[i]))
                    cows++;
            }

            return bulls == history.Answer.Bulls && cows == history.Answer.Cows;
        }

        public bool IsFinished()
        {
            if (_history.Count > 0)
            {
                History last = _history.Last();

                if (!last.Equals(_last))
                {
                    _last = last;
                    _allowedNumbers.RemoveAll(e => !IsConsistent(e, last));
                }
            }

            return _allowedNumbers.Count <= 1;
        }

        public string GetQuestion()
        {
            return _lastQuestion = _allowedNumbers.RandomValue((int)Math.Min(Math.Pow(10, _step++), _allowedNumbers.Count)).AsParallel().MinBy(question => POTENTIAL_ANSWERS.Sum(answer => Math.Pow(_allowedNumbers.AsParallel().Count(e => IsConsistent(e, new History(question, answer))), 2)));
        }

        public void PutAnswer(int bulls, int cows)
        {
            PutAnswer(new Answer(bulls, cows));
        }

        public void PutAnswer(Answer answer)
        {
            _history.Add(new History(_lastQuestion, answer));
        }

        public int GetStep()
        {
            if (IsFinished())
            {
                if (_history.Last().Answer.Bulls == (int)_digit)
                    return _step;
                return _step + 1;
            }
            return _step;
        }

        public bool IsCorrect(out string result)
        {
            result = _allowedNumbers.FirstOrDefault(e => IsConsistent(e, _history.Last()));
            return result != null;
        }
    }
}
