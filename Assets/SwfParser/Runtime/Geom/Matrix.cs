using UnityEngine;

public class Matrix {

    public float a;
    public float b;
    public float c;
    public float d;
    public float tx;
    public float ty;

    public Matrix(float a = 1, float b = 0, float c = 0, float d = 1, float tx = 0, float ty = 0) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.tx = tx;
        this.ty = ty;
    }

    public Matrix Clone() {
        return new Matrix(a, b, c, d, tx, ty);
    }

    public void Concat(Matrix m) {
        var a1 = a * m.a + b * m.c;
        b = a * m.b + b * m.d;
        a = a1;

        var c1 = c * m.a + d * m.c;
        d = c * m.b + d * m.d;
        c = c1;

        var tx1 = tx * m.a + ty * m.c + m.tx;
        ty = tx * m.b + ty * m.d + m.ty;
        tx = tx1;
    }

    public void CopyColumnFrom(int column, Vector3D vector3D) {
        if (column > 2) {
            throw new System.Exception("Column " + column + " out of bounds (2)");
        } else if (column == 0) {
            a = vector3D.x;
            b = vector3D.y;
        } else if (column == 1) {
            c = vector3D.x;
            d = vector3D.y;
        } else {
            tx = vector3D.x;
            ty = vector3D.y;
        }
    }

    public void CopyColumnTo(int column, Vector3D vector3D) {
        if (column > 2) {
            throw new System.Exception("Column " + column + " out of bounds (2)");
        } else if (column == 0) {
            vector3D.x = a;
            vector3D.y = b;
            vector3D.z = 0;
        } else if (column == 1) {
            vector3D.x = c;
            vector3D.y = d;
            vector3D.z = 0;
        } else {
            vector3D.x = tx;
            vector3D.y = ty;
            vector3D.z = 1;
        }
    }

    public void CopyFrom(Matrix sourceMatrix) {
        a = sourceMatrix.a;
        b = sourceMatrix.b;
        c = sourceMatrix.c;
        d = sourceMatrix.d;
        tx = sourceMatrix.tx;
        ty = sourceMatrix.ty;
    }

    public void CopyRowFrom(int row, Vector3D vector3D) {
        if (row > 2) {
            throw new System.Exception("Row " + row + " out of bounds (2)");
        } else if (row == 0) {
            a = vector3D.x;
            c = vector3D.y;
            tx = vector3D.z;
        } else if (row == 1) {
            b = vector3D.x;
            d = vector3D.y;
            ty = vector3D.z;
        }
    }

    public void CopyRowTo(int row, Vector3D vector3D) {
        if (row > 2) {
            throw new System.Exception("Row " + row + " out of bounds (2)");
        } else if (row == 0) {
            vector3D.x = a;
            vector3D.y = c;
            vector3D.z = tx;
        } else if (row == 1) {
            vector3D.x = b;
            vector3D.y = d;
            vector3D.z = ty;
        } else {
            vector3D.SetTo(0, 0, 1);
        }
    }

    public void CreateBox(float scaleX, float scaleY, float rotation = 0, float tx = 0, float ty = 0) {
        if (rotation != 0) {
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);

            a = cos * scaleX;
            b = sin * scaleY;
            c = -sin * scaleX;
            d = cos * scaleY;
        } else {
            a = scaleX;
            b = 0;
            c = 0;
            d = scaleY;
        }

        this.tx = tx;
        this.ty = ty;
    }

    public void CreateGradientBox(float width, float height, float rotation = 0, float tx = 0, float ty = 0) {
        a = width / 1638.4f;
        d = height / 1638.4f;

        // rotation is clockwise
        if (rotation != 0) {
            var cos = Mathf.Cos(rotation);
            var sin = Mathf.Sin(rotation);

            b = sin * d;
            c = -sin * a;
            a *= cos;
            d *= cos;
        } else {
            b = 0;
            c = 0;
        }

        this.tx = tx + width / 2;
        this.ty = ty + height / 2;
    }

    public Vector2 DeltaTransformPoint(Vector2 point) {
        return new Vector2(point.x * a + point.y * c, point.x * b + point.y * d);
    }

    public void Identity() {
        a = 1;
        b = 0;
        c = 0;
        d = 1;
        tx = 0;
        ty = 0;
    }

    public Matrix Invert() {
        var norm = a * d - b * c;

        if (norm == 0f) {
            a = b = c = d = 0f;
            tx = -tx;
            ty = -ty;
        } else {
            norm = 1.0f / norm;
            var a1 = d * norm;
            d = a * norm;
            a = a1;
            b *= -norm;
            c *= -norm;

            var tx1 = -a * tx - c * ty;
            ty = -b * tx - d * ty;
            tx = tx1;
        }
        return this;
    }

    public void Rotate(float theta) {
        /**
			Rotate object "after" other transforms
            
			[  a  b   0 ][  ma mb  0 ]
			[  c  d   0 ][  mc md  0 ]
			[  tx ty  1 ][  mtx mty 1 ]

			ma = md = cos
			mb = sin
			mc = -sin
			mtx = my = 0
		**/

        var cos = Mathf.Cos(theta);

        var sin = Mathf.Sin(theta);

        var a1 = a * cos - b * sin;
        b = a * sin + b * cos;
        a = a1;

        var c1 = c * cos - d * sin;
        d = c * sin + d * cos;
        c = c1;

        var tx1 = tx * cos - ty * sin;
        ty = tx * sin + ty * cos;
        tx = tx1;
    }

    public void Scale(float sx, float sy) {
        /*
			Scale object "after" other transforms
            
			[  a  b   0 ][  sx  0   0 ]
			[  c  d   0 ][  0   sy  0 ]
			[  tx ty  1 ][  0   0   1 ]
		**/
        a *= sx;
        b *= sy;
        c *= sx;
        d *= sy;
        tx *= sx;
        ty *= sy;
    }

    public void SetTo(float a, float b, float c, float d, float tx, float ty) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.tx = tx;
        this.ty = ty;
    }

    public override string ToString() {
        return $"(a={a}, b={b}, c={c}, d={d}, tx={tx}, ty={ty})";
    }

    public Vector2 TransformPoint(Vector2 pos) {
        return new Vector2(pos.x * a + pos.y * c + tx, pos.x * b + pos.y * d + ty);
    }

    public void Translate(float dx, float dy) {
        tx += dx;
        ty += dy;
    }

}