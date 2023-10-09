using System;

namespace FirstLab.src.back_end
{
    public record FlashcardSetLog
    {
        public string PlayedSetsName { get; init; }
        public DateTime Date { get; init; }
        public int Duration { get; init; }
        public FlashcardSetLog(string playedSetsName, DateTime date, int duration)
        {
            PlayedSetsName = playedSetsName;
            Date = date;
            Duration = duration;
        }
    }
}
