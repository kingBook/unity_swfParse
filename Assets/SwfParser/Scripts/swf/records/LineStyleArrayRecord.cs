using System.Collections;

public struct LineStyleArrayRecord {
	public byte lineStyleCount;
	public ushort lineStyleCountExtended;

	/*If Shape1,Shape2, or Shape3, LINESTYLE[count]. 
	  If Shape4,LINESTYLE2[count] */
	public ArrayList lineStyles;
	
}
