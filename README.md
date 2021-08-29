# unity-golang-native-plugin

This small project shows off how you can use the c-shared buildmode in golang to build a library that can be consumed by the Unity game engine as a native plugin. There are some fun caveats:

- It's not very fast
- Doing this is rather painful
- This technique has not been tested en masse, this is only a proof of concept to prove it is possible.

## Why might you want to think about this?

If you were writing a golang service for a game to consume, there can be value in shared code that the Unity client can run to avoid reimplementation.