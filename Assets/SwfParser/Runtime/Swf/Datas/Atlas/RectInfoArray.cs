[System.Serializable]
public class RectInfoArray {

    public RectInfo[] rectInfos;

    public RectInfoArray(int length) {
        rectInfos = new RectInfo[length];
    }

    public RectInfo this[int index] {
        get { return rectInfos[index]; }
        set { rectInfos[index] = value; }
    }
}