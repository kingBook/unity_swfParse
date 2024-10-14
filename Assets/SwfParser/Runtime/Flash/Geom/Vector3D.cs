using UnityEngine;

public class Vector3D {

    public static Vector3D X_AXIS = new Vector3D(1, 0, 0);

    public static Vector3D Y_AXIS = new Vector3D(0, 1, 0);

    public static Vector3D Z_AXIS = new Vector3D(0, 0, 1);

    public float length => Mathf.Sqrt(x * x + y * y + z * z);

    public float lengthSquared => x * x + y * y + z * z;

    public float w;

    public float x;

    public float y;

    public float z;


    public Vector3D(float x = 0, float y = 0, float z = 0, float w = 0) {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3D Add(Vector3D a) {
        return new Vector3D(x + a.x, y + a.y, z + a.z);
    }

    public Vector3D AddToOutput(Vector3D a, Vector3D output) {
        if (output != null) {
            output.SetTo(x + a.x, y + a.y, z + a.z);
            return output;
        }
        return new Vector3D(x + a.x, y + a.y, z + a.z);
    }

    public static float AngleBetween(Vector3D a, Vector3D b) {
        var la = a.length;
        var lb = b.length;
        var dot = a.DotProduct(b);

        if (la != 0) {
            dot /= la;
        }

        if (lb != 0) {
            dot /= lb;
        }

        return Mathf.Acos(dot);
    }

    public Vector3D Clone() {
        return new Vector3D(x, y, z, w);
    }

    public void CopyFrom(Vector3D sourceVector3D) {
        x = sourceVector3D.x;
        y = sourceVector3D.y;
        z = sourceVector3D.z;
    }

    public Vector3D CrossProduct(Vector3D a) {
        return new Vector3D(y * a.z - z * a.y, z * a.x - x * a.z, x * a.y - y * a.x, 1);
    }

    public Vector3D CrossProductToOutput(Vector3D a, Vector3D output) {
        if (output != null) {
            output.SetTo(y * a.z - z * a.y, z * a.x - x * a.z, x * a.y - y * a.x);
            output.w = 1;
            return output;
        }
        return new Vector3D(y * a.z - z * a.y, z * a.x - x * a.z, x * a.y - y * a.x, 1);
    }

    public void DecrementBy(Vector3D a) {
        x -= a.x;
        y -= a.y;
        z -= a.z;
    }

    public static float Distance(Vector3D pt1, Vector3D pt2) {
        var x = pt2.x - pt1.x;
        var y = pt2.y - pt1.y;
        var z = pt2.z - pt1.z;

        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    public float DotProduct(Vector3D a) {
        return x * a.x + y * a.y + z * a.z;
    }

    public bool Equals(Vector3D toCompare, bool allFour = false) {
        return x == toCompare.x && y == toCompare.y && z == toCompare.z && (!allFour || w == toCompare.w);
    }

    public void IncrementBy(Vector3D a) {
        x += a.x;
        y += a.y;
        z += a.z;
    }

    public bool NearEquals(Vector3D toCompare, float tolerance, bool allFour = false) {
        return Mathf.Abs(x - toCompare.x) < tolerance
            && Mathf.Abs(y - toCompare.y) < tolerance
            && Mathf.Abs(z - toCompare.z) < tolerance
            && (!allFour || Mathf.Abs(w - toCompare.w) < tolerance);
    }

    public void Negate() {
        x *= -1;
        y *= -1;
        z *= -1;
    }

    public float Normalize() {
        var l = length;

        if (l != 0) {
            x /= l;
            y /= l;
            z /= l;
        }

        return l;
    }

    public void Project() {
        x /= w;
        y /= w;
        z /= w;
    }

    public void ScaleBy(float s) {
        x *= s;
        y *= s;
        z *= s;
    }

    public void SetTo(float xa, float ya, float za) {
        x = xa;
        y = ya;
        z = za;
    }

    public Vector3D Subtract(Vector3D a) {
        return new Vector3D(x - a.x, y - a.y, z - a.z);
    }

    public Vector3D SubtractToOutput(Vector3D a, Vector3D output) {
        if (output != null) {
            output.SetTo(x - a.x, y - a.y, z - a.z);
            return output;
        }
        return new Vector3D(x - a.x, y - a.y, z - a.z);
    }

    public override string ToString() {
        return $"Vector3D({x}, {y}, {z})";
    }

}