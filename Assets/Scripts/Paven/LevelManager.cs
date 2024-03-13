using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public RoomStateManager[] roomStateManagers;
    public RoomStateManager activeRSM;

    [SerializeField] private GameObject popUpPrefab;
    private GameObject popUpPrefabRef;
    
}
