# VTK in Unity via ActiViz

## Prerequisites

- Windows only
- Install [ActiViz 5.8 x32][1]
- Add the *ActiViz Installation Dir/bin* to the `PATH` environment variable

## Usage

All C#-Dlls (Kitware.*) from ActiViz' *bin*-folder have to be in a *Plugins*-folder in Unity. Have a look at the example scene. For more infos regarding VTK in C# check out the [examples][2].

## Run as a player

- In *Player Settings* set *API Compatibility Level* to *.NET 2.0*
- Either all Dlls are in the same directory as the player exe or simply have ActiViz installed and its *bin*-dir appended to the `PATH`


[1]:	http://www.kitware.com/KWLD/download/download.php?cid=anonymous&fid=67&pid=17
[2]:	http://www.vtk.org/Wiki/VTK/Examples/CSharp
