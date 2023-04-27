namespace MySpot.Api.ValueObjects;

public sealed record Week
{
    public Date From { get; }
    public Date To { get; }

    public Week(DateTimeOffset value)
    {
        var pastDays = value.DayOfWeek is DayOfWeek.Sunday ? 7 : (int) value.DayOfWeek;
        var futureDays = 7 - pastDays;
        
        From = new Date(value.AddDays(-pastDays));
        To = new Date(value.AddDays(futureDays));
    } 
    
    //  Override
    public override string ToString() => $"{From} - {To}";
}