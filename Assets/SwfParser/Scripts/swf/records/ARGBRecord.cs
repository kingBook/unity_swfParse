using System;
public struct ARGBRecord {
	public byte alpha;
	public byte red;
	public byte green;
	public byte blue;

	override public string ToString() {
		string a=Convert.ToString(alpha,16);
		string r=Convert.ToString(red,16);
		string g=Convert.ToString(green,16);
		string b=Convert.ToString(blue,16);
		if(a.Length<2)a="0"+a;
		if(r.Length<2)r="0"+r;
		if(g.Length<2)g="0"+g;
		if(b.Length<2)b="0"+b;
		return a+r+g+b;
	}
}
