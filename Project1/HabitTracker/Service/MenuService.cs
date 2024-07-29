using System.Windows.Markup;
using HabitTracker.Entities;

namespace HabitTracker.Service;

public class MenuService
{
    private readonly IService<Habit> _habitService;
    private readonly IService<DailyEntry> _dailyEntryService;

    public MenuService(IService<Habit> habitService, IService<DailyEntry> dailyEntryService)
    {
        _habitService = habitService;
        _dailyEntryService = dailyEntryService;
    }

    public bool isValidAction(string input, out int choice)
    {
        if (int.TryParse(input, out choice) && choice >= 0 && choice <= 7)
        {
            return true;
        }
        return false;
    }

    public void ExecuteAction(int action)
    {
        switch (action)
        {
            case 0:
                CloseApplication();
                break;
            case 1:
                ShowAndReturnToMenu(ViewAllRecords);
                break;
            case 2:
                ShowAndReturnToMenu(InsertDailyEntry);
                break;
            case 3:
                ShowAndReturnToMenu(DeleteDailyEntry);
                break;
            case 4:
                ShowAndReturnToMenu(UpdateDailyEntry);
                break;
            case 5:
                ShowAndReturnToMenu(InsertHabit);
                break;
            case 6:
                ShowAndReturnToMenu(DeleteHabit);
                break;
            case 7:
                ShowAndReturnToMenu(UpdateHabit);
                break;
            default:
                Console.WriteLine("Invalid action.");
                break;
        }
    }

    public void CloseApplication()
    {
        Console.WriteLine("Closing Application...");
        Environment.Exit(0);
    }

    public void ShowAndReturnToMenu(Action action)
    {
        Console.Clear();
        action.Invoke();
        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
        Console.Clear();
    }

    public void ViewAllRecords()
    {
        var habits = _habitService.GetAll();

        if (habits == null || habits.Count == 0)
        {
            Console.WriteLine("No habits found.");
            return;
        }

        Console.WriteLine("Habits and Daily Entries:");
        foreach (var habit in habits)
        {
            Console.WriteLine($"\nHabit: {habit.Name} (ID: {habit.HabitID})");
            Console.WriteLine($"Description: {habit.Description}");
            Console.WriteLine($"Start Date: {habit.StartDate.ToShortDateString()}");

            var dailyEntries = _dailyEntryService.GetAll().Where(de => de.Habit.HabitID == habit.HabitID).ToList();

            if (dailyEntries.Count > 0)
            {
                foreach (var entry in dailyEntries)
                {
                    Console.WriteLine($"  - Date: {entry.EntryDate.ToShortDateString()}, Completed: {entry.IsCompleted}, ID: {entry.DailyEntryID}");
                }
            }
            else
            {
                Console.WriteLine("  No entries found for this habit.");
            }
        }
    }

    public void InsertDailyEntry()
    {
        int habitId = GetHabitIdFromUser();
        if (habitId == -1) return;


        var habit = _habitService.GetById(habitId);
        if (habit == null)
        {
            Console.WriteLine("Habit not found");
            return;
        }

        var existingEntry = _dailyEntryService.GetAll()
            .FirstOrDefault(de => de.Habit.HabitID == habitId && de.EntryDate.Date == DateTime.Now.Date);
        if (existingEntry != null)
        {
            Console.WriteLine("A daily entry for this habit already exists for today.");
            return;
        }

        bool? isCompletedNullable = GetCompletionStatusFromUser();
        if (isCompletedNullable == null) return;

        bool isCompleted = isCompletedNullable.Value;

        DailyEntry newEntry = new DailyEntry
        {
            EntryDate = DateTime.Now,
            IsCompleted = isCompleted,
            Habit = habit
        };

        _dailyEntryService.Create(newEntry);
        Console.WriteLine("Daily Entry Added");
    }

    public void DeleteDailyEntry()
    {
        int entryId = GetDailyEntryIdFromUser();
        if (entryId == -1) return;

        var entry = _dailyEntryService.GetById(entryId);
        if (entry == null)
        {
            Console.WriteLine("Daily entry not found.");
            return;
        }

        if (ConfirmAction($"Are you sure you want to delete the entry for {entry.Habit.Name} on {entry.EntryDate}?"))
        {
            _dailyEntryService.Delete(entry);
            Console.WriteLine("Daily entry deleted.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }

    public void UpdateDailyEntry()
    {
        int entryId = GetDailyEntryIdFromUser();
        if (entryId == -1) return;

        var entry = _dailyEntryService.GetById(entryId);
        if (entry == null)
        {
            Console.WriteLine("Daily entry not found.");
            return;
        }

        UpdateEntryDetails(entry);
    }

    public void UpdateEntryDetails(DailyEntry entry)
    {
        int habitId = GetHabitIdFromUser();
        if (habitId != -1)
        {
            var habit = _habitService.GetById(habitId);
            if (habit != null)
            {
                entry.Habit = habit;
            }
            else
            {
                Console.WriteLine("Habit not found");
                return;
            }
        }

        DateTime? newDate = GetNewDateFromUser();
        if (newDate != null)
        {
            entry.EntryDate = newDate.Value;
        }

        bool? isCompleted = GetCompletionStatusFromUser();
        if (isCompleted != null)
        {
            entry.IsCompleted = isCompleted.Value;
        }

        _dailyEntryService.Update(entry);
        Console.WriteLine("Daily entry updated.");
    }

    public void InsertHabit()
    {
            Console.WriteLine("Enter Habit Name:");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Habit name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter Habit Description:");
            string description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(description))
            {
                description = "No description provided.";
            }

            Habit newHabit = new Habit
            {
                Name = name,
                Description = description,
                StartDate = DateTime.Now
            };

            _habitService.Create(newHabit);
            Console.WriteLine("Habit added.");
    }

    public void DeleteHabit()
    {
        int habitId = GetHabitIdFromUser();
        if (habitId == -1) return;

        var habit = _habitService.GetById(habitId);
        if (habit == null)
        {
            Console.WriteLine("Habit not found.");
            return;
        }

        if (ConfirmAction($"Are you sure you want to delete the habit '{habit.Name}'?"))
        {
            _habitService.Delete(habit);
            Console.WriteLine("Habit deleted.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }

    public void UpdateHabit()
    {
        int habitId = GetHabitIdFromUser();
        if (habitId == -1) return;

        var habit = _habitService.GetById(habitId);
        if (habit == null)
        {
            Console.WriteLine("Habit not found.");
            return;
        }

        Console.WriteLine("Enter new Habit Name (or press Enter to keep current):");
        string name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name))
        {
            habit.Name = name;
        }

        Console.WriteLine("Enter new Habit Description (or press Enter to keep current):");
        string description = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(description))
        {
            habit.Description = description;
        }

        _habitService.Update(habit);
        Console.WriteLine("Habit updated.");
    }

    private int GetHabitIdFromUser()
    {
        Console.WriteLine("Enter Habit ID:");
        if (int.TryParse(Console.ReadLine(), out int habitId))
        {
            return habitId;
        }
        Console.WriteLine("Invalid Habit ID.");
        return -1;
    }

    private int GetDailyEntryIdFromUser()
    {
        Console.WriteLine("Enter Daily Entry ID:");
        if (int.TryParse(Console.ReadLine(), out int entryId))
        {
            return entryId;
        }
        Console.WriteLine("Invalid Entry ID.");
        return -1;
    }

    private bool? GetCompletionStatusFromUser()
    {
        Console.WriteLine("Is the habit completed today? (true/false):");
        if (bool.TryParse(Console.ReadLine(), out bool isCompleted))
        {
            return isCompleted;
        }
        Console.WriteLine("Invalid input for completion status.");
        return null;
    }

    private DateTime? GetNewDateFromUser()
    {
        Console.WriteLine("Enter new date (yyyy-mm-dd) (or press enter to keep current):");
        string dateInput = Console.ReadLine();
        if (DateTime.TryParse(dateInput, out DateTime newDate))
        {
            return newDate;
        }
        return null;
    }

    private bool ConfirmAction(string message)
    {
        Console.WriteLine($"{message} (yes/no):");
        return Console.ReadLine()?.ToLower() == "yes";
    }

}