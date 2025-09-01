# DateTimePastMaxValue

[`DateTime.MaxValue`](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.maxvalue) represents the date 23:59:59.9999999 UTC, December 31, 9999, the last representable instance in time before the year 10.000 begins.

That's a conveniently elegant value, and clearly not representative of the actual maximum value of the `DateTime` data structure.

Can we force it beyond its intended maximum value, and if so, what happens?

## Conclusion

Yes, however there is very limited practical use for this.

If you manipulate a `DateTime` struct by setting all its bytes to `byte.MaxValue`, you can get a `DateTime` that represents a date beyond `DateTime.MaxValue`.

Specifically, the true maximum value is roughly 13.24.02, November 8, 14614. That's additional 4614 years of technically representable moments in time.

To put that into perspective, going back 4614 years from today (in 2025) lands us in 2589 BC. That's roughly around the time that the [Great Pyramid of Giza](https://en.wikipedia.org/wiki/Great_Pyramid_of_Giza) was being constructed. That's a lot of time which is technically possible to represent with a `DateTime`, but apparently, calculating things that go 12589 years into the future was deemed "unnecessary" by the developers of .NET.

You can actually perform comparisons on such values, and it will correctly state that the value is greater than `DateTime.MaxValue`. Which is a bit funny.

However, invoking method calls on such an object which return a new `DateTime`, e.g. `AddYears`, is likely to throw an `ArgumentOutOfRangeException` if, as is likely, the resulting `DateTime` represents a date outside of [`DateTime.MinValue` - `DateTime.MaxValue`].

So while this technically allows you to represent an additional 4614 years worth of dates and times, performing any calculations on such an object carries the risk of getting an `ArgumentOutOfRangeException` thrown your way, as punishment for your hubris.

Doable? Yes.

Useful? As expected, a resounding "no".
