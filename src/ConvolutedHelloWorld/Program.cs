// You know it's gonna be good when the code begins by suppressing several warnings.
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles

using Microsoft.Win32.SafeHandles;
using System.Buffers.Binary;
using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

PrintLine([.. Hell(), O1(), Comma(), Space(), W(), O2(), R(), .. Ld(), ExclamationMark()]);

file static partial class Program
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
    /// Topic: Text parsing, text normalization.
    /// </summary>
    /// <returns></returns>
    private static char O1()
    {
        // Dynamic is a bizarre aspect of C#.
        // It essentially tells the compiler to just trust whatever you've written,
        // and as long as it's technically valid C# code, it'll just go with it.

        // We can use/abuse this to turn any valid identifier into a compile-time constant string using this cheeky little nameof-trick.
        // And, because underscore is a valid identifier, this works even though the type "dynamic" doesn't have a member named "_".

        // Sidenote: Unless you're a professional madman (like I am), you should never use dynamic.
        // It causes far more problems than it solves, makes debugging and troubleshooting needlessly painful, and there are always better alternatives
        dynamic dyn;
        string underscore = nameof(dyn._);

        // Next, we bitshift '_' one to the left, and we get '¾', the character for three quarters.
        char threeQuarters = (char)(underscore[0] << 1);

        // And, because the BCL has logic for just about everything imaginable,
        // we can actually parse that character as a double with the value of 0.75.
        double zeroPointSevenFive = CharUnicodeInfo.GetNumericValue(threeQuarters);

        // Next up, let's convert that 0.75 to 75, and use linear interpolation (lerp)
        // to get the value that is 75% of the way between 75 and 255.
        // You ask why we're doing this? Because it gives the answer we're looking for. Stop asking questions!
        char upperCaseCharWithDiacritics = (char)double.Lerp(zeroPointSevenFive * 100, byte.MaxValue, zeroPointSevenFive);

        // Sidenote: The venerable Math and MathF classes are technically soft-deprecated.
        // They're not getting deleted, but it is recommended that you use the static methods on the numeric types themselves.
        // The .NET team just haven't really communicated this.
        // This is in order to improve clarity, as you make it explicit which data type you're working with.
        // Math and MathF also aren't getting expanded with new methods, unlike the static methods on the numeric types.
        // For example, Math.Lerp does not exist.

        // Regardless, our little lerp trick gives us the character 'Ò'.
        // Hmm, we're getting closer to the 'o' we're looking for, but we're not quite there yet.
        // We need to adjust the casing, and then get rid of the diacritics.
        // We'll do the easy one first, and lower-case it.
        string lowerCaseCharWithDiacritics = char.ToLower(upperCaseCharWithDiacritics).ToString();

        // Now we can use character normalization to essentially split the character into its normalized latin variant and its diacritic.
        // We then simply take the first character in that string, which is the latin letter 'o', and we're done!
        return lowerCaseCharWithDiacritics.Normalize(NormalizationForm.FormD)[0];
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

        // The following abomination was inspired by a tweet by Jared Parsons (C# compiler lead), in which he stated that the following can be valid C# code.
        //
        // public class var {
        //    async async async(async async) => await async;
        // }
        //
        // You can see him explain it here: https://www.youtube.com/watch?v=jaPk6Nt33KM&t=228
        // So, in short, blame Parsons for teaching us how to abuse the C# language. He started it!

        using scoped var var = Task.Run<async>(async () =>
        {
            await using var var = await Task.Run(static async () =>
            {
                // This is, obviously, the number 5.
                nint nint = default(var) + await sizeof(int);

                // This is neither asynchronous nor a variable of a compiler-inferred type, despite using the words "async" and "var".
                // But the "default" (which is a non-contextual keyword) does what you'd expect (assuming you expect "async" to be the name of a type).
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
    /// Topic: Expressions, regular expressions, Unicode character literals, URL encoding.
    /// </summary>
    /// <returns></returns>
    private static char Space()
    {
        // Expressions are pretty nifty. They essentially describe logic in an abstract way,
        // which you can then compile into an invokable delegate, or you can parse it and use it
        // elsewhere. For example, Entity Framework tarnslates expressions to SQL.
        // Here is a simple expression, which just adds w to the input number.
        Expression<Func<int, int>> exp = (number) => number + 2;

        // Now we ToString() the expression to get the the C# syntax of the expression as a string.
        // I initially just made the expression body "2 + 2", but the C# compiler then realizes it can be lowered to "4",
        // so we have to use at least one non-literal value to avoid that.
        string expressionString = exp.ToString();

        // So, that's an expression, but what about regular expressions (aka. RegEx)?
        // This entire project is meant to be needlessly convoluted and overly complicated,
        // and if there's one thing often cited as being complicated, it's RegEx.

        // Below you see the string "\w+\s(?<_>\+)\s\d", written using 16-bit Unicode character literal.
        // That is, a RegEx pattern which matches the following sequence:
        // - One or more word-characters
        // - A whitespace character
        // - The character '+' (put into a group named '_')
        // - A whitespace character
        // - A single digit
        // And it just so happens that this would match "number + 2", and put the plus character into group 1. How convenient.

        // Note: Because this string is used as a RegEx pattern by Regex.Match, Visual Studio will apply RegEx syntax coloring to the Unicode literals,
        // exactly like it would if the string was written normally. That's pretty groovy (not the Apache language).
        const string pattern = "\u005C\u0077\u002B\u005C\u0073\u0028\u003F\u003C\u005F\u003E\u005C\u002B\u0029\u005C\u0073\u005C\u0064";

        // Good heavens, would you look at the time? It's RegEx-o'-clock!
        Match match = Regex.Match(expressionString, pattern);

        // We now have the character '+' from our expression in the '_' match group, which we can simply extract.
        string plus = match.Groups["_"].Value;

        // Now to turn a plus into a space. Luckily for us, that's exactly how URLs encode strings.
        // So we can just use WebUtility to "decode" the plus into a space, and grab the first (and only) character from that string.
        return WebUtility.UrlDecode(plus)[0];
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
        // Fun fact: the null-forgiving operator is informally known as the "damn-it" or "dammit" operator, since it's essentially telling the compiler "just do it, dammit".
        // Microsoft might deny this, but we all know it's the truth.
        return (char)type.GetMethod(MethodName)!.Invoke(null, [])!;

        // So, after having written all this, I found out that there's a type called "DynamicMethod",
        // which seemingly just creates a type at runtime without needing to create the surrounding assembly-module-type structure for it.
        // So, this method could've been made quite a bit shorter...
        // Oh well, not gonna bother changing it now.
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
        // You'd normally only end up with NaN if you divide zero by zero, but we got there via binary reinterpretation instead.
        // Note: This only works with floating point numeric types. If you divide a non-floating point number by zero, you'll instead get a nice and pretty DivideByZeroException.
        string nan = half.ToString();

        // And finally, let's index into the "NaN" string, and shuffle the bytes around until we get something useful.
        // We'll also index into the characters back to front (that's what the '^' in the indexers does), simply because we can.
        // This is not to be confused with binary XOR, which also uses '^' as its operator.

        // Since binary NOT ('~') simply flips all the bits, using it an uneven number of times is the same as using it once.
        // Similarly, using it an even number of times does nothing, as it just flips the bits back to their initial state.

        // And, what do you know, we bit manipulated the characters of "NaN" into a lower-case 'o'.
        // Turns out you really can do anything if you set your mind to it, and don't mind writing a bit of dodgy-looking code to make it work.
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
    /// Topic: Integer literals, UTF-8 string literals, field overlap.
    /// </summary>
    /// <returns></returns>
    private static char[] Ld()
    {
        // C# lets you express integer values in more than just decimal (base 10).
        // You can use hexadecimal (base 16), which will be familiar to anyone who's familiar with CSS or similar (albeit with the "0x" prefix instead of "#").
        int i = 0x6c00;

        // You can also use binary (base 2), by prefixing the number with "0b". This can seem rather excessive,
        // but I find it useful when working with bit fields (enums where each individual bit of the number represents a boolean flags).
        // Come to think of it, I should probably have demonstrated bit fields rather than just mentioning them. Oh well.
        // Sidenote: You can use underscores to improve readability of numeric literal, including floating point literals. The compiler just ignores them.
        int j = 0b_0110_0011_1111_1111_1111_1111_1110_1010;

        // Normally, C# strings and chars are UTF-16, however with the "u8" suffix you can also define UTF-8 literals.
        // These do have to be assigned to a ReadOnlySpan<byte>.
        // So for strings known at compile time, you no longer need good ol' Encoding.UTF8 if all you want is the UTF-8 version.
        ReadOnlySpan<byte> text = "This is a UTF8 string "u8;

        // Now let's add our numbers together, as well as the length of out UTF-8 string.
        int sum = i + j + text.Length;

        // This struct has three fields, a 32-bit int and two 16-bit ushorts.
        // However, using the FieldOffset attribute, the int overlaps in memory with the ushorts.
        // This means the ushorts essentially point to the first and last 16-bits of the 32-bit integer, respectively.
        // Easy reinterpretation of data, with no additional overhead or calculations.
        IntWithAddressableShorts result = new IntWithAddressableShorts
        {
            I32 = sum
        };

        Span<byte> buffer = stackalloc byte[2];

        // Now we can just grab our two 16-bit ushorts out of the struct, however their bytes are in the wrong order.
        // This is called endianness, and while you could probably debate the merits of both until the end of time,
        // I'm just here for my two ushorts.
        BinaryPrimitives.WriteUInt16BigEndian(buffer, result.U16a);
        char l = (char)BinaryPrimitives.ReadUInt16LittleEndian(buffer);

        BinaryPrimitives.WriteUInt16BigEndian(buffer, result.U16b);
        char d = (char)BinaryPrimitives.ReadUInt16LittleEndian(buffer);

        // And voila, we're all done.
        // Come to think of it, I wonder if there's an easier way of writing a method in C# that returns the chars 'l' and 'd'...
        // Nah, doubt it. This is surely the most straightforward way. Surely. Surely...
        return [l, d];
    }

    /// <summary>
    /// Topic: Pointers, string memory layout.
    /// </summary>
    /// <returns></returns>
    private unsafe static char ExclamationMark()
    {
        // .NET comes with a lot of types (3609 by my counting, as of .NET 9.0.9).
        // Some have short names, like the beloved "GC" (short for Garbage Collector) at just two characters.
        // On the other end of the spectrum, we have the monstrous "DynamicPartitionEnumeratorForIndexRange_Abstract`2"
        // (the `2 indicates that it has two generic type parameters).

        // Somewhere in the middle, we have "SafeHandleZeroOrMinusOneIsInvalid", clocking in at a very convenient 33 characters.
        // Can you guess what Unicode 33 is? Did the name of this method give it away?

        // Alright, let's create a string which contains the name of SafeHandleZeroOrMinusOneIsInvalid,
        // and then get an unsafe pointer to the first character of that string.
        fixed (char* ptr = nameof(SafeHandleZeroOrMinusOneIsInvalid))
        {
            // Interesting thing about strings, the length of a string is located right before the character buffer, stored as a little endian 32-bit integer.
            // I don't believe this is part of the .NET spec, so this might not be correct for all .NET runtime implementations.
            // But, frankly, I don't really care. If it works, it works.

            // Regardless, we simply need to step four bytes backwards, and then we can access the string's length.
            // And since we're already working with a char pointer, we don't even need to cast it to anything, we can simply dereference it.
            // And there you have it, an exclamation mark "extracted" from the name of a safe handle class
            // (which also happens to be used to represent pointers and whatnot).
            return *(ptr - sizeof(int) / sizeof(char));
        }
    }

    /// <summary>
    /// Topic: Function pointers.
    /// </summary>
    /// <param name="text"></param>
    private static unsafe void PrintLine(char[] text)
    {
        // And now, all we need to do is to write our string to the console.
        // We'll of course use Console.WriteLine for this, but calling it directly seems rather anticlimactic, doesn't it?
        // Let's create a function pointer and invoke it that way. After all, needless complexity is the name of the game here!
        delegate* managed<char[], void> writeLinePtr = &Console.WriteLine;
        writeLinePtr(text);
    }
}

/// <summary>
/// A struct that wraps an int, with two ushorts that each overlap one half of the int in memory.
/// Did I name it that because "addressable shorts" sounds funny? Maybe...
/// </summary>
[StructLayout(LayoutKind.Explicit)]
struct IntWithAddressableShorts
{
    [field: FieldOffset(0)]
    public int I32 { get; set; }

    [field: FieldOffset(0)]
    public ushort U16a { get; set; }

    [field: FieldOffset(sizeof(char))]
    public ushort U16b { get; set; }
}

/// <summary>
/// Just a simple record struct that wraps an <see cref="int"/>.
/// Don't mind the name. Nothing to see here, move along!
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
