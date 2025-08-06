# StringOverwriting

Can we change the value of [`string.Empty`](https://learn.microsoft.com/en-us/dotnet/api/system.string.empty), so it is no longer empty?

**Conclusion:** Yes. It is possible to access and modify the internal length counter of a string, as well as change the content of the string, thereby making the string longer.

This will overwrite any memory that is located right after the string. Doing so can result in a `fatal errors`, if the new string content is sufficiently long.
