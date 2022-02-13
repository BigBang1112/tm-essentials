namespace TmEssentials;

public interface ITime : IComparable, IComparable<TimeSpan>
{
    public int Days { get; }
    public int Hours { get; }
    public int Milliseconds { get; }
    public int Minutes { get; }
    public int Seconds { get; }
    public long Ticks { get; }
    public float TotalDays { get; }
    public float TotalHours { get; }
    public float TotalMilliseconds { get; }
    public float TotalMinutes { get; }
    public float TotalSeconds { get; }

    ITime Add(ITime ts);
    //static int Compare(ITime t1, ITime t2);
    ITime Divide(float divisor);
    float Divide(ITime ts);
    ITime Duration();
    ITime Multiply(float factor);
    ITime Negate();
}
