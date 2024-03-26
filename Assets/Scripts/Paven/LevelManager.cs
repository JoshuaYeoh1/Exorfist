using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //by default, all RSMs, except for the INITIAL RSM, should have their gameObjects set to "false", so that their code does not get executed.
    public List<RoomStateManager> roomStateManagers;
    public RoomStateManager activeRSM;

    void Start()
    {
        GameEventSystem.Current.RoomStateChangedEvent += CycleRoomStateManager;

        if(roomStateManagers != null) 
        {
            activeRSM = roomStateManagers[0];
        }

        SetRoomIDS();
    }
    void OnDestroy()
    {
        GameEventSystem.Current.RoomStateChangedEvent -= CycleRoomStateManager;
    }

    private void SetRoomIDS()
    {
        for (int i = 0; i < roomStateManagers.Count; i++)
        {
            roomStateManagers[i].SetRoomID(i);
        }
    }

    private void CycleRoomStateManager(RoomState roomState)
    {
        if (roomState == RoomState.Clear)
        {
            Debug.Log("Cycling RSM");

            int nextIndex = activeRSM.GetRoomID() + 1;

            if (nextIndex < roomStateManagers.Count)
            {
                activeRSM = roomStateManagers[nextIndex];
                activeRSM.gameObject.SetActive(true);
            }
            else
            {
                //Level is technically complete here.
                //Display "victory" or whatever.
            }
        }
    }
}
