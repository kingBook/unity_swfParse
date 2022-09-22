using System.Collections.Generic;

public interface ICharacterIdTag {

    void GetNeededCharacterIds(List<ushort> characterIds, Swf swf);

    ushort GetCharacterId();

    RuntimeTagData ToRuntimeData();

}