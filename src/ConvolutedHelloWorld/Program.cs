using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

char[] text = [.. Hell(), O1(), Comma(), Space(), W(), O2(), R(), L(), D(), ExclamationMark()];
WriteLine(text);

char[] Hell()
{
    return ['_', '_', '_', '_'];
}

char O1()
{
    return '_';
}

char Comma()
{
    return '_';
}

char Space()
{
    return '_';
}

char W()
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

char O2()
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

char R()
{
    // First, let's create a string. Nothing out of the ordinary here.
    // We'll even make it a compile time constant, so you know it's super-duper not gonna change.
    const string Text = "Please don't mutate me";

    // Next, let's create a ReadOnlyMemory over the text. Again, perfectly normal.
    ReadOnlyMemory<char> readOnlyMemory = Text.AsMemory();

    // Now, let's turn that ReadOnlyMemory<char> into a Memory<char> and get a Span<char> over it.
    // This gives us read-write access to the string's buffer.
    // Look ma! No unsafe!
    Span<char> span = MemoryMarshal.AsMemory(readOnlyMemory).Span;

    // And finally, we'll change the first character in the span to the lower-case letter 'r'.
    // Mutating string is a big no-no! This is technically data corruption.
    // Luckily, we don't use this string elsewhere, so we're technically good.
    span[0] = 'r';

    // And the cherry on top: because this string is known at compile time, it gets interned.
    // This means that all instances of this string point to the same location in memory.
    // We just mutated the underlying buffer, meaning that all other instances of this string were also mutated. Spooky action at a distance!
    // So while it looks like we are returning a 'P', we are actually returning an 'r'. Highly illegal stuff, don't tell your parents.
    return "Please don't mutate me"[0];
}

char L()
{
    return '_';
}

char D()
{
    return '_';
}

char ExclamationMark()
{
    return '_';
}

unsafe void WriteLine(char[] text)
{
    // And now, all we need to do is to write our string to the console.
    // We'll of course use Console.WriteLine for this, but calling it directly seems rather anticlimactic, doesn't it?
    // Let's create a function pointer and invoke it that way. After all, needless complexity is the name of the game here!
    delegate*<char[], void> writeLinePtr = &Console.WriteLine;
    writeLinePtr(text);
}
