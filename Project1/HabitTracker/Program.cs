using HabitTracker.Controller;
using HabitTracker.DAO;
using HabitTracker.Entities;
using HabitTracker.Service;

namespace HabitTracker;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new ApplicationDbContext())
        {
            // Instantiate DAOs
            DailyEntryDAO dailyEntryDao = new DailyEntryDAO(context);
            HabitDAO habitDao = new HabitDAO(context);

            // Instantiate Services
            HabitService habitService = new HabitService(habitDao);
            DailyEntryService dailyEntryService = new DailyEntryService(dailyEntryDao);

            // Instantiate MenuService
            MenuService menuService = new MenuService(habitService, dailyEntryService);

            // Instantiate MenuController with MenuService
            MenuController menuController = new MenuController(menuService);

            while (true)
            {
                menuController.printMenu();
                menuController.ExecuteAction();
            }

        }
    }
}
