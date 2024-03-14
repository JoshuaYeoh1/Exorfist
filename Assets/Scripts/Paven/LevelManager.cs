using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //by default, all RSMs, except for the INITIAL RSM, should have their gameObjects set to "false", so that their code does not get executed.
    public List<RoomStateManager> roomStateManagers;
    public RoomStateManager activeRSM;

    private void OnEnable()
    {
        if(roomStateManagers != null) 
        {
            activeRSM = roomStateManagers[0];
        }
        GameEventSystem.Current.RoomStateChangedEvent += CycleRoomStateManager;
    }

    private void OnDisable()
    {
        GameEventSystem.Current.RoomStateChangedEvent -= CycleRoomStateManager;
    }

    private void CycleRoomStateManager(RoomState roomState)
    {
        if(roomState == RoomState.Clear)
        {
            Debug.Log("Cycling RSM");
            activeRSM = roomStateManagers[activeRSM.GetRoomID() + 1];
            activeRSM.gameObject.SetActive(true);
        }
    }
}
