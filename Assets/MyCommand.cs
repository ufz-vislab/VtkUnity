using UnityEngine;
using System.Collections;

// Usage
//MyCommand myCommand = new MyCommand();
//SphereSource.AddObserver((uint)Kitware.VTK.vtkCommand.EventIds.ModifiedEvent, myCommand, 0.69f);
//SphereSource.Modified();

public class MyCommand : Kitware.VTK.vtkCommand
{
    public override void Execute(
        [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler,
        MarshalTypeRef = typeof(Kitware.VTK.vtkObjectMarshaler))]
        Kitware.VTK.vtkObject caller, uint eventId, System.IntPtr callData)
    {
        Debug.Log("Executed!");
    }
}
