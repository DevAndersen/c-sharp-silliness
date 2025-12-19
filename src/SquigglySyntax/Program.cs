// "Hello, World!"
Squiggler t = new Squiggler(13);

t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~t;
t = +~~!t;
t = +~~!+~+~+~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~+~t;
t = +~~!+~+~+~t;
t = +~~!-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~t;
t = +~~!-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~t;

Console.WriteLine([.. t.Values.Select(x => (char)x)]);

public class Squiggler
{
    private ModeKind _mode;
    private int _position;
    private int _value;

    public byte[] Values { get; init; }

    public Squiggler(int size)
    {
        Values = new byte[size];
    }

    /// <summary>
    /// Increments the mode of <paramref name="t"/>.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Squiggler operator ~(Squiggler t)
    {
        t._mode++;
        return t;
    }

    /// <summary>
    /// Makes <paramref name="t"/> perform the operation according to its mode, with the specified <paramref name="relative"/> value.
    /// </summary>
    /// <param name="t"></param>
    /// <param name="relative"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static Squiggler PerformRelativeOperation(Squiggler t, int relative)
    {
        switch (t._mode)
        {
            case ModeKind.Undefined:
                throw new Exception("Undefined mode.");

            case ModeKind.Change:
                t._value += (byte)relative;
                break;

            case ModeKind.Move:
                t._position += relative;
                break;

            default:
                throw new Exception($"Unexpected mode {t._mode}.");
        }

        t._mode = ModeKind.Undefined;

        return t;
    }

    /// <summary>
    /// Makes <paramref name="t"/> performs an incrementation operation, depending on its mode.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Squiggler operator +(Squiggler t) => PerformRelativeOperation(t, 1);

    /// <summary>
    /// Makes <paramref name="t"/> performs a decrementing operation, depending on its mode.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Squiggler operator -(Squiggler t) => PerformRelativeOperation(t, -1);

    /// <summary>
    /// Stores the current value of <paramref name="t"/> into its current position.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Squiggler operator !(Squiggler t)
    {
        t.Values[t._position] = (byte)t._value;
        return t;
    }

    private enum ModeKind
    {
        Undefined,
        Change,
        Move,
    }
}
