using CardManager.Domain.services;

namespace CardManager.Tests.doubles.faker;

public class InMemoryTimeService : ITimeService
{
    private DateTime _currentDate = DateTime.UtcNow;

    public DateTime CurrentDate()
    {
        return _currentDate;
    }

    // only for test purposes
    public void SetCurrentDate(DateTime currentDate)
    {
        _currentDate = currentDate;
    }
}
