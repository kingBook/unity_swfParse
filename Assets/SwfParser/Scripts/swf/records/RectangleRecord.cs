using UnityEngine;
using System.Collections;

public struct RectangleRecord{
	public int xMin;
	public int xMax;
	public int yMin;
	public int yMax;

	override public string ToString(){
        return xMin+","+yMin+","+xMax+","+yMax;
    }
}
