using HabitTracker.Entities;
using HabitTracker.Service;
using HabitTracker.DAO;

namespace HabitTracker.Tests
{
    public class MenuServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly HabitService _habitService;
        private readonly DailyEntryService _dailyEntryService;
        private readonly MenuService _menuService;

        public MenuServiceTests()
        {
            _context = new ApplicationDbContext();
            var habitDao = new HabitDAO(_context);
            var dailyEntryDao = new DailyEntryDAO(_context);

            _habitService = new HabitService(habitDao);
            _dailyEntryService = new DailyEntryService(dailyEntryDao);
            _menuService = new MenuService(_habitService, _dailyEntryService);

            SeedData();
        }

        private void SeedData()
        {
            if (!_context.Habits.Any())
            {
                _context.Habits.Add(new Habit { Name = "Sample Habit", Description = "Sample Description", StartDate = DateTime.Now });
                _context.SaveChanges();
            }
        }

        public void Dispose()
        {
            // Clean up the database after each test
            var habits = _context.Habits.ToList();
            _context.Habits.RemoveRange(habits);

            var dailyEntries = _context.DailyEntries.ToList();
            _context.DailyEntries.RemoveRange(dailyEntries);

            _context.SaveChanges();
            _context.Dispose();
        }

        [Fact]
        public void InsertDailyEntry_WhenEntryExistsForToday_ShouldNotAllowMultipleEntries()
        {
            // Arrange
            var habit = _context.Habits.First();
            var dailyEntry = new DailyEntry
            {
                EntryDate = DateTime.Now.Date,
                IsCompleted = true,
                Habit = habit
            };
            _context.DailyEntries.Add(dailyEntry);
            _context.SaveChanges();

            // Simulate user input
            SimulateUserInput(habit.HabitID.ToString() + Environment.NewLine + "true");

            // Act
            var output = CaptureConsoleOutput(() => _menuService.InsertDailyEntry());

            // Assert
            Assert.Contains("A daily entry for this habit already exists for today.", output);
        }

        [Fact]
        public void InsertDailyEntry_WithValidInput_ShouldAllowSingleEntry()
        {
            // Arrange
            var habit = _context.Habits.First();

            // Simulate user input
            SimulateUserInput(habit.HabitID.ToString() + Environment.NewLine + "true");

            // Act
            var output = CaptureConsoleOutput(() => _menuService.InsertDailyEntry());

            // Assert
            var entry = _context.DailyEntries.FirstOrDefault(e => e.Habit.HabitID == habit.HabitID && e.EntryDate.Date == DateTime.Now.Date);
            Assert.NotNull(entry);
            Assert.Contains("Daily Entry Added", output);
        }

        [Fact]
        public void InsertHabit_WithNullOrWhitespaceName_ShouldNotCreateHabit()
        {
            // Simulate user input
            SimulateUserInput(Environment.NewLine + "Test Description");

            // Act
            var output = CaptureConsoleOutput(() => _menuService.InsertHabit());

            // Assert
            Assert.Contains("Habit name cannot be empty.", output);
            Assert.DoesNotContain(_context.Habits, h => h.Description == "Test Description");
        }

        [Fact]
        public void InsertHabit_WithValidName_ShouldCreateHabit()
        {
            // Simulate user input
            SimulateUserInput("Test Habit" + Environment.NewLine + "Test Description");

            // Act
            var output = CaptureConsoleOutput(() => _menuService.InsertHabit());

            // Assert
            var habit = _context.Habits.FirstOrDefault(h => h.Name == "Test Habit");
            Assert.NotNull(habit);
            Assert.Contains("Habit added.", output);
        }

        private void SimulateUserInput(string input)
        {
            var stringReader = new StringReader(input);
            Console.SetIn(stringReader);
        }

        private string CaptureConsoleOutput(Action action)
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            action.Invoke();
            return stringWriter.ToString();
        }
    }
}