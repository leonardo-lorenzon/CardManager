namespace CardManager.Domain.services;

public class TimeService : ITimeService
{
    public DateTime CurrentDate()
    {
        return DateTime.UtcNow;
    }
}
