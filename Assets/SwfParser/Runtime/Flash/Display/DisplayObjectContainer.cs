public abstract class DisplayObjectContainer : DisplayObject {

    public int numChildren;

    public DisplayObject AddChild(DisplayObject child) {
        return null;
    }

    public DisplayObject AddChildAt(DisplayObject child, int index) {
        return null;
    }

    public bool Contains(DisplayObject child) {
        return false;
    }

    public DisplayObject GetChildAt(int index) {
        return null;
    }

    public DisplayObject GetChildByName(string name) {
        return null;
    }

    public int GetChildIndex(DisplayObject child) {
        return 0;
    }

    public DisplayObject RemoveChild(DisplayObject child) {
        return null;
    }

    public DisplayObject RemoveChildAt(int index) {
        return null;
    }

    public void RemoveChildren(int beginIndex = 0, int endIndex = 0x7fffffff) {

    }

    public void SetChildIndex(DisplayObject child, int index) {

    }

    public void StopAllMovieClips() {

    }

    public void SwapChildren(DisplayObject child1, DisplayObject child2) {

    }

    public void SwapChildrenAt(int index1, int index2) {

    }
}