# Modubil

Modubil is a modular vehicle library for Unity 3D. The library is focused on
scripts only, i.e. no 3D models. It is built to make it easily extensible, with
the ability to change any individual part with either one of multiple
implementations, or your own implementation. For this purpose, every module is
implemented using C# interfaces, making everything loosely coupled, and
(hopefully) adhere to the [SOLID](https://en.wikipedia.org/wiki/SOLID)
principles.

This library is meant for people who wish to have full control over the
implementation details of the handling of a vehicle in Unity 3D. It is not meant
to be an easy way to get decent vehicle physics, there are other and much better
libraries for this (look in the
[Unity Asset Store](https://assetstore.unity.com/?q=vehicle%20physics&orderBy=1)
for inspiration).

The source code in this repository was originally based on
[Adrenak/tork](https://github.com/adrenak/Tork), but has since diverged enough
that it deserves it's own branding.
