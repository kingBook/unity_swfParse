using System.Collections.Generic;
using UnityEngine;

public class SwfData : ScriptableObject {

    public TagTypeAndIndex[] tagTypeAndIndices;


    public List<DefineShapeTagData> defineShapeTagDatas = new List<DefineShapeTagData>(64);

    public List<DefineBitsTagData> defineBitsTagDatas = new List<DefineBitsTagData>(64);

    public List<PlaceObjectTagData> placeObjectTagDatas = new List<PlaceObjectTagData>(64);
    public List<PlaceObject2TagData> placeObject2TagDatas = new List<PlaceObject2TagData>(64);
    public List<PlaceObject3TagData> placeObject3TagDatas = new List<PlaceObject3TagData>(64);

    public List<UnknownTagData> unknownTagDatas = new List<UnknownTagData>(64);


}