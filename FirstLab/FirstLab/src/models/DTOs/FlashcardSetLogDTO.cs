using System.ComponentModel.DataAnnotations;
using System;

namespace FirstLab.src.models.DTOs;

public class FlashcardSetLogDTO
{
    [Key]
    public DateTime Date { get; set; }
    public string PlayedSetsName { get; set; }
    public int Duration { get; set; }
}
