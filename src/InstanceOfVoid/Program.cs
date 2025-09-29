using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

static class Program
{
    public static void Main()
    {
        Func<object?>[] methods =
        [
            NewVoidObject,
            ActivatorCreateInstance,
            UninitializedVoidObject,
            UnsafeAs,
            RuntimeCreatedType,
            RuntimeCreatedMethod,
            RuntimeCreatedUnsafeMethod,
        ];

        foreach (Func<object?> method in methods)
        {
            string methodName = method.Method.Name;
            object? result = method();
            if (result == null)
            {
                Console.WriteLine($"{methodName}: failure");
            }
            else if (result.GetType() != typeof(void))
            {
                Console.WriteLine($"{methodName}: {result.GetType().FullName}. That's not {typeof(void).Name}.");
            }
            else
            {
                Console.WriteLine($"{methodName}: {result.GetType().FullName}! Behold the void!");
            }
        }
    }

    /// <summary>
    /// Alright, let's try the naive approach, and just use the <c>new</c> keyword.
    /// </summary>
    /// <returns>
    /// Nope, doesn't compile.
    /// There's literally a diagnostics error specifically to prevent you from ever referring to <see cref="Void"/> directly, <see href="https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0673">CS0673</see>.
    /// It's literally invalid C# code to reference this type.
    /// </returns>
    static object? NewVoidObject()
    {
        // System.Void voidObj = new System.Void();
        return null;
    }

    /// <summary>
    /// Let's try
    /// </summary>
    /// <returns>
    /// Nope. Doing so throws a <see cref="NotSupportedException"/> with the message "<c>Cannot dynamically create an instance of System.Void.</c>".
    /// </returns>
    static object? ActivatorCreateInstance()
    {
        try
        {
            return Activator.CreateInstance(typeof(void))!;
        }
        catch (NotSupportedException)
        {
            return null;
        }
    }

    /// <summary>
    /// Maybe <see cref="RuntimeHelpers.GetUninitializedObject"/> can help us out?
    /// </summary>
    /// <returns>
    /// Nope. Just another <see cref="ArgumentException"/> with the message "<c>Type is not supported</c>".
    /// </returns>
    static object? UninitializedVoidObject()
    {
        try
        {
            return RuntimeHelpers.GetUninitializedObject(typeof(void));
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    /// <summary>
    /// Okay, let's use <see cref="Unsafe.As"/> to unsafely cast from <see cref="FakeVoid"/> to <see cref="Void"/>.
    /// And since we can't use <see cref="Void"/> directly, we'll create our own delegate using reflection.
    /// </summary>
    /// <returns>
    /// Nope. <see cref="Void"/> cannot be used as a generic argument.
    /// </returns>
    static object? UnsafeAs()
    {
        try
        {
            MethodInfo method = ((Delegate)Unsafe.As<object, object>).Method;
            MethodInfo newMethod = method.GetGenericMethodDefinition().MakeGenericMethod(typeof(FakeVoid), typeof(void));
            return newMethod.Invoke(null, [default(FakeVoid)]);
        }
        catch (ArgumentException)
        {
            return null;
        }
    }

    /// <summary>
    /// Fine, how about generating a class at runtime, which has a field of type <see cref="Void"/>?
    /// </summary>
    /// <returns>
    /// Nope. Trying to define a field of type <see cref="Void"/> throws an <see cref="ArgumentException"/> with the text <c>Bad field type in defining field</c>.
    /// </returns>
    static object? RuntimeCreatedType()
    {
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("VoidAssembly"), AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("VoidModule");
        TypeBuilder typeBuilder = moduleBuilder.DefineType("TypeWithVoidField", TypeAttributes.Public);

        FieldBuilder fieldBuilder;
        try
        {
            fieldBuilder = typeBuilder.DefineField("MyVoidField", typeof(void), FieldAttributes.Public);
        }
        catch (ArgumentException)
        {
            return null;
        }

        Type myType = typeBuilder.CreateType();

        return Activator.CreateInstance(myType);
    }

    /// <summary>
    /// Okay, what if we just created a method at runtime, which essentially just returns <c>default(System.Void)</c>?
    /// </summary>
    /// <returns>
    /// Nope. If it ever tries to create a local of type <c>System.Void</c>, it throws an <see cref="TargetInvocationException"/>,
    /// which wraps an <see cref="InvalidProgramException"/> inner exception.
    /// Gotta be honest here, this is just my best attempt at replicating the IL that https://lab.razor.fyi spat out for a default-returning method.
    /// But it works fine if I use another type, so once again, the runtime just refuses to create void objects.
    /// </returns>
    static object? RuntimeCreatedMethod()
    {
        Type voidType = typeof(void);

        DynamicMethod method = new DynamicMethod("CreateNewVoid", voidType, []);
        ILGenerator ilGenerator = method.GetILGenerator();

        LocalBuilder voidLocal = ilGenerator.DeclareLocal(voidType);
        LocalBuilder voidLocal2 = ilGenerator.DeclareLocal(voidType);

        ilGenerator.Emit(OpCodes.Nop);
        ilGenerator.Emit(OpCodes.Ldloca_S, voidLocal);
        ilGenerator.Emit(OpCodes.Initobj, voidType);
        ilGenerator.Emit(OpCodes.Ldloc_0);
        ilGenerator.Emit(OpCodes.Stloc_1);

        Label label = ilGenerator.DefineLabel();
        ilGenerator.Emit(OpCodes.Br_S, label);
        ilGenerator.MarkLabel(label);

        ilGenerator.Emit(OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Ret);

        try
        {
            return method.Invoke(null, []);
        }
        catch (TargetInvocationException e) when (e.InnerException is InvalidProgramException)
        {
            return null;
        }
    }

    /// <summary>
    /// Okay, what about pointers?
    /// We can't define a <see cref="Void*"/>, but can we use IL to get out of that snag?
    /// </summary>
    /// <returns>
    /// Nope.
    /// Same as <see cref="RuntimeCreatedMethod"/>, this throws an <see cref="TargetInvocationException"/>,
    /// which wraps an <see cref="InvalidProgramException"/> inner exception. So it doesn't matter if it's a struct of a pointer, it's still illegal.
    /// </returns>
    static object? RuntimeCreatedUnsafeMethod()
    {
        Type fakeVoidType = typeof(FakeVoid);
        Type fakeVoidPointerType = fakeVoidType.MakePointerType();

        Type voidType = typeof(void);
        Type voidPointerType = voidType.MakePointerType();

        DynamicMethod method = new DynamicMethod("CastNewVoid", typeof(object), []);
        ILGenerator ilGenerator = method.GetILGenerator();

        LocalBuilder fakeVoidLocal = ilGenerator.DeclareLocal(fakeVoidType);
        LocalBuilder voidLocal = ilGenerator.DeclareLocal(fakeVoidPointerType);
        LocalBuilder voidPointerLocal = ilGenerator.DeclareLocal(voidPointerType);
        LocalBuilder objectLocal = ilGenerator.DeclareLocal(typeof(object));

        ilGenerator.Emit(OpCodes.Nop);
        ilGenerator.Emit(OpCodes.Ldloca_S, fakeVoidLocal);
        ilGenerator.Emit(OpCodes.Initobj, fakeVoidType);
        ilGenerator.Emit(OpCodes.Ldloca_S, fakeVoidLocal);
        ilGenerator.Emit(OpCodes.Conv_U);
        ilGenerator.Emit(OpCodes.Stloc_1);
        ilGenerator.Emit(OpCodes.Ldloc_1);
        ilGenerator.Emit(OpCodes.Stloc_2);
        ilGenerator.Emit(OpCodes.Ldloc_2);
        ilGenerator.Emit(OpCodes.Ldobj, voidType);
        ilGenerator.Emit(OpCodes.Box, voidType);
        ilGenerator.Emit(OpCodes.Stloc_3);

        Label label = ilGenerator.DefineLabel();
        ilGenerator.Emit(OpCodes.Br_S, label);
        ilGenerator.MarkLabel(label);
        ilGenerator.Emit(OpCodes.Ldloc_3);
        ilGenerator.Emit(OpCodes.Ret);

        try
        {
            return method.Invoke(null, []);
        }
        catch (TargetInvocationException e) when (e.InnerException is InvalidProgramException)
        {
            return null;
        }
    }

    /// <summary>
    /// Functionally identical to <see cref="Void"/>.
    /// </summary>
    struct FakeVoid
    {
    }
}
