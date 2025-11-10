# SolutionVersionHandler

!!! Work in progresss !!!

SolutionVersionHandler is a small WPF desktop tool for inspecting and editing project version information inside a Visual Studio solution. It provides a grid of discovered projects and per-project editors for package, assembly and file versions, and helpers to propagate version changes across projects.

This repository targets .NET 8 and is intended to be run on Windows (WPF).

## Features

- Analyse a `.sln` file and list contained projects
- Inspect and edit project `PackageVersion`, `AssemblyVersion`, and `FileVersion`
- Per-project version editor with numeric and textual support
- Propagate version values across projects or across version types
- Simple UI for loading a solution and saving modified project files

## Prerequisites

- Windows
- .NET 8 SDK
- Visual Studio 2022/2023 (recommended) or `dotnet` CLI for building

## Building and running

Using Visual Studio

1. Open `SolutionVersionHandler.sln` in Visual Studio.
2. Build the solution (Build -> Rebuild Solution).
3. Run the project (F5 or Start Debugging).

Using dotnet CLI (Windows only)

```bash
dotnet build SolutionVersionHandler.sln
dotnet run --project SolutionVersionHandler\SolutionVersionHandler.csproj
```

Note: the project is a WPF application and requires Windows to run the UI.

## Usage

- Click "Load Solution" and choose a `.sln` file to analyse.
- The application lists discovered projects in a grid.
- Use the inline version editors to change version components or enter an unparseable textual version.
- Use the provided buttons to propagate the chosen version to other projects or version fields.
- Click "Save" to persist changes back to project files.

## Contributing

Contributions are welcome. Please open issues for bugs or feature requests and submit pull requests for changes.

When contributing:
- Target the existing project style and architecture
- Keep changes small and focused
- Add unit tests where appropriate (this project currently contains primarily UI code)

## License

This project is licensed under the MIT License - see the full license text below.

---

MIT License

Copyright (c) <YEAR> <OWNER>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
