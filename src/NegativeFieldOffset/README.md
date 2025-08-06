# NegativeFieldOffset

[`FieldOffsetAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.fieldoffsetattribute) does not support negative numbers, and will result in a `CS0591: Invalid value for argument to attribute`, preventing compilation.

But can we work around this by defining a type with a negative `FieldOffsetAttribute` at runtime?

## Conclusion

No, if a negative number is supplied, an [`ArgumentException`](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception) will be thrown with the message "*Invalid custom attribute provided.*"
