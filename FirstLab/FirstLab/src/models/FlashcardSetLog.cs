using System;
using System.ComponentModel.DataAnnotations;

namespace FirstLab.src.models;

public record FlashcardSetLog
{
    [Key]
    public DateTime Date { get; init; }
    public string PlayedSetsName { get; init; }
    public int Duration { get; init; }
    public FlashcardSetLog(string playedSetsName, DateTime date, int duration)
    {
        PlayedSetsName = playedSetsName;
        Date = date;
        Duration = duration;
    }
}
