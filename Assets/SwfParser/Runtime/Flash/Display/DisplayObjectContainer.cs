using System.Collections.Generic;

public abstract class DisplayObjectContainer : DisplayObject {

    protected List<DisplayObject> m_children;
    protected List<DisplayObject> m_removeChildren;

    public int numChildren => m_children.Count;

    public DisplayObjectContainer() : base() {
        m_children = new List<DisplayObject>();
        m_removeChildren = new List<DisplayObject>();
    }

    public DisplayObject AddChild(DisplayObject child) {
        return AddChildAt(child, numChildren);
    }

    public DisplayObject AddChildAt(DisplayObject child, int index) {
        if (child == null) {
            throw new System.Exception("Parameter child must be non-null.");
        } else if (child == this) {
            throw new System.Exception("An object cannot be added as a child of itself.");
        }

        if (index > m_children.Count || index < 0) {
            throw new System.Exception("Invalid index position " + index);
        }

        if (child.parent == this) {
            if (m_children[index] != child) {
                m_children.Remove(child);
                m_children.Insert(index, child);
            }
        } else {
            if (child.parent != null) {
                child.parent.RemoveChild(child);
            }

            m_children.Insert(index, child);
            child.parent = this;
        }
        return child;
    }

    public bool Contains(DisplayObject child) {
        while (child != this && child != null) {
            child = child.parent;
        }
        return child == this;
    }

    public DisplayObject GetChildAt(int index) {
        if (index >= 0 && index < m_children.Count) {
            return m_children[index];
        }
        return null;
    }

    public DisplayObject GetChildByName(string name) {
        foreach (var child in m_children) {
            if (child.name == name) return child;
        }
        return null;
    }

    public int GetChildIndex(DisplayObject child) {
        for (int i = 0, len = m_children.Count; i < len; i++) {
            if (m_children[i] == child) return i;
        }
        return -1;
    }

    public DisplayObject RemoveChild(DisplayObject child) {
        if (child != null && child.parent == this) {
            child.parent = null;
            m_children.Remove(child);
            m_removeChildren.Add(child);
        }
        return child;
    }

    public DisplayObject RemoveChildAt(int index) {
        if (index >= 0 && index < m_children.Count) {
            return RemoveChild(m_children[index]);
        }
        return null;
    }

    public void RemoveChildren(int beginIndex = 0, int endIndex = 0x7fffffff) {
        if (endIndex == 0x7FFFFFFF) {
            endIndex = m_children.Count - 1;

            if (endIndex < 0) {
                return;
            }
        }

        if (beginIndex > m_children.Count - 1) {
            return;
        } else if (endIndex < beginIndex || beginIndex < 0 || endIndex > m_children.Count) {
            throw new System.Exception("The supplied index is out of bounds.");
        }

        var numRemovals = endIndex - beginIndex;
        while (numRemovals >= 0) {
            RemoveChildAt(beginIndex);
            numRemovals--;
        }
    }

    public void SetChildIndex(DisplayObject child, int index) {
        if (index >= 0 && index <= m_children.Count && child.parent == this) {
            m_children.Remove(child);
            m_children.Insert(index, child);
        }
    }

    public void StopAllMovieClips() {

    }

    public void SwapChildren(DisplayObject child1, DisplayObject child2) {
        if (child1.parent == this && child2.parent == this) {
            var index1 = m_children.IndexOf(child1);
            var index2 = m_children.IndexOf(child2);

            m_children[index1] = child2;
            m_children[index2] = child1;
        }
    }

    public void SwapChildrenAt(int index1, int index2) {
        var swap = m_children[index1];
        m_children[index1] = m_children[index2];
        m_children[index2] = swap;
    }
}