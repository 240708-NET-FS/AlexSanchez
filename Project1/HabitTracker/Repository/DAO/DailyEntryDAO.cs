using HabitTracker.Entities;

namespace HabitTracker.DAO;

public class DailyEntryDAO : IDAO<DailyEntry>
{
    // CRUD
    
    private ApplicationDbContext _context;

    public DailyEntryDAO(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Create(DailyEntry item)
    {
        _context.DailyEntries.Add(item);
        _context.SaveChanges();
    }

    public void Delete(DailyEntry item)
    {
        _context.DailyEntries.Remove(item);
        _context.SaveChanges();
    }

    public ICollection<DailyEntry> GetAll()
    {
        List<DailyEntry> dailyEntries = _context.DailyEntries.ToList();
        return dailyEntries;
    }

    public DailyEntry GetById(int ID)
    {
        DailyEntry dailyEntry = _context.DailyEntries.FirstOrDefault(d => d.DailyEntryID == ID);
        return dailyEntry;
    }

    public void Update(DailyEntry newItem)
    {
        DailyEntry originalEntry = _context.DailyEntries.FirstOrDefault(d => d.DailyEntryID == newItem.DailyEntryID);

        if (originalEntry != null)
        {
            originalEntry.EntryDate = newItem.EntryDate;
            originalEntry.IsCompleted = newItem.IsCompleted;
            _context.DailyEntries.Update(originalEntry);
            _context.SaveChanges();
        }
    }
}