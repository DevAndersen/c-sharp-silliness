DateTime pastMax = default;

unsafe
{
    // Create a span over pastMax, and set all its bytes to their maximum value (255).
    Span<byte> span = new Span<byte>(&pastMax, sizeof(DateTime));
    span.Fill(byte.MaxValue);
}

Console.WriteLine($"DateTime.MaxValue: {DateTime.MaxValue}");
Console.WriteLine($"PastMax DateTime:  {pastMax}");

// This will be "true", and that just feels deeply wrong, doesn't it?
bool isGreaterThanMax = pastMax > DateTime.MaxValue;

Console.WriteLine($"Is the PastMax DateTime greater than DateTime.MaxValue? {isGreaterThanMax}");
Console.WriteLine($"PastMax minus 10.000 years is {pastMax.AddYears(-10000)}");

try
{
    // This will fail. What a bummer.
    Console.WriteLine($"PastMax minus 1 year is {pastMax.AddYears(-1)}");
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine($"""
        Nope, didn't work.
        Calculations that result in a DateTime out of the expected range throws an {nameof(ArgumentOutOfRangeException)} with the message:
        "{e.Message}"
        """);
}
