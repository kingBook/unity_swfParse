using UnityEngine;

public abstract class DisplayObject : MonoBehaviour {

    public Mesh mesh { get; protected set; }
    //
    public float alpha;
    //public filters
    //public DisplayObject mask;
    //public string name;
    public DisplayObjectContainer parent;
    public float rotation;
    public float scaleX;
    public float scaleY;
    public Transform transformFl;
    public bool visible;
    public float width;
    public float height;
    public float x;
    public float y;

    public Rect GetBounds() {
        return new Rect();
    }

    public Rect GetRect() {
        return new Rect();
    }

    public Vector2 GlobalToLocal(Vector2 point) {
        return Vector2.zero;
    }

    public Vector2 LocalToGlobal(Vector2 point) {
        return Vector2.zero;
    }

}