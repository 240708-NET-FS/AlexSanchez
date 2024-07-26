using HabitTracker.DAO;
using HabitTracker.Entities;

namespace HabitTracker.Service;

public class HabitService : IService<Habit>
{
    private readonly HabitDAO _habitDAO;

    public HabitService(HabitDAO habitDAO)
    {
        _habitDAO = habitDAO;
    }

    public Habit GetById(int id)
    {
        return _habitDAO.GetById(id);
    }

    public ICollection<Habit> GetAll()
    {
        return _habitDAO.GetAll();
    }

    public void Create(Habit habit)
    {
        _habitDAO.Create(habit);
    }

    public void Delete(Habit habit)
    {
        _habitDAO.Delete(habit);
    }

    public void Update(Habit habit)
    {
        _habitDAO.Update(habit);
    }
}