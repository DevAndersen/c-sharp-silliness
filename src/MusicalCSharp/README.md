# MusicalCSharp

C# is named after a musical note, C♯. But have you ever wondered how `.wav` audio files work? No? Neither had I, until the idea suddenly entered my mind.

So, as someone with zero knowledge or experience in the world of music, how hard can it be to make a `.wav` file from scratch, that plays the C♯ musical note?

## The `.wav` format

I had already read somewhere that `wav` is an open standard, and not having to deal with compression also seemed like the easiest option.

Googling led me to find three resources that helped explain the `wav` format:

- http://soundfile.sapp.org/doc/WaveFormat/
- https://www.mmsp.ece.mcgill.ca/Documents/AudioFormats/WAVE/WAVE.html
- https://en.wikipedia.org/wiki/WAV

Wikipedia is kind of obvious as a resource, and you know the other two sites are legit because they look very... how to put it politely... *rustic*. The way you'd expect a scientific professor to design a website. But their information was invaluable, so don't judge a book by its cover (or a website based on its monochromatic background and lack of modern CSS features).

Regardless, the `.wav` format turned out to be surprisingly straightforward. A 44-byte header, and then just raw audio sample data. And 12 of those 44 bytes are magic strings.

Did you know that all `.wav` files start with the four-bytes magic string `RIFF`? *Sick, dude!*

The rest of the header consists of general data, such as the number of samples per second, the number of audio channels, etc.

Audio channels are implemented pretty simply, as the samples simply cycle through the channels one after another. So in a stereo audio file, the samples switch between left- and right speaker. Simple and elegant.

The samples themselves can either be integer values of floating-point values, but not arbitrarily. Certain sample data

Using VLC for testing, I can conclude the following data types appear to be valid/invalid:

| Size | Integer | Floating-point |
|---|---|---|
| 8 | Invalid (`byte`) | Invalid (*Does not exist*) |
| 16 | **Valid** (`ushort`)  | Invalid (`Half`) |
| 32 | **Valid** (`uint`)  | **Valid** (`float`) |
| 64 | Invalid (`ulong`) | **Valid** (`double`) |

For whatever reason, integer samples can be 16 or 32 bit, but floating-point samples can be 32 or 64 bit. Floating-point seems like the obvious type for expressing audio waves, but you seemingly can't use these without having to approx. double the file size (assuming 16 bit audio would otherwise suffice). You can of course just multiply it up and cast to a `ushort`, but still. Weird.

## Makin' waves

Alright, time to make some music!

One *slight* problem: I'm not a musician, I don't play any instruments, and my understanding of audio doesn't go much beyond a general "waves in air that vibrates the inner ear".

But in theory, it should be easy: just make the right wave, and it'll sound however you'd like.

The "beep" sound used to for censoring in movies and on TV is famously a 1 kHz wave (called "[bleep censor](https://en.wikipedia.org/wiki/Bleep_censor)" by Wikipedia). So if you make the audio fluctuate exactly a thousand times per second, you get that tone. And... it works. Quite easily.

Alright, next problem: how do I get from a single tone, to something that at least approximates the sound of a musical note? The C♯ note is supposedly 277.183 Hz, so I just have to hit that tone, and then make it sound note-like.

After a lot of playing around with lots of weird math stuff (and my good old friend [GeoGebra](https://www.geogebra.org/)), making lots of weird sample files, I finally came up with something that sounds at least vaguely like the C♯ musical note. I think.

The general idea:

- Play a double-tone consisting of `277.183 hz` and `277.183 hz * 0.5` (it sounds kinda note-like to me).
- Multiply it by a function that spikes and then drops off over time, which to me is how something like a plucked string behaves.

## Conclusion

It kinda works? I'm no musician, but to my untrained ear, it does sound somewhat like what I can hear online if I try to find the C♯ note played on a piano or similar.

I had hoped for something more like a piano, but what I got sounds more like a simple string instrument. Not optimal, but I'll take it.

So while the end-product of this project didn't quite live up to expectations, I'd say it scored a passing grade. I might not have the makings of a musician, but at least I now know that all good `wav`es start with a sick `RIFF`.
