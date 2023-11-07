
using System.ComponentModel.DataAnnotations;
using System;

namespace FirstLab.src.back_end;

public class FlashcardSetLogDTO
{
    [Key]
    public DateTime Date { get; set; }
    public string PlayedSetsName { get; set; }
    public int Duration { get; set; }
}
