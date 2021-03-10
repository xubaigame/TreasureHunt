using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitElement : CantCoverElement
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Exit;
        LoadSprite(MapManager.Instance.mapData.Exit);
    }
}
