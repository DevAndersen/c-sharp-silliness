// You know it's gonna be good when the code starts with some warning suppressions.
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

WriteLine([.. Hell(), O1(), Comma(), Space(), W(), O2(), R(), L(), D(), ExclamationMark()]);

file partial class Program
{
    /// <summary>
    /// Topic: TBD.
    /// </summary>
    /// <returns></returns>
    private static char[] Hell()
    {
        int veryBigNumber = 270_291_474; // Todo: Find a way to land at this number.

        // This gives us the number 270,291,474, which, if represented as a 32-bit integer value,
        // can be hashed using MD5 to yield a digest that, if read as UTF-8 text, just so happens to start with the four letters "hell".
        // How do I know this? Because I bruteforced it. Took one of my CPU cores around 40 seconds before it found a match.
        // Also, having some part of the code literally return "hell" seems fitting, considering the absolute abomination that is this project.
        byte[] hellBytes = MD5.HashData(BitConverter.GetBytes(veryBigNumber));

        // Now, the "h" is lower-case, so we'll need to make it upper-case.
        // Here we can use a really handy design feature of ASCII/Unicode: the binary difference between any
        // Latin letter's upper- and lower case variants is a single bit (the sixth bit, to be precise).
        // We can therefore take any Latin upper-case letter, XOR it with its lower-case counterpart to get the case bit,
        // and then AND that with any other Latin letter to make it upper-case.
        // Just gotta add a cast because XOR'ing two chars returns an int, and also wrapt in with the "unchecked" keyword since we're converting a negative int to a byte.
        hellBytes[0] &= unchecked((byte)~('A' ^ 'a'));

        // Now we just need to turn our four first bytes into chars, and return them.
        // But, since .NET uses UTF-16, we'll need to cast the four first bytes as chars,
        // and then turn that as a char array. A pinch of LINQ, a dash of collection expressions, and the meal is ready to be served!
        return [.. hellBytes.Take(4).Select(x => (char)x)];
    }

    /// <summary>
    /// Topic: TBD.
    /// </summary>
    /// <returns></returns>
    private static char O1()
    {
        return '_';
    }

    /// <summary>
    /// Topic: Duck typing, contextual keywords.
    /// </summary>
    /// <returns></returns>
    private static char Comma()
    {
        // Some C# language features, such as "foreach" and "await", are implemented via duck typing.
        // This means that, as long as a type defines the correct members, you can use it for those features.
        // Going further, you can implement these on existing types by using extension methods.
        // So, what if we made good old int both awaitable and enumerable?

        // Interestingly, the "using" keyword when used in using statements typically doesn't support duck typing.
        // If you want to use it, the type has to actually implement IDisposable or IAsyncDisposable.
        // However, because ref structs couldn't implement interfaces up until C# 13 (and still can't be cast as an interface as that would cause them to be boxed),
        // there's a special case which allows duck typing the IDisposable or IAsyncDisposable pattern when doing so on a ref struct.

        // Another fun fact about C#: some keywords are "contextual".
        // This means that they can sometimes be used elsewhere in code, for example as identifiers.
        // So, not only can you name a variable things like "var" or "async", but you can also name a type "var" or "async".
        // This is one of the reasons why you shouldn't have all-lowercase type names.

        using var var = Task.Run<async>(async () =>
        {
            await using var var = await Task.Run(async () =>
            {
                // This is, obviously, the number 5.
                nint nint = default(var) + await sizeof(int);

                async var = default;
                // This is basically just a wonky looking for-loop. Sort of. Trust me, it just works.
                await foreach (int async in await await (int)nint)
                {
                    // Also, turns out that if you have an awaitable type which, when awaited, returns an awaited type (e.g. itself),
                    // you can chain as many "await" keywords as you want.
                    var ^= -await async & await (await await await async * ~await await async);
                }
                return var;
            });

            return var;
        }).GetAwaiter().GetResult();

        return var;
    }

    /// <summary>
    /// Topic: TBD.
    /// </summary>
    /// <returns></returns>
    private static char Space()
    {
        return '_';
    }

    /// <summary>
    /// Topic: Reflection, IL emit.
    /// </summary>
    /// <returns></returns>
    private static char W()
    {
        // For this letter, we'll simply write a method that returns the character.
        // Nothing overly complicated, just a simple little method.
        // ... which we'll write in runtime-emitted Intermediate Language (IL), because why the hell not!
        const char Character = 'W';

        const string AssemblyName = nameof(AssemblyName);
        const string ModuleName = nameof(ModuleName);
        const string TypeName = nameof(TypeName);
        const string MethodName = nameof(MethodName);

        // Alright, so we need to define a public static method that returns our char.
        // But before that, we'll first need to define an assembly, to contain the module, to contain the type, to contain the method.
        // With so many layers, this ought to qualify as one of those fancy layered designs that make software architects happy.
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(AssemblyName), AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(ModuleName);
        TypeBuilder typeBuilder = moduleBuilder.DefineType(TypeName, TypeAttributes.Public);
        MethodBuilder methodBuilder = typeBuilder.DefineMethod(MethodName, MethodAttributes.Public | MethodAttributes.Static, typeof(char), []);

        // So, let's write the IL for a method body that returns a char.
        // Is this the intended way one would write such a method in IL? I very much doubt it.
        // Does it work? Yes, and that's all that matters.
        ILGenerator ilGenerator = methodBuilder.GetILGenerator();

        // Loads the character onto the stack. I *think* this is the correct operation to use here?
        ilGenerator.Emit(OpCodes.Ldc_I4_S, Character);

        // Return with whatever is left on the stack (our character).
        // The CLR gets rather upset if you forget to do this.
        // "BadImageException" my ass...
        ilGenerator.Emit(OpCodes.Ret);

        // Everything is set up, now we just need to create our type.
        Type type = typeBuilder.CreateType();
        // IT'S ALIVE!!!

        // With all that done, we simply need to invoke our newly generated method, and return the returned char.
        // Let's also suppress those pesky null warnings. Shut up, compiler, I know what I'm doing!
        // Fun fact: the null-forgiving operator is also known as the "dammit" operator, since you're essentially telling the compiler "just do it, dammit".
        return (char)type.GetMethod(MethodName)!.Invoke(null, [])!;
    }

    /// <summary>
    /// Topic: IEEE 754, bitwise operators.
    /// </summary>
    /// <returns></returns>
    private static char O2()
    {
        // First, let's take the lower-case letters 'i' and 'w', and melt them together.
        // This gives us the number 127, or 01111111 in binary.
        byte b = 'i' | 'w';

        // Now, let's left-shift those bits eight times, and we get 32512.
        // What a lovely number, consisting of 32 and 512. Nice and round numbers, or at least, they are in binary.
        short s = (short)(b << 8);

        // Next, let's reinterpret this 16-bit integer as a 16-bit floating point number. In case you didn't know, .NET has those nowadays.
        // It's half the size of a float (technically called a "Single"), hence the name.
        Half half = BitConverter.Int16BitsToHalf(s);

        // According to IEEE 754, if bytes 2 through 9 are all set to 1, that number is not a number. Literally, it's NaN.
        // You'd normally only end up with NaN if you divide zero with zero, but we got there via binary reinterpretation instead.
        string nan = half.ToString();

        // And finally, let's index into the "NaN" string, and shuffle the bytes around until we get something useful.
        // We'll also index into the characters back to front (that's what the '^' in the indexers does), simply because we can.
        // This is not to be confused with binary XOR, which also uses '^' as its operator.

        // Since binary NOT ('~') simply flips all the bits, using it an uneven number of times is the same as using it once.
        // Similarly, using it an even number of times does nothing, as it just flips the bits back to their initial state.

        // And, what do you know, we bit manipulated the characters of "NaN" into a lower-case 'o'.
        // Turns out you really can do anything if you set your mind to it.
        return (char)~~(~~~nan[^3] & nan[^2] ^ nan[^1]);
    }

    /// <summary>
    /// Topic: String mutation.
    /// </summary>
    /// <returns></returns>
    private static char R()
    {
        // First, let's create a string. Nothing out of the ordinary here.
        // We'll even make it a compile-time constant, so you know it's super-duper not gonna change.
        const string Text = "Please don't mutate me";

        // Next, let's create a ReadOnlyMemory over the text. Again, perfectly normal.
        ReadOnlyMemory<char> readOnlyMemory = Text.AsMemory();

        // Now, let's turn that ReadOnlyMemory<char> into a Memory<char> and get a Span<char> over it.
        // This gives us read-write access to the string's buffer.
        // Look ma! No unsafe!
        Span<char> span = MemoryMarshal.AsMemory(readOnlyMemory).Span;

        // And finally, we'll change the first character in the span to the lower-case letter 'r'.
        // String mutation is a big no-no in .NET, so this technically qualifies as data corruption.
        // Luckily, we don't use this string elsewhere, so we're technically good.
        span[0] = 'r';

        // And the cherry on top: because the string is known at compile time, it gets interned.
        // This means that all strings with that exact same content will all point to the same location in memory.
        // And since we just mutated the underlying buffer, that means we implicitly also mutated all other instances of that string. Spooky action at a distance!
        // So while it looks like we are returning a 'P', we are actually returning an 'r'. Highly illegal stuff, don't tell your parents.
        return "Please don't mutate me"[0];
    }

    /// <summary>
    /// Topic: TDB.
    /// </summary>
    /// <returns></returns>
    private static char L()
    {
        return '_';
    }

    /// <summary>
    /// Topic: TBD.
    /// </summary>
    /// <returns></returns>
    private static char D()
    {
        return '_';
    }

    /// <summary>
    /// Topic: TBD.
    /// </summary>
    /// <returns></returns>
    private static char ExclamationMark()
    {
        return '_';
    }

    /// <summary>
    /// Topic: Function pointers.
    /// </summary>
    /// <param name="text"></param>
    private static unsafe void WriteLine(char[] text)
    {
        // And now, all we need to do is to write our string to the console.
        // We'll of course use Console.WriteLine for this, but calling it directly seems rather anticlimactic, doesn't it?
        // Let's create a function pointer and invoke it that way. After all, needless complexity is the name of the game here!
        delegate*<char[], void> writeLinePtr = &Console.WriteLine;
        writeLinePtr(text);
    }
}

/// <summary>
/// Just a simple record struct that wraps an <see cref="int"/>. Don't mind the name.
/// </summary>
/// <param name="await"></param>
record struct async(int await)
{
    public static implicit operator async(int await) => new async(await);

    public static implicit operator int(async await) => await.await;

    public static implicit operator var(async await) => new var(await.await);

    public static implicit operator async(var var) => new async(var);
}

/// <summary>
/// Nothing to see here, just an innocent little ref struct with a perfectly ordinary name.
/// </summary>
/// <param name="number"></param>
ref struct var(int number)
{
    private readonly int _number = number;

    public static implicit operator char(var var) => (char)var._number;

    public readonly void Dispose() { }

    public readonly ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

/// <summary>
/// An async enumerator that wraps around an <see cref="int"/>.
/// </summary>
/// <param name="Number"></param>
record class AsyncEnumerator(int Number)
    : IAsyncEnumerator<int>
{
    public int Current { get; private set; } = -1;

    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(++Current < Number);

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

static class Extensions
{
    /// <summary>
    /// Makes <see cref="int"/> enumerable (<c>foreach</c>).
    /// I decided to make it enumerate up to itself, exclusively, meaning that throwing 5 into a foreach returns [0, 1, 2, 3, 4].
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static IEnumerator<int> GetEnumerator(this int number) =>
        Enumerable.Range(default, number).GetEnumerator();

    /// <summary>
    /// Makes <see cref="int"/> awaitable (<c>await</c>).
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static ValueTaskAwaiter<int> GetAwaiter(this int number) =>
        ValueTask.FromResult(number + 1).GetAwaiter();

    /// <summary>
    /// Makes <see cref="int"/> asynchronously enumerable (<c>await foreach</c>).
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static IAsyncEnumerator<int> GetAsyncEnumerator(this int number) =>
        new AsyncEnumerator(number - 1);
}
