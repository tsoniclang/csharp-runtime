# `@tsonic/csharp-runtime`

Base C# runtime substrate for Tsonic-generated C#. It owns closed carriers and
helpers required independently of the JavaScript surface and Node capability,
including undefined/union values, finite broad values, generators, resource
management, and typed locations.

Canonical product documentation:

- [C# interop and safety](https://github.com/tsoniclang/tsonic/blob/main/docs/manual/targets/csharp/interop-and-safety.md)
- [C# type mapping](https://github.com/tsoniclang/tsonic/blob/main/docs/reference/targets/csharp/type-mapping.md)
- [Provider and runtime ownership](https://github.com/tsoniclang/tsonic/blob/main/docs/architecture/provider-and-runtime-ownership.md)

The npm package contains C# source, its project, and build settings. The public
`@tsonic/csharp-runtime/runtime.csproj` export identifies the native project.
The C# target references it for the application's selected framework. Build
outputs stay in the application's `.tsonic/cache`, not the installed package.
