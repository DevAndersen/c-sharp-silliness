### InstanceOfVoid

If you've ever used reflection, you might have noticed that the return type of a void method is not simply `null`. Instead, it is `System.Void`.

Sure, whatever, just a placeholder so that `Type.ReturnType` never returns `null`, right?

`System.Void` is an empty struct. No methods, no properties, no fields, no attributes, no nothing.

So, just for fun, can we create an object of type `System.Void`, so that `voidObject.GetType() == typeof(void)` is true? I mean, it's just an empty struct, how hard can it be?

## Conclusion

TL;DR: No.

---

First of all, `System.Void` doesn't show up in IntelliSense. It's hidden from being recommended. Off to a good start.

Next, let's just use the `new` keyword.	`new System.Void();`, how hard can it be? Well, you see, `System.Void` is illegal C# syntax. You cannot even reference this type directly, as doing so will result in a [CS0673](https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0673) diagnostics error. Yes, there's literally a unique diagnostics error specifically for referencing this particular type. I'm starting to suspect this is more than just a placeholder type used for reflection.

Alright, `CS0673` tells us to use `typeof(void)`, so how about using `Activator.CreateInstance`? You give it a type, and it attempts to create a new instance of that type. Heh, no. `NotSupportedException`.

Hmm, okay. Anyone who has skimmed through the .NET source code has probably come across references to `RuntimeHelpers`. Maybe that will help me on my quest? Hah, no, fat chance. Using it with `typeof(void)` throws an `ArgumentException`. Another dead end.

Fine. What if we create our own memberless struct, and use reflection to create a delegate of `Unsafe.As` which converts from `FakeVoid` to `System.Void`? Nope, `System.Void` cannot be used as a generic argument. So anything that relies on generics is going to fail if one of the arguments is `System.Void`, even if that method is internal to the .NET runtime itself. Well, shit.

Okay, what if we create a type at runtime, and define a field on that type which has the type `System.Void`? Nope, `TypeBuilder.DefineField` throws an `ArgumentException` if the field type is `System.Void`.

Alright, now the gloves are coming off! How about using `DynamicMethod` and `ILGenerator` to create a method at runtime which returns `default(System.Void)`? Nope, `InvalidProgramException`.

Time to pull out the big guns. What if we use `DynamicMethod` to generate a method at runtime which replicates unsafe code that casts a `FakeVoid*` to a `Void*`? Nope, same error as the above. Using pointer didn't make any difference whatsoever.

---

Alas, I have reached my wits' end. I have no more ideas or tricks up my sleeves. The compiler and runtime actively refuses any and all attempt at creating an instance of `System.Void`.

Sadly, the clever people of the .NET team seem to have done their homework on this one. Surely, this is because `System.Void` isn't just a placeholder, but actually the type that the runtime uses internally to represent `void`? Right?

`<see cref="Void"/>` in XML comments appear as if they're references to `void` in Visual Studio's hover-over tooltip. Is this a hint of what is actually happening, or am I just being paranoid?

Regardless, I admit defeat. I have, quite literally, failed to create nothing.
