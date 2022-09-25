using System.Collections.Generic;
using UnityEngine;

public class SwfData : ScriptableObject {

    public TagTypeAndIndex[] tagTypeAndIndices;


    public List<DefineShapeTagData> defineShapeTagDatas = new List<DefineShapeTagData>(256);

    public List<DefineBitsTagData> defineBitsTagDatas = new List<DefineBitsTagData>(256);

    public List<DefineSpriteTagData> defineSpriteTagDatas = new List<DefineSpriteTagData>(256);
    
    public List<UnknownTagData> unknownTagDatas = new List<UnknownTagData>(64);


}