namespace BullsnCows
{
    public struct History
    {
        public string Question { get; set; }
        public Answer Answer { get; set; }

        public History(string question, Answer answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}
