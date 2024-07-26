using HabitTracker.DAO;
using HabitTracker.Entities;

namespace HabitTracker.Service;

public class DailyEntryService : IService<DailyEntry>
{
    private readonly DailyEntryDAO _dailyDAO;

    public DailyEntryService(DailyEntryDAO dailyDAO)
    {
        _dailyDAO = dailyDAO;
    }

    public DailyEntry GetById(int id)
    {
        return _dailyDAO.GetById(id);
    }

    public ICollection<DailyEntry> GetAll()
    {
        return _dailyDAO.GetAll();
    }

    public void Create(DailyEntry dailyEntry)
    {
        _dailyDAO.Create(dailyEntry);
    }

    public void Delete(DailyEntry dailyEntry)
    {
        _dailyDAO.Delete(dailyEntry);
    }

    public void Update(DailyEntry dailyEntry)
    {
        _dailyDAO.Update(dailyEntry);
    }
}