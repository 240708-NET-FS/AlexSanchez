using HabitTracker.Entities;

namespace HabitTracker.DAO;

public class HabitDAO : IDAO<Habit>
{
    private ApplicationDbContext _context;

    public HabitDAO(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(Habit item)
    {
        _context.Habits.Add(item);
        _context.SaveChanges();
    }

    public void Delete(Habit item)
    {
        _context.Habits.Remove(item);
        _context.SaveChanges();
    }

    public ICollection<Habit> GetAll()
    {
        List<Habit> habits = _context.Habits.ToList();

        return habits;
    }

    public Habit GetById(int ID)
    {
        Habit habit = _context.Habits.FirstOrDefault(h => h.HabitID == ID);

        return habit;
    }

    public void Update(Habit newItem)
    {
        Habit originalHabit = _context.Habits.FirstOrDefault(h => h.HabitID == newItem.HabitID);

        if (originalHabit != null)
        {
            originalHabit.Name = newItem.Name;
            originalHabit.Description = newItem.Description;
            originalHabit.StartDate = newItem.StartDate;

            _context.Habits.Update(originalHabit);
            _context.SaveChanges();
        }

    }
}