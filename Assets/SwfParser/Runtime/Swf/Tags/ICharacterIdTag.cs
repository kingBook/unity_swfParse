using System.Collections.Generic;

public interface ICharacterIdTag {

    void FindUsedCharacterIds(List<ushort> characterIds, Swf swf);

    ushort GetCharacterId();

}