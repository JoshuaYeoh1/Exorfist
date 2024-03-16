using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInfo
{
    public string lootName { get; set; }
    public int quantity { get; set; }
    public Vector3 contactPoint { get; set; }
    
    public LootInfo()
    {
        lootName="";
        quantity=0;
        contactPoint = Vector3.zero;
    }
}
