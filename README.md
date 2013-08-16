# VTK in Unity via ActiViz

Brings scientific visualization into the Unity game engine. Goal of this project is to interactively control visualization parameters in a VR environment (powered by [MiddleVR][4]).

## Prerequisites

- Windows only
- Install [ActiViz 5.8 x32][1]
- Add the *ActiViz Installation Dir/bin* to the `PATH` environment variable

## Usage

All C#-Dlls (Kitware.\*) from ActiViz' *bin*-folder have to be in a *Plugins*-folder in Unity. Have a look at the example scene. For more infos regarding VTK in C# check out the [examples][2].

## Run as a player

- In *Player Settings* set *API Compatibility Level* to *.NET 2.0*
- Either all Dlls are in the same directory as the player exe or simply have ActiViz installed and its *bin*-dir appended to the `PATH`

## Features

You can build arbitrary VTK filter pipelines but at the end the `VtkToUnity`-class accepts a `vtkAlgorithmOutput` with `vtkPolyData`. It then runs a `vtkTriangleFilter` on it. Points and Lines are rendered via [Vectrosity][3] which is not included. All primitves (points, lines, triangles) can be colored by a solid color, a point data scalar (1d) field or by a cell data scalar (1d) field. Some lookup table presets can be used.

## General ActiViz tips

- When instantiating a VTK class use its `New()`-method instead of C#s `new`
- C#s automatic garbage collection should work as expected. Calling `Dispose()` on VTK objects should not be necessary.
- Vtk data files should be placed in *Assets/StreamingAssets* so that they get included into the standalone build (the *Resources*-folder does not work because it is used only for Unity-known file formats)

[1]:	http://www.kitware.com/KWLD/download/download.php?cid=anonymous&fid=67&pid=17
[2]:	http://www.vtk.org/Wiki/VTK/Examples/CSharp
[3]:	http://starscenesoftware.com/vectrosity.html
[4]:  http://imin-vr.com/
