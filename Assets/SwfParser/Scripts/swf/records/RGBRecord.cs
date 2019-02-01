using UnityEngine;
using System.Collections;
using System;

public struct RGBRecord {
	public uint red;
	public uint green;
	public uint blue;

	override public string ToString() {
		string r=Convert.ToString(red,16);
		string g=Convert.ToString(green,16);
		string b=Convert.ToString(blue,16);
		if(r.Length<2)r="0"+r;
		if(g.Length<2)g="0"+g;
		if(b.Length<2)b="0"+b;
		return r+g+b;
	}
}
