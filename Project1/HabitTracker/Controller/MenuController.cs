using HabitTracker.Service;

namespace HabitTracker.Controller;

public class MenuController
{
    private readonly MenuService _menuService;
    
    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    public void printMenu()
    {
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Daily Entry.");
        Console.WriteLine("Type 3 to Delete Daily Entry.");
        Console.WriteLine("Type 4 to Update Daily Entry.");
        Console.WriteLine("Type 5 to Insert Habit.");
        Console.WriteLine("Type 6 to Delete Habit.");
        Console.WriteLine("Type 7 to Update Habit.");
        Console.WriteLine("---------------------------------------\n");
    }

    public void ExecuteAction()
    {
        int action = getAction();
        Console.Clear();
        _menuService.ExecuteAction(action);
    }

    public int getAction()
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (input != null && _menuService.isValidAction(input, out int choice))
            {
                return choice;
            }

            Console.WriteLine("Please input a number between 0-7.");
        }
    }
}
