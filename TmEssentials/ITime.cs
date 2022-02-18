namespace TmEssentials;

public interface ITime : IComparable, IComparable<ITime>
{
    int Days { get; }
    int Hours { get; }
    int Milliseconds { get; }
    int Minutes { get; }
    int Seconds { get; }
    long Ticks { get; }
    float TotalDays { get; }
    float TotalHours { get; }
    float TotalMilliseconds { get; }
    float TotalMinutes { get; }
    float TotalSeconds { get; }
    bool IsNegative { get; }

    ITime Add(ITime ts);
    //static int Compare(ITime t1, ITime t2);
    ITime Divide(float divisor);
    float Divide(ITime ts);
    ITime Duration();
    ITime Multiply(float factor);
    ITime Negate();
}
