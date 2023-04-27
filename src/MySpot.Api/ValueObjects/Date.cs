namespace MySpot.Api.ValueObjects;

public sealed record Date
{
    public DateTimeOffset Value { get; }
    
    public Date(DateTimeOffset value)
    {
        Value = value;
    }
    
    public Date AddDays(int days)
        => new (Value.AddDays(days));
    
    
    public static implicit operator DateTimeOffset(Date date) => date.Value;
    
    public static implicit operator Date(DateTimeOffset value) => new (value);
    
    
    public static bool operator <(Date left, Date right) => left.Value < right.Value;

    public static bool operator >(Date left, Date right) => left.Value > right.Value;
    
    public static bool operator <=(Date left, Date right) => left.Value <= right.Value;
    
    public static bool operator >=(Date left, Date right) => left.Value >= right.Value;


    public static Date Now => new(DateTimeOffset.Now);
    
    //  Override
    public override string ToString() => Value.ToString("d");
}