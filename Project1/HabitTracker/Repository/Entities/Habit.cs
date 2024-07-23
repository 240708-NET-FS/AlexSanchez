using System.ComponentModel.DataAnnotations;

namespace HabitTracker.Entities;

public class Habit
{
    public int HabitID {get;set;}
    
    [Required]
    public string Name {get;set;}
    public string Description {get;set;}

    [Required]
    public DateTime StartDate {get;set;}

    public ICollection<DailyEntry> DailyEntries {get;set;} = new List<DailyEntry>();

    public override string ToString()
    {
        return $"{HabitID} {Name} {Description} {StartDate}";
    }
}