using System;
public struct RGBARecord {
	public byte red;
	public byte green;
	public byte blue;
	public byte alpha;

	override public string ToString() {
		string r=Convert.ToString(red,16);
		string g=Convert.ToString(green,16);
		string b=Convert.ToString(blue,16);
		string a=Convert.ToString(alpha,16);
		if(r.Length<2)r="0"+r;
		if(g.Length<2)g="0"+g;
		if(b.Length<2)b="0"+b;
		if(a.Length<2)a="0"+a;
		return r+g+b+a;
	}
}
