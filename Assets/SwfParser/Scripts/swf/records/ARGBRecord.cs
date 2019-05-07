using System;
using System.Text;

public struct ARGBRecord {
	public byte alpha;
	public byte red;
	public byte green;
	public byte blue;

	override public string ToString() {
		uint color=alpha;
		color=(color<<8)|red;
		color=(color<<8)|green;
		color=(color<<8)|blue;
		string str=Convert.ToString(color,16);
		byte headZeroCount=(byte)(8-str.Length);
		for(byte i=0;i<headZeroCount;i++)str='0'+str;
		return str;
	}
}
