# BinarySerializer.Ray1
BinarySerializer.Ray1 is an extension library to [BinarySerializer](https://github.com/RayCarrot/BinarySerializer), created by [RayCarrot](https://github.com/RayCarrot) and [Byvar](https://github.com/byvar). It supports serializing formats from all versions of Rayman 1, including the SNES prototype and the Atari Jaguar versions.

# Usage
Using this library you can read/write to any supported data format.

```cs
// Create a context. The base path passed in is the absolutely base path for all the files which are to be serialized.
using var context = new Context(basePath);

// Add settings
context.AddSettings(new Ray1Settings(Ray1EngineVersion.R1_PC, World.Jungle, 1, pcVersion: Ray1PCVersion.PC_1_21));

// Add the files to serialize using their relative paths (relative to the base path)
context.AddFile(new LinearSerializedFile(context, relativeFilePath));

// Use FileFactory for reading/writing helpers
PC_LevFile levelData = FileFactory.Read<PC_LevFile>(relativeFilePath, context);

// Modify the data as a normal C# object
levelData.ObjData.Objects.First(x => x.Type == ObjType.TYPE_RAY_POS).XPosition = 500;

// Use FileFactory to write the file, saving the modifications
FileFactory.Write<PC_LevFile>(relativeFilePath, context);
```
