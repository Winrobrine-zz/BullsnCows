namespace BullsnCows
{
    public struct Answer
    {
        public int Bulls { get; private set; }
        public int Cows { get; set; }

        public Answer(int bulls, int cows)
        {
            Bulls = bulls;
            Cows = cows;
        }
    }
}
