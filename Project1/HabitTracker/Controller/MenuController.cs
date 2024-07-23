namespace HabitTracker;

public class Menu
{
    public void printMenu()
    {
        Console.WriteLine("MAIN MENU\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("Type 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Record.");
        Console.WriteLine("Type 4 to Update Record.");
        Console.WriteLine("---------------------------------------\n");
    }

    public int getAction()
    {
        while (true)
        {

            string? input = Console.ReadLine();

            if (input != null && isValidAction(input, out int choice))
            { 
                return choice;
            }

            Console.WriteLine("Please input a number between 0-4.");
        }
    }

    private bool isValidAction(string input, out int choice)
    {
        if (int.TryParse(input, out choice))
            {
                if (choice >= 0 && choice <= 4)
                {
                    return true;
                }
            }
        return false;
    }
}
