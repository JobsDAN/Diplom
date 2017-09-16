using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
    public bool IsOccupied { get; private set; }
    public bool IsSelected { get; set; }
    public Vector3 Position { get; private set; }
	
    public Cell(Vector3 position, bool isOccupied = false)
    {
        Position = position;
        IsOccupied = isOccupied;
        IsSelected = false;
    }
}
