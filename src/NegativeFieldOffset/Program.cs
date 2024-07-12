using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

// A span containing three bytes.
// The structs will be marshalled on top of the "1", and use field offsets to access the other bytes in the span.
Span<byte> bytes = [255, 1, 2];

// An offset of 0 will point to the "1" byte.
// An offset of 1 will point to the "2" byte.
// An offset of -1 will point to the "255" byte.
foreach (int offset in new int[] { 0, 1, -1 })
{
    try
    {
        byte b = TestWithOffset(ModuleBuilder, offset, bytes);
        Console.WriteLine($"With an offset of {offset}, the value of MyByte is {b}");
    }
    catch (Exception e)
    {
        Console.WriteLine($"Attempt to create type with offset {offset} threw an {e.GetType().Name}: {e.Message}");
    }
}

Console.ReadLine();

static partial class Program
{
    public static ModuleBuilder ModuleBuilder { get; } = CreateModuleBuilder();

    static unsafe byte TestWithOffset(ModuleBuilder moduleBuilder, int offset, Span<byte> bytes)
    {
        // Create a new type with the specified field offset.
        Type type = CreateTypeWithOffset(moduleBuilder, offset);

        // Get a pointer to the "1" byte of the span.
        fixed (byte* ptr = &bytes[1..][0])
        {
            // Create an instance of our struct on top of the "1" byte of the span.
            dynamic d = Marshal.PtrToStructure(new nint(ptr), type)!;
            return d.MyByte;
        }
    }

    /// <summary>
    /// Creates an assembly and module at runtime.
    /// </summary>
    /// <returns></returns>
    static ModuleBuilder CreateModuleBuilder()
    {
        string assemblyName = "DynamicallyGeneratedAssembly";

        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName(assemblyName),
            AssemblyBuilderAccess.Run);

        return assemblyBuilder.DefineDynamicModule(assemblyName);
    }

    /// <summary>
    /// Creates a new type at runtime, with byte field "<c>MyByte</c>" which has a field offset of <paramref name="offset"/>.
    /// </summary>
    /// <param name="moduleBuilder"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    static Type CreateTypeWithOffset(ModuleBuilder moduleBuilder, int offset)
    {
        // Add a GUID to the type name, to ensure it is unique.
        string typeName = $"DynamicallyGeneratedType_{Guid.NewGuid()}";
        string fieldName = "MyByte";

        TypeBuilder typeBuilder = moduleBuilder.DefineType(
            typeName,
            TypeAttributes.Public,
            typeof(ValueType));

        // Decorates the type with StructLayoutAttribute, setting its layout to explicit.
        typeBuilder.SetCustomAttribute(GetAttributeConstructor<StructLayoutAttribute, LayoutKind>(LayoutKind.Explicit));

        FieldBuilder fieldBuilder = typeBuilder.DefineField(
            fieldName,
            typeof(byte),
            FieldAttributes.Public);

        // Decorates the field with FieldOffsetAttribute, setting its field offset to the specified offset.
        fieldBuilder.SetCustomAttribute(GetAttributeConstructor<FieldOffsetAttribute, int>(offset));

        return typeBuilder.CreateType();

    }

    /// <summary>
    /// Returns a builder for <typeparamref name="TAttribute"/>, using the constructor with a single parameter of type <typeparamref name="TConstructorArg1"/>
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <typeparam name="TConstructorArg1"></typeparam>
    /// <param name="arg1"></param>
    /// <returns></returns>
    static CustomAttributeBuilder GetAttributeConstructor<TAttribute, TConstructorArg1>(TConstructorArg1 arg1)
    {
        return new CustomAttributeBuilder(
            typeof(TAttribute).GetConstructor([typeof(TConstructorArg1)])!,
            [arg1]);
    }
}
