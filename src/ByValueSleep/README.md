# ByValueSleep

Value types are passed by value (shocking, I know), meaning that the time it takes to pass them to a method depends on the size of the structure.

If we were to create a really big struct, could we use this to approximate a 1-second delay simply by passing our struct around?

**Conclusion:** Yes, we can approximate a 1-second sleep simply by measuring the time it takes to pass a really bug `struct` around.

With a 100,000 byte struct, we can narrow down the approximate number of times we need to pass it around before reaching a roughly 1-second delay.

100,000 bytes is also well short of the usual 1 MB size of the execution stack, meaning we are completely safe from `StackOverflowException`.

This all of course depends on the particular CPU being used, what it is currently doing besides this project, and so on. But, technically, this works.

Note: This is one of the reasons why it is recommended that `struct` types should be small (16 bytes or less seems to be a common recommendation).
