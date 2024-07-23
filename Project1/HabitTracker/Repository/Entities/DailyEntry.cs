using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Entities;

public class DailyEntry
{
    public int DailyEntryID {get;set;}
    
    [Required]
    public DateTime EntryDate {get;set;}
    [Required]
    public bool IsCompleted {get;set;}

    [Required]
    public int HabitID {get;set;}
    public Habit Habit {get;set;}

    public override string ToString()
    {
        return $"{DailyEntryID} {EntryDate} {IsCompleted}";
    }
}